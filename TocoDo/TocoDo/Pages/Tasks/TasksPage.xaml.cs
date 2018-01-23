using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
		// Todo: Remove static refence to the page
		public static TasksPage Instance;
		
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