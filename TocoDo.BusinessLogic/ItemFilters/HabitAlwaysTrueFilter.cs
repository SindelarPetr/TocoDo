using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
	public class HabitAlwaysTrueFilter : ItemFilter<IHabitViewModel>
	{
		public HabitAlwaysTrueFilter() : base(h => true)
		{

		}
	}
}
