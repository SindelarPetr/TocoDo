using System;
using System.Globalization;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	// TODO: Get rid of this class by using trigger
	public class IsSettedValueToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return (Color)App.Current.Resources["UnsetItemColor"];
			}

			return (Color)App.Current.Resources["SetItemColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
