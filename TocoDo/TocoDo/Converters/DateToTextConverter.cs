using System;
using System.Globalization;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class DateToTextConverter : IValueConverter
	{
		public static string Convert(DateTime? date)
		{
			if (date == null)
				return Resources.IsNotSet;

			if (date.Value.Date == DateTime.Today)
				return Resources.Today;

			if (date.Value.Date == DateTime.Today + TimeSpan.FromDays(1))
				return Resources.Tomorrow;

			//if (date.Value.Date == DateTime.Today + TimeSpan.FromDays(2))
			//	return Resources.TheDayAfterTomorrow;

			string dateFormat = "ddd, d MMM";
			if (date.Value.Date.Year == DateTime.Today.Year)
				return date.Value.ToString(dateFormat);
			return date.Value.ToString(dateFormat + " yyyy");
		}

		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime? date = value as DateTime?;

			return Convert(date);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
