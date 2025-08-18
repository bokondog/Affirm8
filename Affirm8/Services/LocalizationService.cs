using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Affirm8.Services
{
    public class LocalizationService : INotifyPropertyChanged
    {
        private const string LanguageKey = "AppLanguage";
        
        public event Action<CultureInfo>? LanguageChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private CultureInfo _currentCulture;
        
        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            private set
            {
                _currentCulture = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEnglish));
                OnPropertyChanged(nameof(IsDutch));
                OnPropertyChanged(nameof(LanguageDisplayName));
            }
        }
        
        public bool IsEnglish => CurrentCulture.Name == "en-US";
        public bool IsDutch => CurrentCulture.Name == "nl-NL";
        public string LanguageDisplayName => CurrentCulture.Name == "nl-NL" ? "Nederlands" : "English";
        
        public LocalizationService()
        {
            LoadLanguage();
        }
        
        public void SetLanguage(string languageCode)
        {
            var culture = new CultureInfo(languageCode);
            CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            
            Preferences.Set(LanguageKey, languageCode);
            
            // Notify all computed properties have changed
            OnPropertyChanged(nameof(SettingsTitle));
            OnPropertyChanged(nameof(InboxTitle));
            OnPropertyChanged(nameof(MyMessagesTitle));
            OnPropertyChanged(nameof(ProfileTitle));
            OnPropertyChanged(nameof(SendMessageTitle));
            OnPropertyChanged(nameof(AppearanceTitle));
            OnPropertyChanged(nameof(LanguageTitle));
            
            LanguageChanged?.Invoke(culture);
        }
        
        public void ToggleLanguage()
        {
            var newLanguage = IsEnglish ? "nl-NL" : "en-US";
            SetLanguage(newLanguage);
        }
        
        private void LoadLanguage()
        {
            var savedLanguage = Preferences.Get(LanguageKey, "en-US");
            CurrentCulture = new CultureInfo(savedLanguage);
            CultureInfo.DefaultThreadCurrentCulture = CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CurrentCulture;
        }
        
        // Localized strings
        public string GetString(string key)
        {
            return key switch
            {
                // Navigation
                "Settings" => IsEnglish ? "Settings" : "Instellingen",
                "Inbox" => IsEnglish ? "Inbox" : "Postvak",
                "MyMessages" => IsEnglish ? "My Messages" : "Mijn Berichten",
                "Profile" => IsEnglish ? "Profile" : "Profiel",
                "SendMessage" => IsEnglish ? "Send Message" : "Bericht Versturen",
                
                // Appearance
                "Appearance" => IsEnglish ? "Appearance" : "Weergave",
                "Language" => IsEnglish ? "Language" : "Taal",
                "EnableDarkMode" => IsEnglish ? "Enable Dark Mode" : "Donkere modus inschakelen",
                "EnableLightMode" => IsEnglish ? "Enable Light Mode" : "Lichte modus inschakelen",
                "SwitchToEnglish" => IsEnglish ? "Switch to Dutch" : "Schakel naar Engels",
                "SwitchToDutch" => IsEnglish ? "Switch to English" : "Schakel naar Nederlands",
                
                // Inbox
                "SpreadKindness" => IsEnglish ? "💌 Spread Kindness" : "💌 Verspreidt Vriendelijkheid",
                "SearchMessages" => IsEnglish ? "Search messages..." : "Berichten zoeken...",
                "AllCategories" => IsEnglish ? "All Categories" : "Alle Categorieën",
                "SendKindnessBack" => IsEnglish ? "💝 Send some kindness back" : "💝 Stuur wat vriendelijkheid terug",
                "TypeKindWords" => IsEnglish ? "Type your kind words here..." : "Typ hier je lieve woorden...",
                "SendReply" => IsEnglish ? "Send Reply" : "Verstuur Antwoord",
                
                // My Messages
                "WelcomeBack" => IsEnglish ? "Welcome back" : "Welkom terug",
                "NoMessagesYet" => IsEnglish ? "No messages yet" : "Nog geen berichten",
                "ShareFirstMessage" => IsEnglish ? "Share your first message to start spreading kindness!" : "Deel je eerste bericht om vriendelijkheid te verspreiden!",
                "GoToSend" => IsEnglish ? "Go to Send" : "Ga naar Versturen",
                
                // Profile
                "UserStatistics" => IsEnglish ? "Your Statistics" : "Jouw Statistieken",
                "MessagesSent" => IsEnglish ? "Messages Sent" : "Berichten Verzonden",
                "RepliesReceived" => IsEnglish ? "Replies Received" : "Antwoorden Ontvangen",
                "RepliesSent" => IsEnglish ? "Replies Sent" : "Antwoorden Verzonden",
                "LikesReceived" => IsEnglish ? "Likes Received" : "Likes Ontvangen",
                "ImpactScore" => IsEnglish ? "Impact Score" : "Impact Score",
                "DaysActive" => IsEnglish ? "Days Active" : "Dagen Actief",
                "MemberSince" => IsEnglish ? "Member since" : "Lid sinds",
                
                // Authentication
                "Login" => IsEnglish ? "Login" : "Inloggen",
                "Register" => IsEnglish ? "Register" : "Registreren",
                "Email" => IsEnglish ? "Email" : "E-mail",
                "Password" => IsEnglish ? "Password" : "Wachtwoord",
                "NickName" => IsEnglish ? "Nickname" : "Bijnaam",
                "Logout" => IsEnglish ? "Logout" : "Uitloggen",
                
                // Send Message
                "ShareMessage" => IsEnglish ? "💝 Share Your Message" : "💝 Deel Je Bericht",
                "WriteMessage" => IsEnglish ? "Write your message here..." : "Schrijf je bericht hier...",
                "SelectCategory" => IsEnglish ? "Select Category" : "Selecteer Categorie",
                "SendAnonymously" => IsEnglish ? "Send anonymously" : "Anoniem versturen",
                "Send" => IsEnglish ? "Send" : "Versturen",
                
                // Categories
                "Support" => IsEnglish ? "Support" : "Ondersteuning",
                "Hope" => IsEnglish ? "Hope" : "Hoop",
                "Celebration" => IsEnglish ? "Celebration" : "Viering",
                "Gratitude" => IsEnglish ? "Gratitude" : "Dankbaarheid",
                
                _ => key // Return the key if no translation found
            };
        }
        
        // Computed properties for common UI texts
        public string SettingsTitle => GetString("Settings");
        public string InboxTitle => GetString("Inbox");
        public string MyMessagesTitle => GetString("MyMessages");
        public string ProfileTitle => GetString("Profile");
        public string SendMessageTitle => GetString("SendMessage");
        public string AppearanceTitle => GetString("Appearance");
        public string LanguageTitle => GetString("Language");
        
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

