Push-Location -Path "../VoidCore.Test"
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover /p:CoverletOutput="./coverage.opencover.xml"
Pop-Location
