[CmdletBinding()]
param(
)

$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  dotnet format
} finally {
  Set-Location $originalLocation
}
