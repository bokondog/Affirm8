# Start Kind Words Full-Stack Solution - PowerShell Script
# This script starts both the Kind Words API and the MAUI app

Write-Host "🚀 Starting Kind Words Full-Stack Solution..." -ForegroundColor Green

# Start the Kind Words API in a new PowerShell window
Write-Host "📡 Starting Kind Words API..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'KindWordsApi\KindWordsApi'; dotnet run"

# Wait a moment for the API to start
Start-Sleep -Seconds 5

# Start the MAUI app
Write-Host "📱 Starting Kind Words MAUI App..." -ForegroundColor Cyan
cd "KindWordsApp\KindWords"
dotnet run --framework net8.0-windows10.0.19041.0

Write-Host "✅ Kind Words Full-Stack started successfully!" -ForegroundColor Green 