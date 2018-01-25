using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.Models;
using Xamarin.Forms;

namespace TocoDo.Converters.Habits
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
