using System.Globalization;

namespace Affirm8.Converters
{
    public class BoolToSendButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSending)
            {
                return isSending ? "Sending... 💫" : "Send 💌";
            }
            return "Send 💌";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 