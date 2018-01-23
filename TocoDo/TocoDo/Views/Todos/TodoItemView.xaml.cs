﻿
using System;
using System.Diagnostics;
using TocoDo.Pages;
using TocoDo.Pages.Main;
using TocoDo.Pages.Tasks;
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

			InitializeComponent();
			BindingContext = model;
		}

		private async void TapTitle_OnTapped(object sender, EventArgs e)
		{
			Debug.WriteLine("------- Called TapTitle_OnTapped.");

			var page = new ModifyTaskPage(TaskViewModel);

			Debug.WriteLine("------- Created instance of ModifyTaskPage in TapTitle_OnTapped.");

			await PageService.PushAsync(page);

			Debug.WriteLine("------- Finished calling of TapTitle_OnTapped.");
		}

		private void TapCalendar_OnTapped(object sender, EventArgs e)
		{
			// TODO: Get rid of handling View control
			TodayPage.Instance.ShowGlobalDatePicker(TaskViewModel.Deadline ?? DateTime.Today, d => ChangeDate(d));
		}

		// TODO: Get rid of this
		private async void ChangeDate(DateTime? date)
		{
			try
			{
				Debug.WriteLine("Started ChangeDate");
				if (date == TaskViewModel.Deadline)
					return;
				DateTime? originDateTime = TaskViewModel.Deadline;
				TaskViewModel.Deadline = date;
				await StorageService.UpdateTask(TaskViewModel);
				Debug.WriteLine("Ended ChangeDate");
			}
			catch (Exception e)
			{
				Debug.WriteLine("Through exception with details: " + e.Message);
			}
		}
	}
}