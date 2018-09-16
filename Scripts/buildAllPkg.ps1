function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Exit $LASTEXITCODE
  }
}

# Tests
./testCore.ps1
Stop-OnError

# Build VoidCore.Model
Push-Location -Path "../VoidCore.Model"
Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
dotnet build --configuration "Release"
Stop-OnError
InheritDoc --base "bin" --overwrite
Stop-OnError
dotnet pack --configuration "Release" --no-build --output "out"
Pop-Location
Stop-OnError

# Build VoidCore.AspNet
Push-Location -Path "../VoidCore.AspNet"
Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
dotnet build --configuration "Release"
Stop-OnError
InheritDoc --base "bin" --overwrite
Stop-OnError
dotnet pack --configuration "Release" --no-build --output "out"
Pop-Location
