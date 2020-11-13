[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPack
)

Push-Location $PSScriptRoot

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Restore local dotnet tools
Push-Location -Path "../"
dotnet tool restore
Pop-Location

. ./util.ps1

# Build solution
Push-Location -Path "../"

if (-not $SkipFormat) {
  dotnet format --check
  Stop-OnError
}

dotnet restore

if (-not $SkipOutdated) {
  dotnet outdated
}

# Run Analyzers through building debug
if ($Configuration -ne "Debug") {
  dotnet build --configuration "Debug" --no-restore #-warnaserror
  Stop-OnError
}

dotnet build --configuration "$Configuration" --no-restore
Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "$testProjectFolder"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --results-directory '../../testResults' `
    --logger 'trx' `
    --collect:"XPlat Code Coverage"

  Stop-OnError

  if (-not $SkipTestReport) {
    # Generate code coverage report
    dotnet reportgenerator `
      "-reports:../../testResults/*/coverage.cobertura.xml" `
      "-targetdir:../../coverage" `
      "-reporttypes:HtmlInline_AzurePipelines"
    Stop-OnError
  }

  Pop-Location
}

if (-not $SkipPack) {
  # Pack nugets for each package
  Get-ChildItem -Path "../src" |
    Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
    Select-Object -ExpandProperty Name |
    ForEach-Object {
      Push-Location -Path "../src/$_"

      # Run inheritdoc
      dotnet InheritDoc --base "./bin/$Configuration/" --overwrite
      Stop-OnError

      # Pack pre-release and release version
      dotnet pack --configuration "$Configuration" --no-build --output "../../artifacts/pre-release" /p:PublicRelease=false
      dotnet pack --configuration "$Configuration" --no-build --output "../../artifacts"
      Stop-OnError

      Pop-Location
    }
}

Pop-Location

Write-Host "`nBuilt $projectName $projectVersion`n"
