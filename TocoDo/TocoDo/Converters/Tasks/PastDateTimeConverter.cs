using System;
using System.Globalization;
using TocoDo.BusinessLogic.Properties;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Tasks
{
	public class PastDateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is DateTime dateTime)
			{
				var difference = DateTime.Today - dateTime;

				// if the task is less than 3 mins old, then show that it has been created now
				if ((DateTime.Now - dateTime).Minutes <= 3) return Resources.Now;

				// Show that it was created today
				if (dateTime.Date == DateTime.Today) return Resources.Today;

				// Show that it was created yesterday
				if (dateTime.Date == DateTime.Today - TimeSpan.FromDays(1)) return Resources.Yeasterday;

				// Show how many days
				if (dateTime.Date >= DateTime.Today - TimeSpan.FromDays(6))
					return string.Format(Resources.Day_sAgo, difference.Days);

				// Show how many weeks
				if (dateTime.Date >= DateTime.Today - TimeSpan.FromDays(31))
					return string.Format(Resources.Week_sAgo, difference.Days / 7);

				// Show how many months
				if (dateTime.Date >= DateTime.Today - TimeSpan.FromDays(365))
					return string.Format(Resources.Month_sAgo, difference.Days / 30);

				// Show how many years
				return string.Format(Resources.Year_sAgo, difference.Days / 365);
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}