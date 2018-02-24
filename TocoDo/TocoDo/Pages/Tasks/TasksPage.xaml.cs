using System;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.Services;
using TocoDo.UI.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPage : ContentPage
	{
		public TasksPage()
		{
			MyLogger.WriteStartMethod();
			try
			{
				InitializeComponent();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}
			MyLogger.WriteEndMethod();
		}

		private void ButtonAddSomeday_OnClicked(object sender, EventArgs e)
		{
			((App)App.Current).TaskService.StartCreation(null);
		}
	}
}