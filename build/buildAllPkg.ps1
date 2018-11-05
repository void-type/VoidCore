. ./util.ps1

# Build solution
Push-Location -Path "../"
dotnet build --configuration "Release" /p:PublicRelease=true
Stop-OnError
Pop-Location

# Run tests, generate code coverage report
Push-Location -Path "../tests/VoidCore.Test"
dotnet test --configuration "Release" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="./coveragereport/coverage.opencover.xml"
reportgenerator "-reports:coveragereport/coverage.opencover.xml" "-targetdir:coveragereport"
Stop-OnError
Pop-Location

Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Pack nugets
"../src/VoidCore.Model",
"../src/VoidCore.AspNet" |
  ForEach-Object {
  Push-Location -Path "$_"
  InheritDoc --base "./bin/Release/" --overwrite
  dotnet pack --configuration "Release" --no-build --output "../../artifacts" /p:PublicRelease=true
  Stop-OnError
  Pop-Location
}
