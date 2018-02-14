using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
	public class IsNotNullOrWhitespaceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = (string)value;
			return !string.IsNullOrWhiteSpace(str);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
