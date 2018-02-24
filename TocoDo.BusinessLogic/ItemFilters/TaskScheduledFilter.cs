using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class TaskScheduledFilter : ItemFilter<ITaskViewModel>
    {
	    public TaskScheduledFilter() 
		    : base(t => t.ScheduleDate != null)
	    {

	    }
    }
}
