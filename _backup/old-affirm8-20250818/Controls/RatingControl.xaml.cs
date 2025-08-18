using Microsoft.Maui.Controls;

namespace Affirm8.Controls;

public partial class RatingControl : ContentView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(double), typeof(RatingControl), 0.0, propertyChanged: OnValueChanged);

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
        nameof(Maximum), typeof(int), typeof(RatingControl), 5);

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(
        nameof(IsReadOnly), typeof(bool), typeof(RatingControl), false);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int Maximum
    {
        get => (int)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    // Star color properties for binding
    public Color Star1Color => Value >= 1 ? Colors.Gold : Colors.Gray;
    public Color Star2Color => Value >= 2 ? Colors.Gold : Colors.Gray;
    public Color Star3Color => Value >= 3 ? Colors.Gold : Colors.Gray;
    public Color Star4Color => Value >= 4 ? Colors.Gold : Colors.Gray;
    public Color Star5Color => Value >= 5 ? Colors.Gold : Colors.Gray;

    public RatingControl()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is RatingControl control)
        {
            control.OnPropertyChanged(nameof(Star1Color));
            control.OnPropertyChanged(nameof(Star2Color));
            control.OnPropertyChanged(nameof(Star3Color));
            control.OnPropertyChanged(nameof(Star4Color));
            control.OnPropertyChanged(nameof(Star5Color));
        }
    }

    private void OnStarClicked(object sender, EventArgs e)
    {
        if (IsReadOnly) return;

        if (sender is Button button && button.CommandParameter is int rating)
        {
            Value = rating;
        }
    }

    public event EventHandler<double>? ValueChanged;

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        
        if (propertyName == nameof(Value))
        {
            ValueChanged?.Invoke(this, Value);
        }
    }
} 