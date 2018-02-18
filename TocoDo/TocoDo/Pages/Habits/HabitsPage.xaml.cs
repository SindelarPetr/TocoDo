using System;
using TocoDo.BusinessLogic.Services;
using TocoDo.UI.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		public HabitsPage()
		{
			//Storage = ((App) Application.Current).StorageService;
			InitializeComponent();
		}

		//public StorageService Storage { get; }

		private void AddButton_OnTapped(object sender, EventArgs e)
		{
			((App) Application.Current).StorageService.StartCreatingHabit();
		}
	}
}