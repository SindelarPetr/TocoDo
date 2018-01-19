using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.Properties;
using Xamarin.Forms;

namespace TocoDo.Converters.Habits
{
    public class HabitStartDateConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			string text = Resources.StartDate;
		    DateTime? date = value as DateTime?;
		    if (date == null)
			    return $"{text} {Resources.IsNotSet}";

		    return $"{text}: {DateToTextConverter.Convert(date)}";
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
