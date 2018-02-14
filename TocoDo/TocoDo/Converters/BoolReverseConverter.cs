using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace TocoDo.UI.Converters
{
	public class BoolReverseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Reverse(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Reverse(value);
		}

		private bool Reverse(object obj)
		{
			bool? bl = obj as bool?;

			if (bl == null)
			{
				Debug.WriteLine($"--------- Couldnt convert {obj.GetType().FullName} to bool.");
				throw new TypeAccessException($"Cannot convert type {obj.GetType().FullName} to bool.");
			}

			return !(bool)obj;
		}
	}
}
