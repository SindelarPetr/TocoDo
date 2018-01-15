﻿using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyTaskPage : ContentPage
	{
		private TaskViewModel TaskViewModel => (TaskViewModel)BindingContext;

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