using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class HabitUnscheduledFilter : ItemFilter<IHabitViewModel>
    {
	    public HabitUnscheduledFilter() : base(h => h.StartDate == null) { }
    }
}
