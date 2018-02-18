using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Tasks
{
	public class DescriptionToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.IsNullOrWhiteSpace((string) value)
				? Application.Current.Resources["UnsetItemColor"]
				: Application.Current.Resources["DescriptionColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}