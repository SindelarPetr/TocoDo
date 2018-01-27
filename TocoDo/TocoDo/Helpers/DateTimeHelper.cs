using System;
using TocoDo.Models;
using TocoDo.ViewModels;

namespace TocoDo.Helpers
{
	public static class DateTimeHelper
	{
		public static string GetDayExtension(this DateTime date)
		{
			var ext = date.Day % 10 == 1 && date.Day != 11
				? "st"
				: date.Day % 10 == 2 && date.Day != 12
					? "nd"
					: date.Day % 10 == 3 && date.Day != 13
						? "rd"
						: "th";

			return ext;
		}
		
		public static TimeSpan HabitLength(DateTime start, RepeatType repeatType, int daysToRepeat)
		{
			switch (repeatType)
			{
				case RepeatType.Years:
					return start.AddYears(daysToRepeat) - start;
				case RepeatType.Months:
					return start.AddMonths(daysToRepeat) - start;
				case RepeatType.Days:
					return start.AddDays(daysToRepeat) - start;
			}

			int fromWeekMissing = 0;
			switch (repeatType)
			{
				case RepeatType.Sat:
					fromWeekMissing = 1;
					break;
				case RepeatType.Fri:
					fromWeekMissing = 2;
					break;
				case RepeatType.Thu:
					fromWeekMissing = 3;
					break;
				case RepeatType.Wed:
					fromWeekMissing = 4;
					break;
				case RepeatType.Tue:
					fromWeekMissing = 5;
					break;
				case RepeatType.Mon:
					fromWeekMissing = 6;
					break;
			}

			return start.AddDays(daysToRepeat * 7 - fromWeekMissing) - start;
		}

		public static bool IsHabitToday(DateTime? startDate, RepeatType repeat, int daysToRepeat)
		{
			if (startDate == null || !IsHabitCurrent(startDate, repeat, daysToRepeat)) return false;

			var start = startDate.Value;
			switch (repeat)
			{
				case RepeatType.Days:
					return true;
				case RepeatType.Months:
					return start.Day == DateTime.Today.Day;
				case RepeatType.Years:
					return start.Day == DateTime.Today.Day && start.Month == DateTime.Today.Month;
			}

			return repeat.HasFlag(RepeatType.Mon) && (int)DateTime.Today.DayOfWeek == 1 ||
				   repeat.HasFlag(RepeatType.Tue) && (int)DateTime.Today.DayOfWeek == 2 ||
				   repeat.HasFlag(RepeatType.Wed) && (int)DateTime.Today.DayOfWeek == 3 ||
				   repeat.HasFlag(RepeatType.Thu) && (int)DateTime.Today.DayOfWeek == 4 ||
				   repeat.HasFlag(RepeatType.Fri) && (int)DateTime.Today.DayOfWeek == 5 ||
				   repeat.HasFlag(RepeatType.Sat) && (int)DateTime.Today.DayOfWeek == 6 ||
				   repeat.HasFlag(RepeatType.Sun) && (int)DateTime.Today.DayOfWeek == 0;
		}


		public static bool IsHabitFinished(DateTime? start, RepeatType repeatType, int daysToRepeat)
		{
			return start != null && start + HabitLength(start.Value, repeatType, daysToRepeat) < DateTime.Now;
		}

		public static bool IsHabitCurrent(DateTime? startDate, RepeatType repeat, int daysToRepeat)
		{
			return startDate != null && !IsHabitFinished(startDate, repeat, daysToRepeat) && startDate.Value <= DateTime.Today;
		}
	}
}
