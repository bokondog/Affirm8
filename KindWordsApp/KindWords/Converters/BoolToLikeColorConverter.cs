using System.Globalization;

namespace KindWords.Converters
{
    /// <summary>
    /// Converter to show different colors for liked vs non-liked states
    /// </summary>
    public class BoolToLikeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isLiked)
            {
                return isLiked ? Color.FromArgb("#FF69B4") : Color.FromArgb("#999999"); // Pink vs Gray
            }
            return Color.FromArgb("#999999");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
