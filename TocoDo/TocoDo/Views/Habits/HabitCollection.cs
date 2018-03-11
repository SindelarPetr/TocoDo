using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Habits
{
	public class HabitCollection : ItemCollection<IHabitViewModel, BaseHabitView>
	{
		public HabitCollection(bool showActive = false) : base(vm => vm.IsActive() && showActive ? (BaseHabitView)new ActualHabitView(vm) : (BaseHabitView)new HabitView(vm), nameof(IHabitViewModel.StartDate))
		{
		}
	}
}