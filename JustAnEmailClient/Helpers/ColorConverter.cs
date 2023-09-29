using Microsoft.Maui.Graphics.Converters;
using System.Globalization;

namespace JustAnEmailClient.Helpers;
// https://stackoverflow.com/questions/72902119/net-maui-is-it-possible-to-convert-a-string-to-a-color-inside-a-binding/72909265#72909265
public class ColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ColorTypeConverter converter = new ColorTypeConverter();
        var color = (Color)(converter.ConvertFromInvariantString((string)value));
        return color ?? Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
