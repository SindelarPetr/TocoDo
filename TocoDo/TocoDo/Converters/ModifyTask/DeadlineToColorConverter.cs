﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters.ModifyTask
{
	public class DeadlineToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null
				? Application.Current.Resources["UnsetItemColor"]
				: Application.Current.Resources["DeadlineColor"];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}