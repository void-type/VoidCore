[CmdletBinding()]
param(
  [string] $Configuration = 'Release',
  [switch] $SkipFormat,
  [switch] $SkipOutdated,
  [switch] $SkipTest,
  [switch] $SkipTestReport,
  [switch] $SkipPack
)

function Stop-OnError([string]$errorMessage) {
  if ($LASTEXITCODE -eq 0) {
    return
  }

  if (-not [string]::IsNullOrWhiteSpace($errorMessage)) {
    Write-Error $errorMessage
  }

  exit $LASTEXITCODE
}

$stopwatch = New-Object System.Diagnostics.Stopwatch
$stopwatch.Start()

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  # Clean the artifacts folders
  Remove-Item -Path './artifacts' -Recurse -ErrorAction SilentlyContinue

  # Restore local dotnet tools
  dotnet tool restore
  Stop-OnError

  # Build solution
  if (-not $SkipFormat) {
    # Don't stop build for TODOS
    dotnet format --verify-no-changes --exclude-diagnostics S1135
    Stop-OnError 'Please run formatter: dotnet format.'
  }

  dotnet restore
  Stop-OnError

  if (-not $SkipOutdated) {
    dotnet outdated
    dotnet list package --vulnerable --include-transitive
    Stop-OnError
  }

  dotnet build --configuration "$Configuration" --no-restore
  Stop-OnError

  if (-not $SkipTest) {
    # Run tests, gather coverage
    dotnet test "$testProjectFolder" `
      --configuration "$Configuration" `
      --no-build `
      --results-directory './artifacts/testResults' `
      --logger 'trx' `
      --collect:'XPlat Code Coverage'
    Stop-OnError

    if (-not $SkipTestReport) {
      # Generate code coverage report
      dotnet reportgenerator `
        '-reports:./artifacts/testResults/*/coverage.cobertura.xml' `
        '-targetdir:./artifacts/testCoverage' `
        '-reporttypes:HtmlInline_AzurePipelines' `
        '-filefilters:-*.g.cs'
      Stop-OnError
    }
  }

  if (-not $SkipPack) {
    # Pack nugets for each package
    Get-ChildItem -Path './src' |
      Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
      ForEach-Object {
        Set-Location -Path $_.FullName

        # Pack pre-release and release version
        dotnet pack --configuration "$Configuration" --no-build --output '../../artifacts/dist/pre-release' /p:PublicRelease=false
        dotnet pack --configuration "$Configuration" --no-build --output '../../artifacts/dist/release'
        Stop-OnError
      }
  }

  $projectVersion = (dotnet nbgv get-version -f json | ConvertFrom-Json).NuGetPackageVersion

  $stopwatch.Stop()

  Write-Output "`nBuilt $projectName $projectVersion in $($stopwatch.Elapsed)`n"

} finally {
  Set-Location $originalLocation
}
