using System;
using System.Globalization;
using TocoDo.Resources;

namespace TocoDo.Converters
{
	public class DeadlineConverter : DateToTextConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return AppResource.Deadline + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}
