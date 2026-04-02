@echo off
dotnet publish -p:PublishProfile=FolderProfile_win-x64.pubxml -f net11.0-windows
explorer "..\..\publish\dotnet11\win-x64"
pause