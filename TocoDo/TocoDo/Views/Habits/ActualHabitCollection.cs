using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Habits
{
	public class ActualHabitCollection : ItemCollection<HabitViewModel, ActualHabitView>
	{
		public ActualHabitCollection() : base(vm => new ActualHabitView(vm))
		{
		}
	}
}