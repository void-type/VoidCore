Push-Location -Path "../VoidCore.Test"
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover /p:CoverletOutput="./coverage.opencover.xml"
dotnet tool install --global dotnet-reportgenerator-globaltool --version 4.0.0-rc4
reportgenerator "-reports:coverage.opencover.xml" "-targetdir:coveragereport"
Pop-Location
