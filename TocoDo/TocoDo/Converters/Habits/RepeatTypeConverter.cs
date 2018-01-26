﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.Models;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo.Converters.Habits
{
    public class RepeatTypeConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    if (value is RepeatType type)
		    {
			    return GetRepeatText(type);
		    }

		    return string.Empty;
	    }

	    public static string GetRepeatText(RepeatType habitModelRepeatType)
	    {
		    if (habitModelRepeatType.HasFlag(RepeatType.Days))
			    return Resources.EveryDay;

		    if (habitModelRepeatType.HasFlag(RepeatType.Months))
			    return Resources.EveryMonth;

		    if (habitModelRepeatType.HasFlag(RepeatType.Years))
			    return Resources.EveryYear;

		    return $"{ Resources.Every } { GetWeekDays(habitModelRepeatType) }";
	    }

	    public static string GetWeekDays(RepeatType type)
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