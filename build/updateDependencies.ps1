$originalLocation = Get-Location
$projectRoot = "$PSScriptRoot/../"

try {
  Set-Location -Path $projectRoot
  . ./build/buildSettings.ps1

  ./build/updateTools.ps1
  dotnet outdated -u

  if ($LASTEXITCODE -ne 0) {
    Write-Warning "You need to update the Directory.build.props files by hand to update the following packages."
    dotnet outdated
  }

} finally {
  Set-Location -Path $originalLocation
}
