using System;
using System.Globalization;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.UI.Converters.ModifyTask
{
	public class DeadlineConverter : DateToTextConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Resources.Deadline + (value == null ? " " : ": ") + base.Convert(value, targetType, parameter, culture);
		}
	}
}