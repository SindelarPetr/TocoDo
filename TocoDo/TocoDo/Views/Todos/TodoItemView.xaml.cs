using System;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Habits;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Todos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoItemView : ContentView, IEntryFocusable<TaskViewModel>
	{
		public TaskViewModel ViewModel
		{
			get => (TaskViewModel)BindingContext;
			set => BindingContext = value;
		}

		public TodoItemView(TaskViewModel model)
		{
			BindingContext = model;
			InitializeComponent();
		}

		private void TapTitle_OnTapped(object sender, EventArgs e)
		{
			ViewModel.EditCommand?.Execute(null);
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			ViewModel.EditTitleCommand?.Execute(EntryEditTitle.Text);
			EntryEditTitle.Text = ViewModel.Title;
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}