using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
	public class NullableDateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return DateTime.Today;
			return (DateTime) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}