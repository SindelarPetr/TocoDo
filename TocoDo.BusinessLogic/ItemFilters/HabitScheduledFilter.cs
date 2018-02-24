using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class HabitScheduledFilter : ItemFilter<IHabitViewModel>
    {
	    public HabitScheduledFilter() : base(h => !h.IsStarted && h.StartDate != null) { }
    }
}
