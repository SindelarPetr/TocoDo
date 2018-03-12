using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.Helpers;

namespace TocoDo.BusinessLogic.DependencyInjection.Models
{
	public static class RepeatTypeHelper
	{
		public static int GetZeroBasedWeekDayOfLastDay(this RepeatType repeatType)
		{
			if (repeatType.HasFlag(RepeatType.Sun)) return 6;
			if (repeatType.HasFlag(RepeatType.Sat)) return 5;
			if (repeatType.HasFlag(RepeatType.Fri)) return 4;
			if (repeatType.HasFlag(RepeatType.Thu)) return 3;
			if (repeatType.HasFlag(RepeatType.Wed)) return 2;
			if (repeatType.HasFlag(RepeatType.Tue)) return 1;
			if (repeatType.HasFlag(RepeatType.Mon)) return 0;

			throw new ArgumentException("The given repeatType does not contain any day specification, no day number can be assinged to it.");
		}
		public static int GetZeroBasedWeekDayOfFirstDay(this RepeatType repeatType)
		{
			if (repeatType.HasFlag(RepeatType.Mon)) return 0;
			if (repeatType.HasFlag(RepeatType.Tue)) return 1;
			if (repeatType.HasFlag(RepeatType.Wed)) return 2;
			if (repeatType.HasFlag(RepeatType.Thu)) return 3;
			if (repeatType.HasFlag(RepeatType.Fri)) return 4;
			if (repeatType.HasFlag(RepeatType.Sat)) return 5;
			if (repeatType.HasFlag(RepeatType.Sun)) return 6;

			throw new ArgumentException("The given repeatType does not contain any day specification, no day number can be assinged to it.");
		}

		public static int GetCountOfWeekDays(this RepeatType repeatType)
		{
			int count = 0;

			if (repeatType.HasFlag(RepeatType.Mon)) count++;
			if (repeatType.HasFlag(RepeatType.Tue)) count++;
			if (repeatType.HasFlag(RepeatType.Wed)) count++;
			if (repeatType.HasFlag(RepeatType.Thu)) count++;
			if (repeatType.HasFlag(RepeatType.Fri)) count++;
			if (repeatType.HasFlag(RepeatType.Sat)) count++;
			if (repeatType.HasFlag(RepeatType.Sun)) count++;

			return count;
		}

		public static RepeatType GetRepeatTypeForAZeroBasedDay(int zeroBasedDay)
		{
			if(zeroBasedDay >= 7)
			throw new ArgumentException($"There is no RepeatType day acctording to the number: { zeroBasedDay }");

			return (RepeatType) (1 << zeroBasedDay);
		}

		public static DateTime? AdjustDateToRepeatType(DateTime? date, RepeatType repeatType)
		{
			if (date == null)
				return null;

			var maxIterations = 7;
			while (!repeatType.HasFlag(RepeatTypeHelper.GetRepeatTypeForAZeroBasedDay(date.Value.ZeroMondayBasedDay())) && maxIterations-- > 0)
			{
				date = date.Value.AddDays(1);
			}

			return date;
		}
	}
}
