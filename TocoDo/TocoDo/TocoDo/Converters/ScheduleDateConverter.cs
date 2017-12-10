using System;
using System.Globalization;
using TocoDo.Resources;

namespace TocoDo.Converters
{
	public class ScheduleDateConverter : DateToTextConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return AppResource.ScheduleDate + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}
