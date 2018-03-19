using System;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Habits;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Todos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TaskView : ContentView, IEntryFocusable<ITaskViewModel>
	{
		public TaskView(ITaskViewModel model)
		{
			BindingContext = model;
			InitializeComponent();
		}

		public ITaskViewModel ViewModel
		{
			get => (ITaskViewModel) BindingContext;
			set => BindingContext = value;
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}

		private async void TapTitle_OnTapped(object sender, EventArgs e)
		{
		    await ViewModel.EditCommand.ExecuteAsync(null);
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			ViewModel.FinishCreationCommand?.Execute(EntryEditTitle.Text);
			EntryEditTitle.Text = ViewModel.Title;
		}
	}
}