$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  Push-Location "$testProjectFolder"

  dotnet watch test --configuration 'Debug'

} finally {
  Set-Location $originalLocation
}
