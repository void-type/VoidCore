Push-Location -Path "../VoidCore.Test"
dotnet watch test -p:CollectCoverage=true
Pop-Location
