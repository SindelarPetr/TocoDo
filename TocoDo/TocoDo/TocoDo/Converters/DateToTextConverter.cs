using System;
using System.Globalization;
using TocoDo.Resources;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class DateToTextConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			DateTime? date = value as DateTime?;

			if (date == null)
				return AppResource.IsNotSet;

			if (date.Value.Date == DateTime.Today)
				return AppResource.Today;

			if (date.Value.Date == DateTime.Today + TimeSpan.FromDays(1))
				return AppResource.Tomorrow;

			if (date.Value.Date == DateTime.Today + TimeSpan.FromDays(2))
				return AppResource.TheDayAfterTomorrow;

			string dateFormat = "ddd, d MMM";
			if (date.Value.Date.Year == DateTime.Today.Year)
				return date.Value.ToString(dateFormat);
			return date.Value.ToString(dateFormat + " yyyy");

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
