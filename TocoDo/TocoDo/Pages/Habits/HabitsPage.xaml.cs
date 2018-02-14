using System;
using TocoDo.BusinessLogic.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		public StorageService Storage { get; }

		public HabitsPage()
		{
			Storage = ((App) App.Current).Storage;
			InitializeComponent();
		}

		private void AddButton_OnTapped(object sender, EventArgs e)
		{
			((App)App.Current).Storage.StartCreatingHabit();
		}
	}
}