using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Helpers
{
	// TODO: Unit test this class
	public class HabitScheduleHelper
	{
		private readonly IDateTimeProvider _provider;

		public HabitScheduleHelper(IDateTimeProvider provider)
		{
			_provider = provider;
		}

		public TimeSpan HabitLength(DateTime start, RepeatType repeatType, int daysToRepeat)
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
		public bool IsHabitToday(HabitViewModel habit) => DateTimeHelper.IsHabitToday(habit.StartDate, habit.RepeatType, habit.DaysToRepeat);
		public bool IsHabitFinished(HabitViewModel habit) => DateTimeHelper.IsHabitFinished(habit.StartDate, habit.RepeatType, habit.DaysToRepeat);
		public bool IsHabitCurrent(HabitViewModel habit) => DateTimeHelper.IsHabitCurrent(habit.StartDate, habit.RepeatType, habit.DaysToRepeat);
	}
}