using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;

namespace TocoDo.UI.Views.Habits
{
	public class HabitCollection : ItemCollection<IHabitViewModel, HabitView>
	{
		public HabitCollection() : base(vm => new HabitView(vm), nameof(IHabitViewModel.StartDate))
		{
		}
	}
}