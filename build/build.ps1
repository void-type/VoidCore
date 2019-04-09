[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPack
)

. ./util.ps1

# Clean the artifacts folders
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Build solution
Push-Location -Path "../"
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

if (-not $SkipTest) {
  # Run tests, gather coverage
  Push-Location -Path "$testProjectFolder"

  dotnet test `
    --configuration "$Configuration" `
    --no-build `
    --logger 'trx' `
    --results-directory '../../testResults' `
    /p:Exclude='[xunit.*]*' `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=cobertura `
    /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

  Stop-OnError
  Pop-Location

  if (-not $SkipTestReport) {
    # Generate code coverage report
    Push-Location -Path "../coverage"
    reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
    Stop-OnError
    Pop-Location
  }
}

if (-not $SkipPack) {
  # Pack nugets
  Get-ChildItem -Path "../src" |
    Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
    Select-Object -ExpandProperty Name |
    ForEach-Object {
    Push-Location -Path "../src/$_"
    InheritDoc --base "./bin/$Configuration/" --overwrite
    Stop-OnError
    dotnet pack --configuration "$Configuration" --no-build --output "../../artifacts/pre-release" /p:PublicRelease=false
    dotnet pack --configuration "$Configuration" --no-build --output "../../artifacts"
    Stop-OnError
    Pop-Location
  }
}
