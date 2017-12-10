using System;
using TocoDo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
		public static TasksPage Instance;

		// For creating a new task
		public TasksPage()
		{
			InitializeComponent();

			Instance = this;
		}

		private void ButtonAddSomeday_OnClicked(object sender, EventArgs e)
		{
			NewTaskView.DefaulDateTime = null;
			ShowNewTaskView();
		}

		private void ShowNewTaskView()
		{
			NewTaskView.IsVisible = true;
			NewTaskView.Focus();
		}
	}
}