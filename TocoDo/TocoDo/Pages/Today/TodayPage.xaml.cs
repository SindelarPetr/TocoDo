using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TocoDo.UI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages.Today
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodayPage : ContentPage
	{
		public static TodayPage Instance;

		private ObservableCollection<TaskModel> _todayTasks;
		public ObservableCollection<TaskModel> TodayTasks
		{
			get => _todayTasks;
			set
			{
				_todayTasks = value;
				OnPropertyChanged();
			}
		}

		private Action<DateTime> _globalDatePickerAction;

		public TodayPage()
		{
			Debug.WriteLine("---------- Called constructor of TodayPage");

			InitializeComponent();

			Instance = this;

			Debug.WriteLine("---------- Finished calling of constructor of TodayPage");
		}

		private void ButtonAddToday_OnClicked(object sender, EventArgs e)
		{
			((App) App.Current).Storage.StartCreatingTask(DateTime.Today);

		}
	}
}