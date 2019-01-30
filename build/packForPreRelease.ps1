. ./util.ps1

# Pack pre-release nugets
Get-ChildItem -Path "../src" |
  Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
  Select-Object -ExpandProperty Name |
  ForEach-Object {
  Push-Location -Path "../src/$_"
  dotnet pack --configuration "Release" --no-build --output "../../artifacts/pre-release" /p:PublicRelease=false
  Stop-OnError
  Pop-Location
}
