using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ItemFilters;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.Helpers
{
	public class HabitScheduleHelper
	{
		private readonly IDateTimeProvider _provider;

		public HabitScheduleHelper(IDateTimeProvider provider)
		{
			_provider = provider;
		}

		public bool IsHabitActive(IHabitViewModel habit, DateTime? date = null) =>
			IsHabitActive(date ?? _provider.Now, habit.StartDate, habit.RepeatType, habit.DaysToRepeat);
		private bool IsHabitActive(DateTime date, DateTime? nullableStartDate, RepeatType repeatType, int daysToRepeat)
		{
			if (nullableStartDate == null) return false;

			var startDate = nullableStartDate.Value;
			
			if (!IsHabitCurrent(date, startDate, repeatType, daysToRepeat)) return false;
			
			switch (repeatType)
			{
				case RepeatType.Days:
					return true;
				case RepeatType.Years:
					return startDate.Day == date.Day && startDate.Month == date.Month;
			}

			return repeatType.HasFlag(RepeatType.Mon) && (int)date.DayOfWeek == 1 ||
			       repeatType.HasFlag(RepeatType.Tue) && (int)date.DayOfWeek == 2 ||
			       repeatType.HasFlag(RepeatType.Wed) && (int)date.DayOfWeek == 3 ||
			       repeatType.HasFlag(RepeatType.Thu) && (int)date.DayOfWeek == 4 ||
			       repeatType.HasFlag(RepeatType.Fri) && (int)date.DayOfWeek == 5 ||
			       repeatType.HasFlag(RepeatType.Sat) && (int)date.DayOfWeek == 6 ||
			       repeatType.HasFlag(RepeatType.Sun) && (int)date.DayOfWeek == 0;
		}

		#region IsHabitCurrent
		/// <summary>
		/// Calculates whether the given date is between the first day and the last day of the habit.
		/// </summary>
		/// <param name="habit">The habit for which the date will be calculated.</param>
		/// <returns>True if the given date is in between the first and start day.</returns>
		public bool IsHabitCurrent(IHabitViewModel habit, DateTime? date = null) => IsHabitCurrent(date ?? _provider.Now, habit.StartDate, habit.RepeatType, habit.DaysToRepeat);

		/// <summary>
		/// Calculates whether the given date is between the first day and the last day of the habit.
		/// </summary>
		/// <returns>True if the given date is in between the first and start day.</returns>
		private bool IsHabitCurrent(DateTime date, DateTime? startDate, RepeatType repeat, int daysToRepeat)
		{
			return startDate != null && startDate.Value <= date && !IsHabitFinished(date, startDate, repeat, daysToRepeat);
		}
		#endregion

		#region IsHabitFinished

		/// <summary>
		/// Calculates if the last date of the habit is before the given date
		/// </summary>
		/// <param name="habit">The habit for which will be made the calculation.</param>
		/// <param name="date">Date which will be used for measuring if the habit is finished at this date.</param>
		/// <returns>True if the last date of habit is before the given date</returns>
		public bool IsHabitFinished(IHabitViewModel habit, DateTime? date = null) => IsHabitFinished(date ?? _provider.Now, habit.StartDate, habit.RepeatType, habit.DaysToRepeat);
		
		/// <summary>
		/// Calculates if the last date of the habit is before the given date
		/// </summary>
		/// <returns>True if the last date of habit is before the given date</returns>
		private bool IsHabitFinished(DateTime date, DateTime? start, RepeatType repeatType, int daysToRepeat)
		{
			return start != null && start + HabitLength(start.Value, repeatType, daysToRepeat) <= date;
		}
		#endregion
		
		#region HabitLength
		/// <summary>
		/// Returns time between the first day and last day of a habit
		/// </summary>
		/// <returns>The time between first and last day of the habit.</returns>
		public TimeSpan HabitLength(IHabitViewModel habit) => (habit.StartDate != null)
			? HabitLength(habit.StartDate.Value, habit.RepeatType, habit.DaysToRepeat)
			: throw new ArgumentException("Habit must have a StartDate.");

		/// <summary>
		/// Returns time between the first day and last day of a habit
		/// </summary>
		/// <param name="start">The start day of a habit.</param>
		/// <param name="repeatType">The type of repeating of the habit.</param>
		/// <param name="weeksToRepeat">Number of days (or weeks) for which the habit will run.</param>
		/// <returns>The time between first and last day of the habit.</returns>
		private TimeSpan HabitLength(DateTime start, RepeatType repeatType, int weeksToRepeat)
		{
			switch (repeatType)
			{
				case RepeatType.Years:
					return start.AddYears(weeksToRepeat) - start;
				case RepeatType.Days:
					return start.AddDays(weeksToRepeat) - start;
			}

			// The first week can begin later than on the first week day of the repeatType, so we have to treat it specially
			var firstWeekDays = 7 - start.ZeroMondayBasedDay();
			var lastWeekDays  = repeatType.GetZeroBasedWeekDayOfLastDay() + 1;

			if(weeksToRepeat == 1) return TimeSpan.FromDays( lastWeekDays  - (7 - firstWeekDays));


			var middleWeeksDays = (weeksToRepeat - 2) * 7;

			return TimeSpan.FromDays(firstWeekDays + middleWeeksDays + lastWeekDays);
		}
		#endregion

		/// <summary>
		/// Returns index-th day in the filling of the habit
		/// </summary>
		/// <param name="index">Index of the day for which should be calculated the date</param>
		/// <param name="habit">The habit for day date calculation</param>
		/// <returns>If there is no day on the given index (because its higher than index of the last day) then null is returned, othervise the date corresponding to the index.</returns>
		public DateTime? GetFillingDay(int index, IHabitViewModel habit) => habit.StartDate != null
			? GetFillingDay(index, habit.StartDate.Value, habit.RepeatType, habit.DaysToRepeat)
			: throw new ArgumentException("Cannot get filling day on a habit which has a null StartDate");
		private DateTime? GetFillingDay(int index, DateTime startDate, RepeatType repeatType, int daysToRepeat)
		{
			throw new NotImplementedException();
			// Check if the repeatType is setted to days in week
			if (repeatType < RepeatType.Days)
			{
				// Here the thing gets a bit more complicated - we need to index each day, so firstly, lets find the week in which the index is.
				// The first week is a complication - lets resolve it in a greedy way
				int count = 0;
				int i = 0;
				DateTime? firstWeekDate = WeekCalculation(ref i, index, startDate, repeatType, daysToRepeat);
				if (firstWeekDate != null || i >= index)
					return firstWeekDate;
				
				// Calculate count of indexes in one week
				int weekDays = repeatType.GetCountOfWeekDays();
				int weeksToSkip = (index - i) / weekDays;
				int lastWeekIndex =  index - i - weeksToSkip * weekDays;

				// now we need just to find the day of the week according to the lastWeekIndex
				var finalDate = WeekCalculation(ref i, index, startDate, repeatType, daysToRepeat);
				return finalDate;
			}

			if (daysToRepeat >= index)
				return null;

			switch (repeatType)
			{
				case RepeatType.Days:
					return startDate.Date.AddDays(index);
				case RepeatType.Years:
					return startDate.Date.AddYears(index);
			}

			throw new ArgumentException("Not known RepeatType");
		}

		private DateTime? WeekCalculation(ref int i, int index, DateTime startDate, RepeatType repeatType, int daysToRepeat)
		{
			for (DateTime currDate = startDate; currDate.ZeroMondayBasedDay() <= 6; currDate = currDate.AddDays(1))
			{
				int dayInWeek = startDate.ZeroMondayBasedDay();
				if (repeatType.HasFlag(RepeatTypeHelper.GetRepeatTypeForAZeroBasedDay(dayInWeek)))
				{
					if (i >= index)
						return currDate;

					if (i >= daysToRepeat)
						return null;

					i++;
				}
			}

			return null;
		}
	}
}