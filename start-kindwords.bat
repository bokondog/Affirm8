@echo off
echo 🚀 Starting Kind Words with API...
echo.

echo 📡 Starting API in new window...
start "API - ExampleWebApi" cmd /k "cd ..\api\ExampleWebApi && dotnet run --project ExampleWebApi.Api.csproj"

echo ⏳ Waiting for API to start...
timeout /t 5 /nobreak > nul

echo 📱 Starting Kind Words MAUI App...
cd KindWords
dotnet run --framework net8.0-windows10.0.19041.0

echo.
echo ✅ Kind Words started with full API functionality!
pause 