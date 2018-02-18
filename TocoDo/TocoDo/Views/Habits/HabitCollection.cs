using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Habits
{
	public class HabitCollection : ItemCollection<HabitViewModel, HabitView>
	{
		public HabitCollection() : base(vm => new HabitView(vm))
		{
		}
	}
}