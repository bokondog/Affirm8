
using System.Globalization;

namespace Affirm8.Converters
{
    /// <summary>
    /// This class converts the availability-status of chat message recipients from string to a simple icon string.
    /// </summary>
    public class StringToBadgeIconConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the string to badge icon.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the badge icon.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString() switch
                {
                    "Available" => "●", // Green dot
                    "Busy" => "●", // Red dot  
                    "Away" => "●", // Yellow dot
                    _ => "○" // Empty circle
                };
            }

            return "○";
        }

        /// <summary>
        /// This method is used to convert the badge icon to string.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the string.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}