dotnet tool install --global dotnet-outdated
# Waiting on stable version
dotnet tool install --global dotnet-reportgenerator-globaltool --version "4.0.0-rc4"
dotnet tool install --global InheritDocTool
dotnet tool install --global coverlet.console

dotnet tool update --global dotnet-outdated
dotnet tool update --global dotnet-reportgenerator-globaltool
dotnet tool update --global InheritDocTool
dotnet tool update --global coverlet.console
