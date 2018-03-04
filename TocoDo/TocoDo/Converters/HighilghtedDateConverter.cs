using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
	public class HighilghtedDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTime date)
			{
				string format = "MMMM";
				if (date.Year != DateTime.Today.Year)
					format += ", yyyy";

				return date.ToString(format);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
