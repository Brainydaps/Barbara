using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Barbara.Converters
{
    public class AuthorToColorConverter : IValueConverter
    {
        // You can tweak these two colors as you like:
        private static readonly Color UserColor = Colors.LightSkyBlue;
        private static readonly Color BotColor = Colors.LightGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var author = value as string;
            if (string.IsNullOrEmpty(author))
                return BotColor;

            // Treat "You" as the user; everything else is a bot reply
            return author.Equals("You", StringComparison.OrdinalIgnoreCase)
                ? UserColor
                : BotColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("AuthorToColorConverter does not support ConvertBack.");
        }
    }
}
