﻿using System.Diagnostics;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyTaskPage : ContentPage
	{
		public ModifyTaskPage(TaskViewModel taskToModify)
		{
			MyLogger.WriteStartMethod();
			BindingContext = taskToModify;
			InitializeComponent();
			MyLogger.WriteEndMethod();
		}

		public TaskViewModel TaskViewModel
		{
			get => (TaskViewModel) BindingContext;
			set => BindingContext = value;
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