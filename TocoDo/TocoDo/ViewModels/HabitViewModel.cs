﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.Converters;
using TocoDo.Models;
using TocoDo.Pages.Habits;
using TocoDo.Pages.Main;
using TocoDo.Properties;
using TocoDo.Services;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
	public class HabitViewModel : BaseViewModel
	{
		private bool _updateOnPropertyChange;

		#region Backing fields
		private bool _modelIsRecommended;
		private int _modelDailyFillingCount;
		private HabitType _modelHabitType;
		private short _modelDaysToRepeat;
		private DateTime? _modelStartDate;
		private string _modelTitle;
		private RepeatType _modelRepeatType;
		private string _modelDescription;
		private short _modelRepeatsADay;
		#endregion

		#region Properties
		public int ModelId { get; private set; }

		/// <summary>
		/// Is used for explicit setting of ModelId, so there is no possibility of setting ModelId accidentally.
		/// </summary>
		/// <param name="id">New id which will be assigned to the ModelId property.</param>
		public void SetModelId(int id) => ModelId = id;

		public bool ModelIsRecommended
		{
			get => _modelIsRecommended;
			set => SetValue(ref _modelIsRecommended, value);
		}

		/// <summary>
		/// How many times the habit was violated / performed today
		/// </summary>
		public int ModelDailyFillingCount
		{
			get => _modelDailyFillingCount;
			set => SetValue(ref _modelDailyFillingCount, value);
		}

		public ObservableDictionary<DateTime, int> ModelFilling { get; }

		public HabitType ModelHabitType
		{
			get => _modelHabitType;
			set => SetValue(ref _modelHabitType, value);
		}

		public short ModelDaysToRepeat
		{
			get => _modelDaysToRepeat;
			set => SetValue(ref _modelDaysToRepeat, value);
		}

		public DateTime? ModelStartDate
		{
			get => _modelStartDate;
			set
			{
				SetValue(ref _modelStartDate, value); 
				OnPropertyChanged(".");
			}
		}

		public string ModelTitle
		{
			get => _modelTitle;
			set => SetValue(ref _modelTitle, value);
		}

		public RepeatType ModelRepeatType
		{
			get => _modelRepeatType;
			set
			{
				SetValue(ref _modelRepeatType, value);
				OnPropertyChanged(".");
			}
		}

		public string ModelDescription
		{
			get => _modelDescription;
			set => SetValue(ref _modelDescription, value);
		}

		public string DateText { get; set; }

		/// <summary>
		/// Only for Unit habit. Count of times to repeat the habit each day
		/// </summary>
		public short ModelRepeatsADay
		{
			get => _modelRepeatsADay;
			set => SetValue(ref _modelRepeatsADay, value);
		}
		#endregion

		#region IsEditTitleMode
		public static BindableProperty IsEditTitleModeProperty = BindableProperty.Create("IsEditTitleMode",
			typeof(bool), typeof(bool), false);


		public bool IsEditTitleMode
		{
			get => (bool)GetValue(IsEditTitleModeProperty);
			set => SetValue(IsEditTitleModeProperty, value);
		}
		#endregion

		#region Commands
		public ICommand EditCommand { get; private set; }
		public ICommand UpdateCommand { get; private set; }
		public ICommand InsertCommand { get; private set; }
		public ICommand SelectStartDateCommand { get; private set; }
		public ICommand UnsetStartDateCommand { get; private set; }
		public ICommand SelectRepeatCommand { get; private set; }
		#endregion

		public HabitViewModel()
		{
			ModelFilling = new ObservableDictionary<DateTime, int>();
			IsEditTitleMode = true;

			SetupCommands();
		}

		public HabitViewModel(HabitModel model)
		{
			ModelId = model.Id;
			_modelRepeatType = model.RepeatType;
			_modelDescription = model.Description;
			ModelFilling = new ObservableDictionary<DateTime, int>(model.Filling);
			_modelHabitType = model.HabitType;
			_modelDaysToRepeat = model.DaysToRepeat;
			_modelStartDate = model.StartDate;
			_modelTitle = model.Title;
			_modelDailyFillingCount = model.DailyFillingCount;
			_modelIsRecommended = model.IsRecommended;
			_modelRepeatsADay = model.RepeatsADay;

			SetupCommands();

			_updateOnPropertyChange = true;
		}

		private void SetupCommands()
		{
			InsertCommand = new Command<string>(async s => await InsertToStorage(s));
			EditCommand = new Command(async () => await PageService.PushAsync(new ModifyHabitPage(this)));

			SelectStartDateCommand = new Command(async () => await SelectDate(d => ModelStartDate = d, Resources.SelectStartDate));
			UnsetStartDateCommand = new Command(() => ModelStartDate = null);
		}

		private async Task SelectDate(Action<DateTime?> pickedAction, string actionSheetHeader)
		{
			string[] buttons = { Resources.Today, Resources.Tomorrow, Resources.TheDayAfterTomorrow, Resources.PickADate };

			string result = await PageService.DisplayActionSheet(actionSheetHeader, Resources.Cancel, null, buttons);


			DateTime selectedDate;
			if (result == Resources.Today)
			{
				selectedDate = DateTime.Today;
			}
			else if (result == Resources.Tomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(1);
			}
			else if (result == Resources.TheDayAfterTomorrow)
			{
				selectedDate = DateTime.Today + TimeSpan.FromDays(2);
			}
			else if (result == Resources.PickADate)
			{
				SelectDateByPicker(d => pickedAction(d));
				return;
			}
			else
				return;

			pickedAction(selectedDate);
		}

		private void SelectDateByPicker(Action<DateTime> pickedAction)
		{
			TodayPage.Instance.ShowGlobalDatePicker(ModelStartDate ?? DateTime.Today + TimeSpan.FromDays(1), pickedAction, DateTime.Today + TimeSpan.FromDays(1));
		}

		public async Task InsertToStorage(string title)
		{
			_modelTitle = title;
			IsEditTitleMode = false;
			await StorageService.InsertHabit(this);
			OnPropertyChanged(nameof(ModelTitle));
		}

		public HabitModel GetHabitModel()
		{
			return new HabitModel
			{
				Id = ModelId,
				RepeatType = ModelRepeatType,
				Description = ModelDescription,
				Filling = new Dictionary<DateTime, int>(ModelFilling),
				HabitType = ModelHabitType,
				DaysToRepeat = ModelDaysToRepeat,
				StartDate = ModelStartDate,
				Title = ModelTitle,
				DailyFillingCount = ModelDailyFillingCount,
				IsRecommended = ModelIsRecommended,
				RepeatsADay = ModelRepeatsADay
			};
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (_updateOnPropertyChange && propertyName != nameof(IsEditTitleMode))
				StorageService.UpdateHabit(this);
		}
	}
}