using System;
using System.Globalization;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Habits
{
    public class RepeatTypeToBoolConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    var habitRepeat = (RepeatType) value;
		    var btnRepeat = (RepeatType) parameter;

			var val = habitRepeat.HasFlag(btnRepeat);
		    return val;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
    }
}
