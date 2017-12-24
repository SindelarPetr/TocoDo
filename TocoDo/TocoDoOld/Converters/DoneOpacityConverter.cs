using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class DoneOpacityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null)
				return 0.4f;

			return 1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
