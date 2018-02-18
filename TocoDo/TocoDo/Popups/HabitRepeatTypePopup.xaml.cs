using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitRepeatTypePopup : PopupPage
	{
		private readonly List<KeyValuePair<int, string>> _pickerValues;

		private RepeatType lastWeeksRepeat =
			RepeatType.Mon | RepeatType.Tue | RepeatType.Wed | RepeatType.Thu | RepeatType.Fri;

		public HabitRepeatTypePopup(RepeatType repeatType, int daysToRepeat)
		{
			SelectDayCommand = new Command(SelectDayCommandExecute);
			InitializeComponent();


			_pickerValues = new List<KeyValuePair<int, string>>
			{
				new KeyValuePair<int, string>((int) RepeatType.Days,   BusinessLogic.Properties.Resources.Days),
				new KeyValuePair<int, string>(-1,                      BusinessLogic.Properties.Resources.Weeks),
				new KeyValuePair<int, string>((int) RepeatType.Months, BusinessLogic.Properties.Resources.Months),
				new KeyValuePair<int, string>((int) RepeatType.Years,  BusinessLogic.Properties.Resources.Years)
			};

			var list           = _pickerValues.Select(p => p.Value).ToList();
			Picker.ItemsSource = list;

			// Init values in entry and picker
			DaysToRepeat       = daysToRepeat;
			SelectedRepeatType = repeatType;
			if (repeatType < RepeatType.Days)
			{
				lastWeeksRepeat      = repeatType;
				Picker.SelectedIndex = 1;
			}
			else
			{
				Picker.SelectedItem = _pickerValues.First(p => p.Key == (int) repeatType).Value;
			}
		}

		public ICommand SelectDayCommand { get; set; }

		public event Action<RepeatType, int> Save;

		private void SelectDayCommandExecute(object o)
		{
			var btn = (SelectDayButton) o;

			if (!btn.IsToggled)
				SelectedRepeatType |= btn.RepeatType;
			else
				SelectedRepeatType &= ~btn.RepeatType;
		}

		private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			var index = Picker.SelectedIndex;

			// I need to get the repeat type
			var key = _pickerValues[index].Key;

			// -1 indicates selection of Week which means that we have to get the exact days in a week.
			if (key == -1)
			{
				SelectedRepeatType = lastWeeksRepeat;
			}
			else
			{
				// If there were days seles
				if (SelectedRepeatType < RepeatType.Days)
					lastWeeksRepeat = SelectedRepeatType;

				SelectedRepeatType = (RepeatType) key;
			}
		}

		private async void Cancel_OnClicked(object sender, EventArgs e)
		{
			//Save.GetInvocationList().ForEach(i => Save -= (Action<RepeatType, int>)i);
			await Navigation.PopPopupAsync();
		}

		private async void Save_OnClicked(object sender, EventArgs e)
		{
			Save?.Invoke(SelectedRepeatType, DaysToRepeat);
			await Navigation.PopPopupAsync();
		}

		#region Backing fields

		public BindableProperty DaysToRepeatProperty =
			BindableProperty.Create(nameof(DaysToRepeat), typeof(int), typeof(int), 21);

		public BindableProperty SelectedRepeatTypeProperty = BindableProperty.Create(nameof(SelectedRepeatType),
			typeof(RepeatType), typeof(RepeatType), RepeatType.Days);

		#endregion

		#region Properties

		public int DaysToRepeat
		{
			get => (int) GetValue(DaysToRepeatProperty);
			set => SetValue(DaysToRepeatProperty, value);
		}

		public RepeatType SelectedRepeatType
		{
			get => (RepeatType) GetValue(SelectedRepeatTypeProperty);
			set => SetValue(SelectedRepeatTypeProperty, value);
		}

		#endregion
	}
}