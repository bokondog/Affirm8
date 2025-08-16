# Start KindWords with API - PowerShell Script
# This script starts both the API and the MAUI app for full functionality

Write-Host "🚀 Starting Kind Words with API..." -ForegroundColor Green

# Start the API in a new PowerShell window
Write-Host "📡 Starting API (ExampleWebApi)..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '..\api\ExampleWebApi'; dotnet run --project ExampleWebApi.Api.csproj"

# Wait a moment for the API to start
Start-Sleep -Seconds 3

# Start the MAUI app
Write-Host "📱 Starting Kind Words MAUI App..." -ForegroundColor Cyan
cd "KindWords"
dotnet run --framework net8.0-windows10.0.19041.0

Write-Host "✅ Kind Words started with full API functionality!" -ForegroundColor Green 