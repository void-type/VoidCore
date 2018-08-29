function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Exit $LASTEXITCODE
  }
}

function Main {
  ./testCore.ps1
  Stop-OnError
  Push-Location -Path "../VoidCore.Model"
  Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
  dotnet build --configuration "Release"
  Stop-OnError
  ~\.nuget\packages\inheritdoc\1.2.0.1\tools\InheritDoc.exe --base "bin" --overwrite
  Stop-OnError
  dotnet pack --configuration "Release" --no-build --output "out"
  Pop-Location
  Stop-OnError
  Push-Location -Path "../VoidCore.AspNet"
  Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
  dotnet build --configuration "Release"
  Stop-OnError
  ~\.nuget\packages\inheritdoc\1.2.0.1\tools\InheritDoc.exe --base "bin" --overwrite
  Stop-OnError
  dotnet pack --configuration "Release" --no-build --output "out"
  Pop-Location
}

Main
