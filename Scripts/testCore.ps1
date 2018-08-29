Push-Location -Path "../VoidCore.Test"
dotnet test -p:CollectCoverage=true
Pop-Location
