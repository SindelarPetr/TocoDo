using System;
using System.Globalization;
using TocoDo.Properties;

namespace TocoDo.Converters
{
	public class ReminderConverter : DateTimeConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Resources.Reminder + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}
