. ./util.ps1

# Clean coverage folder
Remove-Item -Path "../coverage" -Recurse -ErrorAction SilentlyContinue

# Clean testResults folder
Remove-Item -Path "../testResults" -Recurse -ErrorAction SilentlyContinue

# Build
Push-Location -Path "../"
dotnet build --configuration "Debug"
Stop-OnError
Pop-Location

# Run tests, gather coverage
Push-Location -Path "../tests/VoidCore.Test"

dotnet test `
  --configuration "Debug" `
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
reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:." "-reporttypes:HtmlInline_AzurePipelines"
Stop-OnError
Pop-Location
