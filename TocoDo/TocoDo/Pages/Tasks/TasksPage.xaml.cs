using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
		public TasksPage()
		{
			InitializeComponent();
		}

		private void ButtonAddSomeday_OnClicked(object sender, EventArgs e)
		{
			// TODO: Creating a new task
			//StorageServiceOld.AddTaskToTheList(new TaskViewModel());
		}
	}
}