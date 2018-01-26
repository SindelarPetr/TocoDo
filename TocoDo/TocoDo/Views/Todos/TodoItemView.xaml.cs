
using System;
using System.Diagnostics;
using TocoDo.Pages;
using TocoDo.Pages.Main;
using TocoDo.Pages.Tasks;
using TocoDo.Services;
using TocoDo.ViewModels;
using TocoDo.Views.Habits;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
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

		private async void TapTitle_OnTapped(object sender, EventArgs e)
		{
			Debug.WriteLine("------- Called TapTitle_OnTapped.");

			var page = new ModifyTaskPage(ViewModel);

			Debug.WriteLine("------- Created instance of ModifyTaskPage in TapTitle_OnTapped.");

			await PageService.PushAsync(page);

			Debug.WriteLine("------- Finished calling of TapTitle_OnTapped.");
		}

		private void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			var title = ((Entry)e.VisualElement).Text;
			// If user left the entry blank, then remove the task from collection
			if (string.IsNullOrWhiteSpace(title))
			{
				StorageService.RemoveTaskFromTheList(ViewModel);
				return;
			}

			ViewModel.InsertToStorage(title);
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}
	}
}