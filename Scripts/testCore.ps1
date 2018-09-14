function Stop-OnError {
  if ($LASTEXITCODE -ne 0) {
    Exit $LASTEXITCODE
  }
}

Push-Location -Path "../VoidCore.Test"
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover /p:CoverletOutput="./coverage.opencover.xml"
Stop-OnError
reportgenerator "-reports:coverage.opencover.xml" "-targetdir:coveragereport"
Pop-Location
