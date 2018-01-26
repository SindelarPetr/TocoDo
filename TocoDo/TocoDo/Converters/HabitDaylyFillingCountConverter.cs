using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.Models;
using TocoDo.Properties;
using TocoDo.ViewModels;
using Xamarin.Forms;

namespace TocoDo.Converters
{
    public class HabitDaylyFillingCountConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    if (value is HabitViewModel habit)
		    {
			    if (habit.ModelHabitType == HabitType.Unit)
				    return string.Format(Resources.ActiveHabitButtonTextUnit, habit.ModelRepeatsToday, habit.ModelMaxRepeatsADay);
				return string.Format(Resources.ActiveHabitButtonTextDaylong, habit.ModelRepeatsToday);
		    }

		    return "";
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
