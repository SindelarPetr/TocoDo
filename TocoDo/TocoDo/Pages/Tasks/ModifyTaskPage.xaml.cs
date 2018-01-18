using System;
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyTaskPage : ContentPage
	{
		private TaskViewModel TaskViewModel
		{
			get => (TaskViewModel)BindingContext;
			set => BindingContext = value;
		}

		[Obsolete("Used just for Previewer")]
		public ModifyTaskPage()
		{
			TaskViewModel = new TaskViewModel(new Models.TaskModel
			{
				Id = 1,
				CreateTime = DateTime.Now - TimeSpan.FromDays(2),
				Deadline = DateTime.Today + TimeSpan.FromDays(3),
				Description = "This is a brief description of the task.",
				Done = DateTime.Now,
				ScheduleDate = DateTime.Now - TimeSpan.FromDays(1),
				Title = "Do the laundary finally."
			});
			InitializeComponent();
		}

		public ModifyTaskPage(TaskViewModel taskToModify)
		{
			Debug.Write("-------- Called ModifyTaskPage constructor.");
			BindingContext = taskToModify;
			InitializeComponent();
			Debug.Write("-------- Finished calling of ModifyTaskPage constructor.");
		}

		protected override void OnAppearing()
		{
			Debug.Write("------------ OnApeearing of ModifyTaskPage called.");
			base.OnAppearing();
			Debug.Write("------------ Finished calling  OnApearing of ModifyTaskPage.");
		}

		private void EntryTitle_OnCompleted(object sender, FocusEventArgs e)
		{
			var newTitle = EntryTitle.Text.Trim();

			if (string.IsNullOrWhiteSpace(newTitle))
			{
				ResetEntryTitle();
				return;
			}

			TaskViewModel.Title = newTitle;
			TaskViewModel.UpdateCommand.Execute(sender);
		}

		private void ResetEntryTitle()
		{
			EntryTitle.Text = TaskViewModel.Title;
		}
	}
}