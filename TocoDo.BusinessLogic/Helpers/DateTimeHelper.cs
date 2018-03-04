using System;
using TocoDo.BusinessLogic.DependencyInjection.Models;

namespace TocoDo.BusinessLogic.Helpers
{
	public static class DateTimeHelper
	{
		public static int ZeroMondayBasedDay(this DateTime date)
		{
			return ((int) date.DayOfWeek + 6) % 7;
		}

		public static DateTime GetNextMondayDate(this DateTime date)
		{
			return date.AddDays(7 - date.ZeroMondayBasedDay());
		}
	}
}