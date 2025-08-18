using System.Globalization;

namespace Affirm8.Converters
{
    /// <summary>
    /// Converter to show filled or empty heart based on like status
    /// </summary>
    public class BoolToHeartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isLiked)
            {
                return isLiked ? "❤️" : "🤍"; // Filled heart vs empty heart
            }
            return "🤍";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
