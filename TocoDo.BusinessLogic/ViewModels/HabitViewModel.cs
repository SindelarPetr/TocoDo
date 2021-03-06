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
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class HabitViewModel : BaseViewModel, IHabitViewModel
	{
		#region Properties

		public IAsyncCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new AwaitableCommand(DeleteAsync));

		#endregion

		#region Fields

		private readonly IHabitService _habitService;

		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly INavigationService _navigation;
		private readonly HabitScheduleHelper _scheduleHelper;
		private int _daysToRepeat;
		private IAsyncCommand _deleteCommand;
		private string _description;
		private IAsyncCommand _editCommand;
		private ICommand _editTitleCommand;
		private IAsyncCommand<string> _finishCreationCommand;
		private HabitType _habitType;
		private ICommand _increaseTodayCommand;

		private bool _isRecommended;
		private int _maxRepeatsADay;
		private int _repeatsToday;
		private RepeatType _repeatType;
		private DateTime? _startDate;
		private string _title;
		private IAsyncCommand _updateCommand;

		#endregion

		public HabitViewModel(IDateTimeProvider dateTimeProvider, IHabitService habitService, INavigationService navigation)
			: this(dateTimeProvider, habitService)
		{
			_navigation = navigation;

			// Set initial values
			CreationDate    = DateTime.Now;
			_repeatType     = RepeatType.Days;
			_habitType      = HabitType.Daylong;
			_maxRepeatsADay = 1;
			_daysToRepeat   = 21;

			Filling      = new ObservableDictionary<DateTime, int>();
			IsCreateMode = true;
		}

		public HabitViewModel(IDateTimeProvider dateTimeProvider, IHabitService habitService, INavigationService navigation, IHabitModel model)
			: this(dateTimeProvider, habitService)
		{
			_navigation = navigation;

			CreationDate = model.CreationDate;
			Id           = model.Id;
			_repeatType  = model.RepeatType;
			_description = model.Description;

			if (StartDate != null && StartDate <= habitService.DateTimeProvider.Now)
			{
				if (string.IsNullOrWhiteSpace(model.Filling))
					Filling = InitFilling();
				else
					Filling = new ObservableDictionary<DateTime, int>(
						JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(model.Filling));
			}
			else
			{
				Filling = new ObservableDictionary<DateTime, int>();
			}


			_habitType      = model.HabitType;
			_daysToRepeat   = model.DaysToRepeat;
			_startDate      = model.StartDate;
			_title          = model.Title;
			_repeatsToday   = Filling.ContainsKey(DateTime.Today) ? Filling[DateTime.Today] : 0;
			_isRecommended  = model.IsRecommended;
			_maxRepeatsADay = model.RepeatsADay;
		}

		private HabitViewModel(IDateTimeProvider dateTimeProvider, IHabitService habitService)
		{
			_dateTimeProvider = dateTimeProvider;
			_habitService   = habitService;
			_scheduleHelper = new HabitScheduleHelper(_habitService.DateTimeProvider);
		}

		#region Interface

		public int Id { get; private set; }
		/// <summary>
		///     Is used for explicit setting of ModelId, so there is no possibility of setting ModelId accidentally.
		/// </summary>
		/// <param name="id">New id which will be assigned to the ModelId property.</param>
		public void SetModelId(int id)
		{
			Id = id;
		}
		public bool IsActive(DateTime? date = null)
		{
			return _scheduleHelper.IsHabitActive(this, date);
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
			set => SetValue(ref _repeatsToday, value);
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
		public bool IsFinished
		{ get; set; }
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
					//case RepeatType.Months:
					//	timeScale = Resources.Months;
					//	break;
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

		public ICommand EditTitleCommand =>
			_editTitleCommand ?? (_editTitleCommand = new Command(t => EditTitle(t as string)));
		public IAsyncCommand EditCommand => _editCommand ?? (_editCommand = new AwaitableCommand(Edit));
		public IAsyncCommand UpdateCommand => _updateCommand ??
		                                      (_updateCommand =
			                                      new AwaitableCommand(async () => await _habitService.UpdateAsync(this)));
		public IAsyncCommand<string> FinishCreationCommand => _finishCreationCommand ??
		                                                      (_finishCreationCommand =
			                                                      new AwaitableCommand<string>(FinishCreationAsync));
		public ICommand IncreaseTodayCommand =>
			_increaseTodayCommand ?? (_increaseTodayCommand = new Command(() => RepeatsToday++));
		#endregion

		#region  Private and protected

		private async Task Edit()
		{
			if (IsStarted)
				await _navigation.PushAsync(PageType.HabitProgressPage, this);
			else
				await _navigation.PushAsync(PageType.ModifyHabitPage, this);
		}

		private void EditTitle(string newTitle)
		{
			if (string.IsNullOrWhiteSpace(newTitle))
				return;

			newTitle = newTitle.Trim();
			Title = newTitle;
		}

		private async Task FinishCreationAsync(object title)
		{
			// If user left the entry blank, then remove the habit from collection
			if (!(title is string tit) || string.IsNullOrWhiteSpace(tit))
			{
				_habitService.CancelCreation(this);
				return;
			}

			IsCreateMode = false;
			Title        = tit.Trim();
			await _habitService.ConfirmCreationAsync(this);
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			MyLogger.WriteStartMethod();
			base.OnPropertyChanged(propertyName);

			// Init filling if needed and update it
			if (propertyName == nameof(RepeatsToday))
			{
				if (Filling == null || !Filling.Any()) InitFilling();

				var today   = DateTime.Today;
				var filling = Filling;
				if (filling.ContainsKey(today))
					filling[today] = RepeatsToday;
			}

			MyLogger.WriteEndMethod();
		}

		/// <summary>
		///     Fills all dates when the habit will (or supposed to) be performed
		/// </summary>
		private ObservableDictionary<DateTime, int> InitFilling()
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
				//case RepeatType.Months:
				//	for (var i = 0; i < DaysToRepeat; i++)
				//		newFilling.Add(startDate.AddMonths(i), 0);
				//	break;
				case RepeatType.Years:
					for (var i = 0; i < DaysToRepeat; i++)
						newFilling.Add(startDate.AddYears(i), 0);
					break;
				default:
					for (var i = 0; i < DaysToRepeat * 7; i++)
					{
						// For every day of a week check if the day is within the ModelRepeatType
						if (modelRepeatType.HasFlag((RepeatType) (1 << startDate.ZeroMondayBasedDay())))
							newFilling.Add(startDate, 0);

						startDate = startDate.AddDays(1);
					}

					break;
			}

			return newFilling;
		}

		private async Task DeleteAsync()
		{
			// Ask user if he is sure
			var userIsSure = await _navigation.DisplayAlert(Resources.DeleteHabitConfirmHeader, Resources.DeleteHabitConfirmText,
			                                                Resources.Yes, Resources.Cancel);

			if (!userIsSure)
				return;

			await _habitService.DeleteAsync(this);
			await _navigation.PopAsync();
		}

		#endregion
	}
}