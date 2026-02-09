@echo off
dotnet run --project build/_build.csproj -- %*
exit /b %errorlevel%
