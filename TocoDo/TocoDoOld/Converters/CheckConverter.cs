using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class CheckConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value == false ? null : (DateTime?)DateTime.Now;
		}
	}
}
