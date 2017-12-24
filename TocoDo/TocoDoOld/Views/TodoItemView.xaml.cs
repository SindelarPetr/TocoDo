
using System;
using System.Diagnostics;
using TocoDo.Pages;
using TocoDo.Pages.Main;
using TocoDo.Services;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoItemView : ContentView
	{
		public TaskViewModel TaskViewModel
		{
			get => (TaskViewModel)BindingContext;
			set => BindingContext = value;
		}

		public TodoItemView(TaskViewModel model)
		{
			BindingContext = model;

			InitializeComponent();
		}



		#region Animations
		private void AnimateCheck()
		{
			//LabelTitle.FadeTo(0.4);
		}

		private void AnimateUncheck()
		{
			//LabelTitle.FadeTo(1);
		}
		#endregion

		private async void TapTitle_OnTapped(object sender, EventArgs e)
		{
			var page = new ModifyTaskPage(TaskViewModel);

			await Navigation.PushAsync(page);
		}

		private void TapCalendar_OnTapped(object sender, EventArgs e)
		{
			TodayPage.Instance.ShowGlobalDatePicker(TaskViewModel.Deadline ?? DateTime.Today, d => ChangeDate(d));
		}

		private void ChangeDate(DateTime? date)
		{
			try
			{
				Debug.WriteLine("Started ChangeDate");
				if (date == TaskViewModel.Deadline)
					return;
				DateTime? originDateTime = TaskViewModel.Deadline;
				TaskViewModel.Deadline = date;
				StorageService.UpdateTask(TaskViewModel);
				//TODO: OnDateChanged?.Invoke(this, originDateTime);
				Debug.WriteLine("Ended ChangeDate");
			}
			catch (Exception e)
			{
				Debug.WriteLine("Through exception with details: " + e.Message);
			}
		}
	}
}