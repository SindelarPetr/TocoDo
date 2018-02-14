using System;
using System.Globalization;

namespace TocoDo.UI.Converters
{
	public class DateTimeConverter : DateToTextConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string basicText = (string)base.Convert(value, targetType, parameter, culture);

			DateTime? time = value as DateTime?;
			if (time == null) return basicText;

			return basicText + " at " + time.Value.ToString("t");
		}
	}
}
