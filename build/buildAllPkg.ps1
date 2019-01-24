. ./util.ps1

# Clean the artifacts folder
Remove-Item -Path "../artifacts" -Recurse -ErrorAction SilentlyContinue

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Clean testResults folder
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Build solution
Push-Location -Path "../"
dotnet build --configuration "Release"
Stop-OnError
Pop-Location

# Run tests, gather coverage
Push-Location -Path "../tests/VoidCore.Test"

dotnet test `
  --configuration "Release" `
  --no-build `
  --logger 'trx' `
  --results-directory '../../testResults' `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=cobertura `
  /p:CoverletOutput="../../coverage/coverage.cobertura.xml"

Stop-OnError
Pop-Location

# Generate code coverage report
Push-Location -Path "../coverage"
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:."
Stop-OnError
Pop-Location

# Pack nugets
Get-ChildItem -Path "../src" |
  Where-Object { (Test-Path -Path "$($_.FullName)/*.csproj") -eq $true } |
  Select-Object -ExpandProperty Name |
  ForEach-Object {
  Push-Location -Path "../src/$_"
  InheritDoc --base "./bin/Release/" --overwrite
  Stop-OnError
  dotnet pack --configuration "Release" --no-build --output "../../artifacts"
  Stop-OnError
  Pop-Location
}
