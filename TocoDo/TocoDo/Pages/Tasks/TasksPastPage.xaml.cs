using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Tasks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TasksPastPage : ContentPage
	{
		public TasksPastViewModel ViewModel { get; }

		public TasksPastPage ()
		{
			ViewModel = new TasksPastViewModel(((App)App.Current).Navigation, ((App)App.Current).TaskService);
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.LoadPastTasksCommand.ExecuteAsync(null);
		}
	}
}