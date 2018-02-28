using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TocoDo.BusinessLogic;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
    public class GridCellDateConverter : IValueConverter
    {
	    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	    {
			MyLogger.WriteStartMethod();
		    if (value is DateTime date)
			{
				MyLogger.WriteEndMethod();
				return date.Day;
		    }

			MyLogger.WriteEndMethod();
		    return null;
	    }

	    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	    {
		    throw new NotImplementedException();
	    }
    }
}
