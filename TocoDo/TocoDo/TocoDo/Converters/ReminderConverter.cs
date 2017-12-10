using System;
using System.Globalization;
using TocoDo.Resources;

namespace TocoDo.Converters
{
	public class ReminderConverter : DateTimeConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return AppResource.Reminder + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}
