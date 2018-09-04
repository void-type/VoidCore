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
  dotnet tool install -g InheritDocTool --version 2.0.0
  InheritDoc --base "bin" --overwrite
  Stop-OnError
  dotnet pack --configuration "Release" --no-build --output "out"
  Pop-Location
  Stop-OnError
  Push-Location -Path "../VoidCore.AspNet"
  Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
  dotnet build --configuration "Release"
  Stop-OnError
  InheritDoc --base "bin" --overwrite
  Stop-OnError
  dotnet pack --configuration "Release" --no-build --output "out"
  Pop-Location
}

Main
