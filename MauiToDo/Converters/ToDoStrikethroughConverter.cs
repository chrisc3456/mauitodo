using MauiToDo.Data;
using System.Globalization;

namespace MauiToDo.Converters
{
    public class ToDoStrikethroughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return TextDecorations.Strikethrough;
            else
                return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Label)value).TextDecorations == TextDecorations.Strikethrough;
        }
    }
}
