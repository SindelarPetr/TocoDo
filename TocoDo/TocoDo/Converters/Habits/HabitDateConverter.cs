using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.Models;
using TocoDo.Properties;
using TocoDo.ViewModels;
using Xamarin.Forms;

namespace TocoDo.Converters.Habits
{
    public class HabitDateConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			HabitViewModel habit = value as HabitViewModel;

		    DateTime? date = habit?.ModelStartDate;

			if (date == null)
			    return string.Empty;

		    string startDate = DateToTextConverter.Convert(date);
			string repeatText = GetRepeatText(habit.ModelRepeatType, habit.ModelDaysToRepeat);
		    string dayAmountText = habit.ModelHabitType == HabitType.Daylong
			    ? $"{Resources.AllDay.ToLower()}"
			    : $"{habit.ModelRepeatsADay} {Resources.TimesADay.ToLower()}";
		    return $"From { startDate } - { repeatText } {dayAmountText }";
		}

	    public static  string GetRepeatText(RepeatType habitModelRepeatType, int modelRepeatNumber)
	    {
		    if (habitModelRepeatType.HasFlag(RepeatType.Days))
			    return string.Format(Resources.RepeatTypeDays, modelRepeatNumber);

		    if (habitModelRepeatType.HasFlag(RepeatType.Months))
			    return string.Format(Resources.RepeatTypeMonths, modelRepeatNumber);

		    if (habitModelRepeatType.HasFlag(RepeatType.Years))
			    return string.Format(Resources.RepeatTypeYears, modelRepeatNumber);

		    return string.Format(Resources.RepeatTypeWeeks, GetWeekDays(habitModelRepeatType), modelRepeatNumber);
	    }

	    public  static string GetWeekDays(RepeatType type)
	    {
		    string days = string.Empty;
		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Mon;

		    if (type.HasFlag(RepeatType.Tue))
			    days += (days == String.Empty ? "" : ", ") + Resources.Tue;

		    if (type.HasFlag(RepeatType.Wed))
			    days += (days == String.Empty ? "" : ", ") + Resources.Wed;

		    if (type.HasFlag(RepeatType.Thu))
			    days += (days == String.Empty ? "" : ", ") + Resources.Thu;

		    if (type.HasFlag(RepeatType.Fri))
			    days += (days == String.Empty ? "" : ", ") + Resources.Fri;

		    if (type.HasFlag(RepeatType.Sat))
			    days += (days == String.Empty ? "" : ", ") + Resources.Sat;

		    if (type.HasFlag(RepeatType.Sun))
			    days += (days == String.Empty ? "" : ", ") + Resources.Sun;

		    return days;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
