# 🚀 Kind Words - Multi-Project Setup Guide

## ✅ **Setup Complete!**

This guide documents the **properly configured multi-project solution** for Kind Words with **synchronized startup** and **correct port configuration**.

## 📊 **Project Architecture**

```
📁 Kind Words Full-Stack Solution
├── 📄 KindWords-FullStack.sln      # Master solution file
├── 📁 KindWordsApi/                # Backend API (.NET 9)
│   └── 📁 KindWordsApi/            # Web API project
│       ├── 📄 Program.cs           # API configuration
│       ├── 📁 Properties/
│       │   └── 📄 launchSettings.json  # Port: 7001 (HTTPS), 5001 (HTTP)
│       └── 📁 Controllers/         # API endpoints (ready)
├── 📁 KindWordsApp/                # Frontend MAUI App (.NET 8)
│   └── 📁 KindWords/               # MAUI project
│       ├── 📁 Services/
│       │   └── 📄 AuthenticationService.cs  # Points to localhost:7001
│       └── 📁 Views/               # UI screens
└── 📁 .vs/                        # Visual Studio configuration
    └── 📁 KindWords-FullStack/
        └── 📁 v17/
            └── 📄 ProjectSettings.json  # Multiple startup projects
```

## 🔧 **Port Configuration**

| **Component**   | **Port** | **URL**                  | **Purpose**          |
| --------------- | -------- | ------------------------ | -------------------- |
| **API (HTTPS)** | 7001     | `https://localhost:7001` | Secure API calls     |
| **API (HTTP)**  | 5001     | `http://localhost:5001`  | Development fallback |
| **MAUI App**    | Dynamic  | Auto-assigned            | Client application   |

## 🎯 **Launch Options**

### **Option 1: Visual Studio (Recommended)**

1. Open `KindWords-FullStack.sln` in Visual Studio 2022
2. **Multiple startup projects** are pre-configured
3. Press **F5** - both projects start automatically in correct order:
   - ✅ API starts first (5 second delay)
   - ✅ MAUI app starts second, connects to API

### **Option 2: Command Line Scripts**

**Windows Batch File:**

```batch
.\start-kindwords-fullstack.bat
```

**PowerShell Script:**

```powershell
.\start-kindwords-fullstack.ps1
```

**Manual Commands:**

```bash
# Terminal 1 - Start API
cd KindWordsApi/KindWordsApi
dotnet run

# Terminal 2 - Start MAUI App (wait 5 seconds)
cd KindWordsApp/KindWords
dotnet run --framework net8.0-windows10.0.19041.0
```

## ✅ **Configuration Verification**

### **API Configuration**

- ✅ **Port**: 7001 (HTTPS) / 5001 (HTTP)
- ✅ **Weather endpoint**: `GET /weatherforecast` (test endpoint)
- ✅ **CORS**: Configured for MAUI app
- ✅ **Build**: Successful (.NET 9)

### **MAUI App Configuration**

- ✅ **API URL**: `https://localhost:7001/api`
- ✅ **Authentication Service**: Points to correct API
- ✅ **UI**: Complete with 5 tabs (Send, Inbox, My Messages, Profile, Settings)
- ✅ **Build**: Successful (.NET 8)

### **Visual Studio Configuration**

- ✅ **Solution**: Both projects included
- ✅ **Startup**: Multiple projects configured
- ✅ **Deploy**: Enabled for MAUI project

## 🔄 **Testing the Setup**

### **1. Test API Standalone**

```bash
cd KindWordsApi/KindWordsApi
dotnet run
```

**Expected**: API starts on `https://localhost:7001`  
**Test URL**: `https://localhost:7001/weatherforecast`

### **2. Test Full-Stack Connection**

1. Start API first
2. Start MAUI app second
3. **Expected**: MAUI app successfully connects to API (no connection errors)

### **3. Verify in MAUI App**

- ✅ **Settings Tab**: Login form should be ready for connection
- ✅ **No Connection Errors**: Should not see "connection refused" messages

## 🚧 **Next Development Steps**

With the **multi-project setup complete**, the next phase is:

### **Phase 1: Basic API Endpoints**

- [ ] Add authentication endpoints (`/api/auth/login`, `/api/auth/register`)
- [ ] Test login from MAUI Settings tab
- [ ] Verify JWT token flow

### **Phase 2: Message System**

- [ ] Add message endpoints (`/api/messages/inbox`, `/api/messages/send`)
- [ ] Implement inbox business logic (random 5 messages)
- [ ] Test message sending from MAUI Send tab

### **Phase 3: Complete Integration**

- [ ] Connect My Messages tab to API
- [ ] Implement reply functionality
- [ ] Add user statistics to Profile tab

## 📈 **Benefits Achieved**

- 🎯 **Correct Port Configuration**: No more connection refused errors
- 🚀 **Synchronized Startup**: API starts before MAUI app
- 🔧 **Visual Studio Integration**: F5 starts both projects
- 📋 **Clean Foundation**: Ready for rapid API endpoint development
- 🔄 **Easy Testing**: Multiple launch methods available

## 🐛 **Troubleshooting**

### **Port Already in Use**

```bash
netstat -an | findstr 7001
```

**Solution**: Kill process or change port in `launchSettings.json`

### **MAUI App Can't Connect**

- ✅ Verify API is running on port 7001
- ✅ Check Windows Firewall settings
- ✅ Confirm SSL certificate is trusted

### **Visual Studio Multiple Startup Not Working**

- ✅ Check `ProjectSettings.json` exists
- ✅ Verify project names match exactly
- ✅ Re-open solution file

---

## 🎉 **Success Metrics**

- ✅ **0 Build Errors** for both projects
- ✅ **API Starts** on correct port (7001)
- ✅ **MAUI App Connects** without connection errors
- ✅ **Both Projects** start from Visual Studio F5
- ✅ **Clean Architecture** ready for endpoint development

**Next Step**: Begin implementing authentication endpoints! 🚀
