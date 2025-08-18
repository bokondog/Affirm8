using Microsoft.Maui.Controls;

namespace Affirm8.Services
{
    public class ThemeService
    {
        private const string ThemeKey = "AppTheme";
        
        public event Action<AppTheme>? ThemeChanged;
        
        public AppTheme CurrentTheme { get; private set; }
        
        public ThemeService()
        {
            LoadTheme();
        }
        
        public void SetTheme(AppTheme theme)
        {
            CurrentTheme = theme;
            Application.Current!.UserAppTheme = theme;
            Preferences.Set(ThemeKey, theme.ToString());
            ThemeChanged?.Invoke(theme);
        }
        
        public void ToggleTheme()
        {
            var newTheme = CurrentTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
            SetTheme(newTheme);
        }
        
        private void LoadTheme()
        {
            var savedTheme = Preferences.Get(ThemeKey, AppTheme.Unspecified.ToString());
            if (Enum.TryParse<AppTheme>(savedTheme, out var theme))
            {
                CurrentTheme = theme;
                Application.Current!.UserAppTheme = theme;
            }
            else
            {
                CurrentTheme = AppTheme.Light;
                Application.Current!.UserAppTheme = AppTheme.Light;
            }
        }
        
        public bool IsDarkMode => CurrentTheme == AppTheme.Dark;
    }
}

