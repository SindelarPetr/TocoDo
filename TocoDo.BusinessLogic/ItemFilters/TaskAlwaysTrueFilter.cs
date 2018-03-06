using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
	public class TaskAlwaysTrueFilter : ItemFilter<ITaskViewModel>
	{
		public TaskAlwaysTrueFilter() : base(t => true)
		{

		}
	}
}
