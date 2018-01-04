using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			Debug.Write("------------ Finished calling  OnApeearing of ModifyTaskPage.");
		}

		private void EntryTitle_OnCompleted(object sender, FocusEventArgs e)
		{
			TaskViewModel.UpdateCommand.Execute(sender);
		}
	}
}