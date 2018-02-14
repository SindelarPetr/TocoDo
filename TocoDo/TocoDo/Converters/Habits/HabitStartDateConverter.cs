using System;
using System.Globalization;
using TocoDo.BusinessLogic.Properties;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.Habits
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
