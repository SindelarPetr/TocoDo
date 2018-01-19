using System;
using System.Globalization;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class NoteTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (String.IsNullOrWhiteSpace((string)value))
				return Resources.AddANote;
			
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
