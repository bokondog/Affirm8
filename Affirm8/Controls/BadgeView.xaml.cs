using Microsoft.Maui.Controls;

namespace Affirm8.Controls;

public partial class BadgeView : Grid
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content), typeof(View), typeof(BadgeView), null);

    public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(
        nameof(BadgeText), typeof(string), typeof(BadgeView), string.Empty);

    public static readonly BindableProperty ShowBadgeProperty = BindableProperty.Create(
        nameof(ShowBadge), typeof(bool), typeof(BadgeView), false);

    public static readonly BindableProperty BadgeColorProperty = BindableProperty.Create(
        nameof(BadgeColor), typeof(Color), typeof(BadgeView), Colors.Red);

    public static readonly BindableProperty BadgeTextColorProperty = BindableProperty.Create(
        nameof(BadgeTextColor), typeof(Color), typeof(BadgeView), Colors.White);

    public View Content
    {
        get => (View)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string BadgeText
    {
        get => (string)GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }

    public bool ShowBadge
    {
        get => (bool)GetValue(ShowBadgeProperty);
        set => SetValue(ShowBadgeProperty, value);
    }

    public Color BadgeColor
    {
        get => (Color)GetValue(BadgeColorProperty);
        set => SetValue(BadgeColorProperty, value);
    }

    public Color BadgeTextColor
    {
        get => (Color)GetValue(BadgeTextColorProperty);
        set => SetValue(BadgeTextColorProperty, value);
    }

    public BadgeView()
    {
        InitializeComponent();
        BindingContext = this;
    }
} 