using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Tasks
{
	public class ScheduleDateToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null
				? Application.Current.Resources["UnsetItemColor"]
				: Application.Current.Resources["ColorPrimary"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}