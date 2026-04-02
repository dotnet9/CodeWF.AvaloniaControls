@echo off
dotnet publish -p:PublishProfile=FolderProfile_linux-x64.pubxml -f net11.0
explorer "..\..\publish\dotnet11\linux-x64"
pause