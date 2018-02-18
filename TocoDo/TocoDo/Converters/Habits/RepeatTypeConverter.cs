using System;
using System.Globalization;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Properties;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Habits
{
	public class RepeatTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is RepeatType type) return GetRepeatText(type);

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public static string GetRepeatText(RepeatType habitModelRepeatType)
		{
			if (habitModelRepeatType.HasFlag(RepeatType.Days))
				return Resources.EveryDay;

			if (habitModelRepeatType.HasFlag(RepeatType.Months))
				return Resources.EveryMonth;

			if (habitModelRepeatType.HasFlag(RepeatType.Years))
				return Resources.EveryYear;

			return $"{Resources.Every} {GetWeekDays(habitModelRepeatType)}";
		}

		public static string GetWeekDays(RepeatType type)
		{
			var days = string.Empty;
			if (type.HasFlag(RepeatType.Mon))
				days += (days == string.Empty ? "" : ", ") + Resources.Mon;

			if (type.HasFlag(RepeatType.Tue))
				days += (days == string.Empty ? "" : ", ") + Resources.Tue;

			if (type.HasFlag(RepeatType.Wed))
				days += (days == string.Empty ? "" : ", ") + Resources.Wed;

			if (type.HasFlag(RepeatType.Thu))
				days += (days == string.Empty ? "" : ", ") + Resources.Thu;

			if (type.HasFlag(RepeatType.Fri))
				days += (days == string.Empty ? "" : ", ") + Resources.Fri;

			if (type.HasFlag(RepeatType.Sat))
				days += (days == string.Empty ? "" : ", ") + Resources.Sat;

			if (type.HasFlag(RepeatType.Sun))
				days += (days == string.Empty ? "" : ", ") + Resources.Sun;

			return days;
		}
	}
}