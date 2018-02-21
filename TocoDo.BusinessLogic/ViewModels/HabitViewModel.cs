﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Extensions;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class HabitViewModel : BaseViewModel, IHabitViewModel
	{
		#region Dependency injection
		private readonly INavigationService _navigation;
		private readonly IHabitService _habitService;
		#endregion

		#region Backing fields

		private bool _isRecommended;
		private int _repeatsToday;
		private HabitType _habitType;
		private int _daysToRepeat;
		private DateTime? _startDate;
		private string _title;
		private RepeatType _repeatType;
		private string _description;
		private int _maxRepeatsADay;

		#endregion

		#region Properties

		public int Id { get; private set; }

		/// <summary>
		///     Is used for explicit setting of ModelId, so there is no possibility of setting ModelId accidentally.
		/// </summary>
		/// <param name="id">New id which will be assigned to the ModelId property.</param>
		public void SetModelId(int id)
		{
			Id = id;
		}

		public bool IsRecommended
		{
			get => _isRecommended;
			set => SetValue(ref _isRecommended, value);
		}

		/// <summary>
		///     How many times the habit was violated / performed today
		/// </summary>
		public int RepeatsToday
		{
			get => _repeatsToday;
			set
			{
				if (DateTimeHelper.IsHabitToday(StartDate, RepeatType, DaysToRepeat))
					SetValue(ref _repeatsToday, value);
			}
		}

		public ObservableDictionary<DateTime, int> Filling { get; private set; }

		public DateTime CreationDate { get; }

		public HabitType HabitType
		{
			get => _habitType;
			set
			{
				SetValue(ref _habitType, value);
				OnPropertyChanged(nameof(HabitTypeWithRepeats));
			}
		}

		public int DaysToRepeat
		{
			get => _daysToRepeat;
			set
			{
				SetValue(ref _daysToRepeat, value);
				OnPropertyChanged(nameof(HabitDaysToRepeatWithRepeatType));
			}
		}

		public DateTime? StartDate
		{
			get => _startDate;
			set => SetValue(ref _startDate, value);
		}

		public string Title
		{
			get => _title;
			set => SetValue(ref _title, value);
		}

		public RepeatType RepeatType
		{
			get => _repeatType;
			set
			{
				SetValue(ref _repeatType, value);
				OnPropertyChanged(nameof(HabitDaysToRepeatWithRepeatType));
			}
		}

		public string Description
		{
			get => _description;
			set => SetValue(ref _description, value);
		}

		/// <summary>
		///     Only for Unit habit. Count of times to repeat the habit each day
		/// </summary>
		public int MaxRepeatsADay
		{
			get => _maxRepeatsADay;
			set
			{
				SetValue(ref _maxRepeatsADay, value);
				OnPropertyChanged(nameof(HabitTypeWithRepeats));
			}
		}

		public bool IsCreateMode { get; set; }

		public bool IsStarted => StartDate != null && StartDate.Value < DateTime.Now;

		public bool IsFinished { get; set; }

		public string HabitTypeWithRepeats => HabitType == HabitType.Daylong
			? Resources.HabitDetailHabitTypeDaylong
			: string.Format(Resources.HabitDetailHabitTypeTextUnit, MaxRepeatsADay);

		public string HabitDaysToRepeatWithRepeatType
		{
			get
			{
				string timeScale;
				switch (RepeatType)
				{
					case RepeatType.Days:
						timeScale = Resources.Days;
						break;
					case RepeatType.Months:
						timeScale = Resources.Months;
						break;
					case RepeatType.Years:
						timeScale = Resources.Years;
						break;
					default:
						timeScale = Resources.Weeks;
						break;
				}

				return $"{Resources.For} {DaysToRepeat} {timeScale}";
			}
		}

		#endregion

		#region Commands

		public ICommand EditTitleCommand       { get; private set; }
		public ICommand EditCommand            { get; private set; }
		public ICommand UnsetStartDateCommand { get; }
		public ICommand UpdateCommand          { get; private set; }
		public ICommand FinishCreationCommand { get; private set; }
		public ICommand IncreaseTodayCommand   { get; private set; }
		public ICommand RemoveCommand          { get; private set; }
		public ICommand SelectRepeatCommand { get; }
		public ICommand SelectStartDateCommand { get; }

		#endregion

		public HabitViewModel(IHabitService habitService, INavigationService navigation) 
			: this()
		{
			_habitService    = habitService;
			_navigation = navigation;

			// Set initial values
			CreationDate    = DateTime.Now;
			_repeatType     = RepeatType.Days;
			_habitType      = HabitType.Daylong;
			_maxRepeatsADay = 1;
			_daysToRepeat   = 21;

			Filling = new ObservableDictionary<DateTime, int>();
			IsCreateMode = true;
		}

		public HabitViewModel(IHabitService habitService, INavigationService navigation, IHabitModel model) 
			: this()
		{
			_habitService    = habitService;
			_navigation = navigation;

			CreationDate = model.CreationDate;
			Id           = model.Id;
			_repeatType  = model.RepeatType;
			_description = model.Description;
			Filling      =
				new ObservableDictionary<DateTime, int>(
					JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(model.Filling));
			_habitType      = model.HabitType;
			_daysToRepeat   = model.DaysToRepeat;
			_startDate      = model.StartDate;
			_title          = model.Title;
			_repeatsToday   = Filling.ContainsKey(DateTime.Today) ? Filling[DateTime.Today] : 0;
			_isRecommended  = model.IsRecommended;
			_maxRepeatsADay = model.RepeatsADay;
		}

		private HabitViewModel()
		{
			UpdateCommand = new Command(async () => await _habitService.UpdateAsync(this));
			EditTitleCommand       = new Command<string>(EditTitle);
			FinishCreationCommand = new Command(async t => await ConfirmCreation(t));
			EditCommand            = new Command(async () => await Edit());

			SelectStartDateCommand                                        =
				new Command(async () => await SelectDate(d => StartDate = d, Resources.SelectStartDate));
			UnsetStartDateCommand                                         = new Command(() => StartDate = null);
			IncreaseTodayCommand                                          = new Command(() => RepeatsToday++);
			RemoveCommand                                                 = new Command(async () => await Delete());
		}

		private async Task Edit()
		{
			if (IsStarted)
				await _navigation.PushAsync(PageType.HabitProgressPage, this);
			else
				await _navigation.PushAsync(PageType.ModifyHabitPage, this);
		}

		private void EditTitle(string newTitle)
		{
			newTitle = newTitle.Trim();
			if (string.IsNullOrWhiteSpace(newTitle))
				return;

			Title = newTitle;
		}

		private async Task Delete()
		{
			// Ask user if he is sure
			var result = await _navigation.DisplayAlert(Resources.DeleteHabitConfirmHeader, Resources.DeleteHabitConfirmText,
			                                            Resources.Yes, Resources.Cancel);

			if (result)
				await _habitService.DeleteAsync(this);
		}

		private async Task ConfirmCreation(object title)
		{
			if (title is string tit)
			{
				// If user left the entry blank, then remove the habit from collection
				if (string.IsNullOrWhiteSpace(tit))
				{
					_habitService.CancelCreation(this);
					return;
				}

				Title = tit.Trim();
				await _habitService.ConfirmCreationAsync(this);
			}
		}

		private async Task SelectDate(Action<DateTime?> pickedAction, string actionSheetHeader)
		{
			string[] buttons = {Resources.Today, Resources.Tomorrow, Resources.TheDayAfterTomorrow, Resources.PickADate};

			var result = await _navigation.DisplayActionSheet(actionSheetHeader, Resources.Cancel, null, buttons);

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
			{
				return;
			}

			pickedAction(selectedDate);
		}
		
		private void SelectDateByPicker(Action<DateTime> pickedAction)
		{
			//TODO: Workaround for showing the date picker TodayPage.Instance.ShowGlobalDatePicker(ModelStartDate ?? DateTime.Today + TimeSpan.FromDays(1), pickedAction, DateTime.Today + TimeSpan.FromDays(1));
		}

		protected override async void OnPropertyChanged(string propertyName = null)
		{
			MyLogger.WriteStartMethod();
			base.OnPropertyChanged(propertyName);

			// Init filling if needed and update it
			if (propertyName == nameof(RepeatsToday))
			{
				InitFilling();
				if (Filling == null || !Filling.Any()) InitFilling();

				var today   = DateTime.Today;
				var filling = Filling;
				if (filling.ContainsKey(today))
					filling[today] = RepeatsToday;
			}

			if (!IsCreateMode)
			{
				await _habitService.UpdateAsync(this);
				MyLogger.WriteInMethod($"Updated habit, changed property: {propertyName}");
			}

			MyLogger.WriteEndMethod();
		}

		/// <summary>
		///     Fills all dates when the habit will (or supposed to) be performed
		/// </summary>
		private void InitFilling()
		{
			var modelStartDate  = StartDate;
			var modelRepeatType = RepeatType;

			if (modelStartDate == null)
				throw new ArgumentException("When a habit Filling is being initialized, it cannot have ModelStartDate set to null");

			var newFilling = new ObservableDictionary<DateTime, int>();
			var startDate  = modelStartDate.Value;
			switch (modelRepeatType)
			{
				case RepeatType.Days:
					for (var i = 0; i < DaysToRepeat; i++)
						newFilling.Add(startDate.AddDays(i), 0);
					break;
				case RepeatType.Months:
					for (var i = 0; i < DaysToRepeat; i++)
						newFilling.Add(startDate.AddMonths(i), 0);
					break;
				case RepeatType.Years:
					for (var i = 0; i < DaysToRepeat; i++)
						newFilling.Add(startDate.AddYears(i), 0);
					break;
				default:
					for (var i = 0; i < DaysToRepeat * 7; i++)
					{
						// For every day of a week check if the day is within the ModelRepeatType
						if (modelRepeatType.HasFlag((RepeatType) (1 << startDate.ZeroMondayBasedDay()))) newFilling.Add(startDate, 0);

						startDate = startDate.AddDays(1);
					}

					break;
			}

			Filling = newFilling;
		}
	}
}