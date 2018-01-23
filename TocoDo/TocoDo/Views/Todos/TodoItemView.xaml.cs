
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
			BindingContext = model;
			InitializeComponent();
		}

		private async void TapTitle_OnTapped(object sender, EventArgs e)
		{
			Debug.WriteLine("------- Called TapTitle_OnTapped.");

			var page = new ModifyTaskPage(TaskViewModel);

			Debug.WriteLine("------- Created instance of ModifyTaskPage in TapTitle_OnTapped.");

			await PageService.PushAsync(page);

			Debug.WriteLine("------- Finished calling of TapTitle_OnTapped.");
		}
	}
}