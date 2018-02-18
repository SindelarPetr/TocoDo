using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
	// TODO: Get rid of this class by using trigger
	public class IsSettedValueToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return (Color) Application.Current.Resources["UnsetItemColor"];

			return (Color) Application.Current.Resources["SetItemColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}