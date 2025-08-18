@echo off
echo 🚀 Starting Kind Words Full-Stack Solution...
echo.

echo 📡 Starting Kind Words API...
start "Kind Words API" cmd /k "cd KindWordsApi\KindWordsApi && dotnet run"

echo ⏳ Waiting for API to start...
timeout /t 5 /nobreak > nul

echo 📱 Starting Kind Words MAUI App...
cd KindWordsApp\KindWords
dotnet run --framework net8.0-windows10.0.19041.0

echo.
echo ✅ Kind Words Full-Stack started successfully!
pause 