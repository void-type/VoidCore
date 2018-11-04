. ./util.ps1

# Build
Push-Location -Path "../"
dotnet build --configuration "Release" /p:PublicRelease=true
Stop-OnError
Pop-Location

# Run tests, generate code coverage report
Push-Location -Path "../VoidCore.Test"
dotnet test --configuration "Release" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="./coveragereport/coverage.opencover.xml"
reportgenerator "-reports:coveragereport/coverage.opencover.xml" "-targetdir:coveragereport"
Stop-OnError
Pop-Location

# Pack nugets
"../VoidCore.Model",
"../VoidCore.AspNet" |
  ForEach-Object {
  Push-Location -Path "$_"
  Remove-Item -Path "out" -Recurse -ErrorAction SilentlyContinue
  InheritDoc --base "./bin/Release/" --overwrite
  dotnet pack --configuration "Release" --no-build --output "out" /p:PublicRelease=true
  Stop-OnError
  Pop-Location
}
