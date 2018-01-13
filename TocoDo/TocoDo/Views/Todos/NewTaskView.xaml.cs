using System;
using System.ComponentModel;
using TocoDo.Pages.Main;
using TocoDo.Services;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewTaskView : ContentView
	{
		public DateTime? DefaulDateTime { get; set; }

		public event Action<TaskViewModel> OnNewTaskCreated;

		public NewTaskView()
		{
			InitializeComponent();
		}


		private async void ButtonConfirm_OnClicked(object sender, EventArgs e)
		{
			var title = EntryTaskTitle.Text.Trim();
			if (string.IsNullOrWhiteSpace(title)) return;

			IsVisible = false;

			var task = await StorageService.InsertTask(title, DefaulDateTime);
			EntryTaskTitle.Text = "";

			OnNewTaskCreated?.Invoke(task);
		}

		private void EntryTaskTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			IsVisible = false;
			EntryTaskTitle.Unfocus();
			Unfocus();
		}

		private void NewTaskView_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(IsVisible))
			{
				if (!IsVisible)
				{
					MainTabbedPage.Instance.EnableSwipePagging();
				}
				else
				{
					EntryTaskTitle.Focus();

					MainTabbedPage.Instance.DisableSwipePagging();
				}
			}
		}
	}
}