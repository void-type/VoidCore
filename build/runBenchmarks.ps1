$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet run --project "$benchmarkProjectFolder" --configuration 'Release'

} finally {
  Set-Location $originalLocation
}
