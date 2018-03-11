using System;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Global;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Habits
{
	public class HabitCollection : ItemCollection<IHabitViewModel, BaseHabitView>
	{
		public HabitCollection() : base((vm, param) => ((CalendarView) param)?.SelectedDate != null && ((CalendarView)param).SelectedDate == DateTime.Today ? (BaseHabitView)new ActualHabitView(vm) : (BaseHabitView)new HabitView(vm), nameof(IHabitViewModel.StartDate))
		{
		}
	}
}