[CmdletBinding()]
param(
  [string] $Configuration = "Release",
  [switch] $Quick
)

. ./util.ps1

# Clean the artifacts folder
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Build solution
Push-Location -Path "../"
dotnet build --configuration "$Configuration"
Stop-OnError
Pop-Location

if ($Quick) {
  Exit $LASTEXITCODE
}

./test.ps1 -Configuration "$Configuration"
Stop-OnError

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
