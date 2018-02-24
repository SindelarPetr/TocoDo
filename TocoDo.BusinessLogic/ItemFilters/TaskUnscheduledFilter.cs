using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class TaskUnscheduledFilter : ItemFilter<ITaskViewModel>
    {
	    public TaskUnscheduledFilter() : base(t => t.ScheduleDate == null)
	    {
	    }
    }
}
