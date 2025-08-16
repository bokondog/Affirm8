Student: Glenn Bokondo

Email: glenn.bokondo@student.pxl.be

# KIND WORDS - Full-Stack Social Affirmation Platform

## 📖 Beschrijving

**Kind Words** is een volledige full-stack sociale affirmatie platform geïnspireerd op het indie spel waar gebruikers anoniem bemoedigende berichten kunnen versturen en ontvangen. Het project bestaat uit een **.NET MAUI frontend** en een **ASP.NET Core Web API backend**.

### 🎯 Concept

- **Verstuur** affirmaties, steunverzoeken of dankbaarheidsberichten anoniem
- **Ontvang** willekeurige berichten van anderen om vriendelijk op te reageren
- **Antwoord** met bemoedigende woorden om iemands reis te helpen
- **Verbind** door een gemeenschap van anonieme steun en positiviteit op te bouwen

## 🏗️ Architectuur

### Full-Stack Oplossing

```
📱 Kind Words MAUI App (.NET 8)
    ↕️ HTTPS API Calls
🔧 Kind Words API (.NET 9)
```

**Frontend**: .NET MAUI cross-platform app (Android, Windows)
**Backend**: ASP.NET Core Web API met JWT authenticatie
**Database**: In-memory (klaar voor Entity Framework uitbreiding)

## ✨ Features

### 📱 MAUI App Features

- **5 Schermen**: Send, Inbox, My Messages, Profile, Settings
- **MVVM Architectuur** met CommunityToolkit
- **Authenticatie UI** met login/register functionaliteit
- **Message Management** voor versturen en beantwoorden
- **Responsive Design** met Material Design principes

### 🔧 API Features

- **RESTful Endpoints** voor authenticatie (register/login)
- **JWT Authenticatie** met custom token generatie
- **In-memory Storage** (ConcurrentDictionary - development only)
- **CORS Configuratie** voor MAUI app integratie
- **Swagger UI** voor API documentatie en testing
- **Port Configuratie** (HTTPS: 7001)

### 🎯 Kernfunctionaliteit

- ✅ **Multi-project setup** met gesynchroniseerde startup
- ✅ **Complete MVVM implementatie** met data binding
- ✅ **Custom converters** en UI componenten
- ✅ **Inbox business logic** (willekeurige berichten, reply filtering)
- ✅ **My Messages** functionaliteit (eigen berichten + alle replies)
- ✅ **Authentication flow** (ready voor API integratie)

## 🚀 Hoe te Starten

### Option 1: Visual Studio (Aanbevolen)

1. Open `KindWords-FullStack.sln` in Visual Studio 2022
2. Druk op **F5** - beide projecten starten automatisch
3. API start eerst, MAUI app verbindt zich daarna

### Option 2: Command Line Scripts

```batch
# Windows Batch
.\start-kindwords-fullstack.bat

# PowerShell
.\start-kindwords-fullstack.ps1
```

### Option 3: Handmatig

```bash
# Terminal 1 - Start API
cd KindWordsApi/KindWordsApi
dotnet run

# Terminal 2 - Start MAUI App (wacht 5 seconden)
cd KindWordsApp/KindWords
dotnet run --framework net8.0-windows10.0.19041.0
```

## 📊 Project Status

### ✅ Voltooid

- **Multi-project configuratie** met Visual Studio F5 startup
- **Complete MAUI UI** voor alle schermen
- **MVVM architectuur** met proper separation
- **Authentication API** met register/login endpoints
- **JWT token system** werkend tussen API en MAUI
- **In-memory data storage** voor development

### 🔧 In Ontwikkeling

- **Message API endpoints** (send/inbox/my-messages)
- **Database integratie** met Entity Framework + SQL
- **Message business logic** op API niveau
- **Real-time updates** tussen API en MAUI

## 📋 Screenshots

_Screenshots van de app interfaces worden hier toegevoegd_

## 🎥 Videolink

_Link naar demonstratie video komt hier_

## 📚 Bronnen

### Technical Documentation

- [.NET MAUI Documentatie](https://docs.microsoft.com/en-us/dotnet/maui/)
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [CommunityToolkit.Mvvm](https://docs.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

### Inspiratie

- **Kind Words (Video Game)** - Popcannibal Games
- **Material Design Guidelines** voor UI/UX
- **Clean Architecture Principles** voor code structuur

### Development Tools

- **Visual Studio 2022** met .NET MAUI workload
- **Git** voor versie controle
- **Swagger UI** voor API testing

## 📁 Project Structuur

```
📁 KindWords-FullStack.sln
├── 📱 KindWordsApp/KindWords/     # MAUI Frontend
├── 🔧 KindWordsApi/KindWordsApi/  # ASP.NET Core API
├── 📋 GUIDE.md                   # Developer documentatie
├── 📋 MULTI-PROJECT-SETUP.md     # Setup instructies
└── 🚀 start-kindwords-fullstack.bat  # Launch script
```

---

**Ontwikkeld door Glenn Bokondo** - PXL Hogeschool - C# Mobile Development
