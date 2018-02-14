using System;
using System.Globalization;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.UI.Converters.Tasks
{
	public class ScheduleDateConverter : DateToTextConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Resources.ScheduleDate + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}
