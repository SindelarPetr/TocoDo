using System;
using System.Collections.Generic;
using System.Text;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Habits
{
	public abstract class BaseHabitView : ContentView, IEntryFocusable<IHabitViewModel>
	{
		public IHabitViewModel ViewModel { get; set; }
		public abstract void FocusEntry();

		protected BaseHabitView(IHabitViewModel viewModel)
		{
			ViewModel = viewModel;
		}
	}
}
