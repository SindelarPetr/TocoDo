using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TocoDo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TocoDo.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitRepeatTypePopup : PopupPage
	{
		public BindableProperty DaysToRepeatProperty = BindableProperty.Create(nameof(DaysToRepeat), typeof(int), typeof(int), 21);

		public int DaysToRepeat
		{
			get => (int) GetValue(DaysToRepeatProperty);
			set => SetValue(DaysToRepeatProperty, value);
		}

		private List<KeyValuePair<int, string>> _pickerValues;
		private RepeatType _selectedRepeatType;

		public event Action<RepeatType, int> Save;
		public HabitRepeatTypePopup(RepeatType repeatType, int daysToRepeat)
		{
			InitializeComponent ();

			_pickerValues = new List<KeyValuePair<int, string>>
			{
				new KeyValuePair<int, string>((int) RepeatType.Days, Properties.Resources.Days),
				new KeyValuePair<int, string>(-1, Properties.Resources.Weeks),
				new KeyValuePair<int, string>((int) RepeatType.Months, Properties.Resources.Months),
				new KeyValuePair<int, string>((int) RepeatType.Years, Properties.Resources.Years)
			};

			var list = _pickerValues.Select(p => p.Value).ToList();
			Picker.ItemsSource = list;

			// Init values in entry and picker
			DaysToRepeat = daysToRepeat;

			if (repeatType < RepeatType.Months)
			{
				SetDaysInWeek(repeatType);
				Picker.SelectedIndex = 1;
			}
			else
				Picker.SelectedItem = _pickerValues.First(p => p.Key == (int) repeatType).Value;
		}

		private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			int index = Picker.SelectedIndex;

			// I need to get the repeat type
			int key = _pickerValues[index].Key;

			// -1 indicates selection of Week which means that we have to get the exact days in a week.
			if (key == -1)
			{
				_selectedRepeatType = GetDaysInWeek();
			}
			else
			{
				_selectedRepeatType = (RepeatType) key;
			}
		}

		private RepeatType GetDaysInWeek()
		{
			// TODO: Check toggled buttons selecting week days
			return RepeatType.Fri;
		}

		private void SetDaysInWeek(RepeatType repeatType)
		{
			if (repeatType >= RepeatType.Months)
				return;

			// TODO: Toggle correct day in week buttons
		}

		private async void Cancel_OnClicked(object sender, EventArgs e)
		{
			//Save.GetInvocationList().ForEach(i => Save -= (Action<RepeatType, int>)i);
			await Navigation.PopPopupAsync();
		}

		private async void Save_OnClicked(object sender, EventArgs e)
		{
			Save?.Invoke(_selectedRepeatType, DaysToRepeat);
			await Navigation.PopPopupAsync();
		}
	}
}