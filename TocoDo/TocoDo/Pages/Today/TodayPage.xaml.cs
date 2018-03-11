using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NetBox.Extensions;
using TocoDo.UI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Today
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
		public TodayPage()
		{
			Debug.WriteLine("---------- Called constructor of TodayPage");

			InitializeComponent();

			Debug.WriteLine("---------- Finished calling of constructor of TodayPage");
		}

		private void ButtonAddToday_OnClicked(object sender, EventArgs e)
		{
			if(Calendar.SelectedDate != null)
			((App) Application.Current).TaskService.StartCreation(Calendar.SelectedDate.Value);
		}
	}
}