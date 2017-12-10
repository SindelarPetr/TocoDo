using System;
using System.Globalization;
using TocoDo.Resources;
using TocoDo.ViewModels;
using Xamarin.Forms;

namespace TocoDo.Converters
{
	public class ModifyPageBottmPanelTextConverter : IValueConverter
	{
		/// <summary>
		/// Converts the given date into easily readable string format, so the user will easily know when the task was created
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var model = value as TaskViewModel;

			if (model == null) return null;

			DateTime dateTime;
			string str;
			if (model.Done != null)
			{
				dateTime = model.Done.Value;
				str = AppResource.Completed + " ";
			}
			else
			{
				dateTime = model.CreateTime;
				str = AppResource.Created + " ";
			}

			TimeSpan difference = DateTime.Now - dateTime;

			// if the task is less than 3 mins old, then show that it has been created now
			if (difference.TotalMinutes < 3)
			{
				str += AppResource.Now.ToLower();
				return str;
			}

			// Show that it was created today
			if (dateTime.Date == DateTime.Today)
			{
				str += AppResource.Today.ToLower();
				return str;
			}

			// Show that it was created yesterday
			if (dateTime.Date == DateTime.Today - TimeSpan.FromDays(1))
			{
				str += AppResource.Yeasterday.ToLower();
				return str;
			}

			if (dateTime.Date >= DateTime.Today - TimeSpan.FromDays(10))
			{
				str += 10 - (int)(dateTime.Date - (DateTime.Today - TimeSpan.FromDays(10))).TotalDays + " " + AppResource.DaysAgo;
				return str;
			}

			return str + dateTime.ToString("d MMMM yyyy");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
