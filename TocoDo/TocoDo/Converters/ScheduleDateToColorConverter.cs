using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class ScheduleDateToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? App.Current.Resources["UnsetValueColor"] : App.Current.Resources["TasksPageColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
