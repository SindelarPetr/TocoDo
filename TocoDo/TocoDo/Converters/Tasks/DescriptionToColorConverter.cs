using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class DescriptionToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.IsNullOrWhiteSpace((string)value) ? App.Current.Resources["UnsetItemColor"] : App.Current.Resources["DescriptionColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
