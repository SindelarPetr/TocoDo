using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		public HabitsPage()
		{
			InitializeComponent();
		}

		private void AddButton_OnTapped(object sender, EventArgs e)
		{
			((App)App.Current).Storage.StartCreatingHabit();
		}
	}
}