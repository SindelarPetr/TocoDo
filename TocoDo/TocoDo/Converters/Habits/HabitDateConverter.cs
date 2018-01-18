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
		    string repeatText = GetRepeatText(habit.ModelRepeatType, habit.ModelRepeatNumber);
		    return $"From { startDate } - { repeatText }";
		}

	    private string GetRepeatText(RepeatType habitModelRepeatType, short modelRepeatNumber)
	    {
		    if (habitModelRepeatType.HasFlag(RepeatType.Days))
			    return $"{Resources.EveryDayFor} {modelRepeatNumber} {Resources.Days}";

		    if (habitModelRepeatType.HasFlag(RepeatType.Months))
			    return $"{Resources.For} {modelRepeatNumber} {Resources.Months}";

		    if (habitModelRepeatType.HasFlag(RepeatType.Years))
			    return $"{Resources.For} {modelRepeatNumber} {Resources.NextYears}";

		    return
			    $"{Resources.Every} {GetWeekDays(habitModelRepeatType)} {Resources.For.ToLower()} {modelRepeatNumber} {Resources.Days.ToLower()}";
	    }

	    private string GetWeekDays(RepeatType type)
	    {
		    string days = string.Empty;
		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Mon;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Tue;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Wed;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Thu;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Fri;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Sat;

		    if (type.HasFlag(RepeatType.Mon))
			    days += (days == String.Empty ? "" : ", ") + Resources.Sun;

		    return days;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
