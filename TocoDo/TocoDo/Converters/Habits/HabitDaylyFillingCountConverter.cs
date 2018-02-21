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
				if (habit.HabitType == HabitType.Unit)
					return string.Format(Resources.ActiveHabitButtonTextUnit,   habit.RepeatsToday, habit.MaxRepeatsADay);
				return string.Format(Resources.ActiveHabitButtonTextDaylong, habit.RepeatsToday);
			}

			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}