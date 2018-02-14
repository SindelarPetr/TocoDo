using System;
using System.Globalization;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Habits
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
