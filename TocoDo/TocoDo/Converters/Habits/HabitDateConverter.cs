using System;
using System.Globalization;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Habits
{
	public class HabitDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var habit = value as HabitViewModel;

			var date = habit?.StartDate;

			if (date == null)
				return string.Empty;

			var startDate     = DateToTextConverter.Convert(date);
			var repeatText    = GetRepeatText(habit.RepeatType, habit.DaysToRepeat);
			var dayAmountText = habit.HabitType == HabitType.Daylong
				? $"{Resources.AllDay.ToLower()}"
				: $"{habit.RepeatsToday} {Resources.TimesADay.ToLower()}";
			return $"From {startDate} - {repeatText} {dayAmountText}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public static string GetRepeatText(RepeatType habitModelRepeatType, int modelRepeatNumber)
		{
			if (habitModelRepeatType.HasFlag(RepeatType.Days))
				return string.Format(Resources.RepeatTypeDays, modelRepeatNumber);

			if (habitModelRepeatType.HasFlag(RepeatType.Months))
				return string.Format(Resources.RepeatTypeMonths, modelRepeatNumber);

			if (habitModelRepeatType.HasFlag(RepeatType.Years))
				return string.Format(Resources.RepeatTypeYears, modelRepeatNumber);

			return string.Format(Resources.RepeatTypeWeeks, GetWeekDays(habitModelRepeatType), modelRepeatNumber);
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