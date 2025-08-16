# 🚀 Kind Words - Startup Guide

This guide explains how to run the **Kind Words** app with full API functionality.

## 📋 **Prerequisites**

- **.NET 8.0 SDK** installed
- **Visual Studio 2022** with .NET MAUI workload (recommended)
- **Windows 10/11** for running the MAUI app

## 🎯 **Quick Start Options**

### **Option 1: Batch File (Easiest)**

Simply double-click `start-kindwords.bat` in the `KindWordsApp` folder. This will:

1. Start the API in a new command window
2. Wait 5 seconds for API startup
3. Start the Kind Words MAUI app
4. Both will run simultaneously with full functionality

### **Option 2: PowerShell Script**

Run `start-kindwords.ps1` in PowerShell:

```powershell
.\start-kindwords.ps1
```

### **Option 3: Visual Studio Solution**

1. Open `KindWordsApp.sln` in Visual Studio 2022
2. The solution includes both projects:
   - **KindWords** (MAUI App)
   - **ExampleWebApi.Api** (Backend API)
3. Set **Multiple Startup Projects**:
   - Right-click solution → Properties
   - Select "Multiple startup projects"
   - Set both projects to "Start"
4. Press **F5** or click "Start"

### **Option 4: Manual Terminal**

**Terminal 1 - Start API:**

```bash
cd api/ExampleWebApi
dotnet run --project ExampleWebApi.Api.csproj
```

**Terminal 2 - Start MAUI App:**

```bash
cd KindWordsApp/KindWords
dotnet run --framework net8.0-windows10.0.19041.0
```

## 🔧 **Configuration**

### **API Endpoints**

- **Base URL**: `https://localhost:7027/api`
- **Swagger UI**: `https://localhost:7027/swagger`
- **Authentication**:
  - Register: `POST /api/Authentication/register`
  - Login: `POST /api/Authentication/login`

### **MAUI App**

- **Framework**: .NET 8.0 Windows
- **Authentication**: Configured to use local API
- **Real-time**: Messages and replies sync with API

## ✅ **Testing Full Functionality**

1. **Start both projects** using any method above
2. **Register a new account** in the Settings tab of the MAUI app
3. **Login** with your credentials
4. **Send messages** in the Send tab
5. **Reply to messages** in the Inbox tab
6. **View your messages** in the My Messages tab

## 🐛 **Troubleshooting**

### **API Won't Start**

- Check port 7027 isn't in use: `netstat -an | findstr 7027`
- Verify .NET 8.0 SDK is installed: `dotnet --version`

### **MAUI App Can't Connect**

- Ensure API is running on `https://localhost:7027`
- Check Windows Firewall isn't blocking the connection
- Verify SSL certificate is trusted

### **Build Errors**

- Clean and rebuild: `dotnet clean && dotnet build`
- Restore packages: `dotnet restore`
- Update workloads: `dotnet workload update`

## 📊 **Architecture**

```
┌─────────────────┐    HTTPS     ┌─────────────────┐
│   Kind Words    │◄──────────────│  ExampleWebApi  │
│   MAUI App      │               │     (API)       │
│   (Frontend)    │               │   (Backend)     │
└─────────────────┘               └─────────────────┘
      │                                   │
      │                                   │
   localhost:0                     localhost:7027
   (Random Port)                      (Fixed Port)
```

## 🎉 **Success!**

When both projects are running, you'll have:

- ✅ **Full authentication** with JWT tokens
- ✅ **Real message storage** and persistence
- ✅ **Proper user isolation** and security
- ✅ **Complete CRUD operations** for messages/replies

---

_Happy coding! 💫_
