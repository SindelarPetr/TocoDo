using TocoDo.BusinessLogic.ViewModels;

namespace TocoDo.BusinessLogic.ItemFilters
{
    public class HabitCurrentFilter : ItemFilter<IHabitViewModel>
    {
	    public HabitCurrentFilter() : base(h => h.IsStarted)
	    {

	    }
    }
}
