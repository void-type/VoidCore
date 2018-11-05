. ./util.ps1

# Build
Push-Location -Path "../"
dotnet build --configuration "Debug"
Stop-OnError
Pop-Location

# Run tests, generate code coverage report
Push-Location -Path "../tests/VoidCore.Test"
dotnet test --configuration "Debug" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="./coveragereport/coverage.opencover.xml"
reportgenerator "-reports:coveragereport/coverage.opencover.xml" "-targetdir:coveragereport"
Stop-OnError
Pop-Location
