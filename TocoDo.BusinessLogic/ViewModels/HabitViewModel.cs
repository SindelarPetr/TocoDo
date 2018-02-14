using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Extensions;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class HabitViewModel : BaseViewModel, ICreateMode
	{
		private readonly IStorageService _storage;
		private readonly INavigationService _navigation;

		#region Backing fields

		private bool _modelIsRecommended;
		private int _modelRepeatsToday;
		private HabitType _modelHabitType;
		private int _modelDaysToRepeat;
		private DateTime? _modelStartDate;
		private string _modelTitle;
		private RepeatType _modelRepeatType;
		private string _modelDescription;
		private int _modelMaxRepeatsADay;

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
		public int ModelRepeatsToday
		{
			get => _modelRepeatsToday;
			set
			{
				if (DateTimeHelper.IsHabitToday(ModelStartDate, ModelRepeatType, ModelDaysToRepeat))
					SetValue(ref _modelRepeatsToday, value);
			}
		}

		public ObservableDictionary<DateTime, int> ModelFilling { get; private set; }

		public DateTime ModelCreationDate { get; }

		public HabitType ModelHabitType
		{
			get => _modelHabitType;
			set
			{
				SetValue(ref _modelHabitType, value);
				OnPropertyChanged(nameof(HabitTypeWithRepeats));
			}
		}

		public int ModelDaysToRepeat
		{
			get => _modelDaysToRepeat;
			set
			{
				SetValue(ref _modelDaysToRepeat, value);
				OnPropertyChanged(nameof(HabitDaysToRepeatWithRepeatType));
			}
		}

		public DateTime? ModelStartDate
		{
			get => _modelStartDate;
			set => SetValue(ref _modelStartDate, value);
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
				OnPropertyChanged(nameof(HabitDaysToRepeatWithRepeatType));
			}
		}

		public string ModelDescription
		{
			get => _modelDescription;
			set => SetValue(ref _modelDescription, value);
		}

		/// <summary>
		/// Only for Unit habit. Count of times to repeat the habit each day
		/// </summary>
		public int ModelMaxRepeatsADay
		{
			get => _modelMaxRepeatsADay;
			set
			{
				SetValue(ref _modelMaxRepeatsADay, value);
				OnPropertyChanged(nameof(HabitTypeWithRepeats));
			}
		}

		public bool IsCreateMode { get; set; }

		public bool IsStarted => ModelStartDate != null && ModelStartDate.Value < DateTime.Now;

		public bool ModelIsFinished { get; set; }

		public string HabitTypeWithRepeats => (ModelHabitType == HabitType.Daylong)
			? Resources.HabitDetailHabitTypeDaylong
			: string.Format(Resources.HabitDetailHabitTypeTextUnit, ModelMaxRepeatsADay);

		public string HabitDaysToRepeatWithRepeatType
		{
			get
			{
				string timeScale;
				switch (ModelRepeatType)
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

				return $"{Resources.For} {ModelDaysToRepeat} {timeScale}";
			}
		}

		#endregion

		#region Commands

		public ICommand EditTitleCommand { get; private set; }
		public ICommand EditCommand { get; private set; }
		public ICommand UpdateCommand { get; private set; }
		public ICommand ConfirmCreationCommand { get; private set; }
		public ICommand SelectStartDateCommand { get; private set; }
		public ICommand UnsetStartDateCommand { get; private set; }
		public ICommand SelectRepeatCommand { get; private set; }
		public ICommand IncreaseTodayCommand { get; private set; }
		public ICommand RemoveCommand { get; private set; }

		#endregion

		public HabitViewModel(IStorageService storageService, INavigationService navigation)
		{
			_storage = storageService;
			_navigation = navigation;

			// Set initial values
			ModelCreationDate = DateTime.Now;
			_modelRepeatType = RepeatType.Days;
			_modelHabitType = HabitType.Daylong;
			_modelMaxRepeatsADay = 1;
			_modelDaysToRepeat = 21;

			ModelFilling = new ObservableDictionary<DateTime, int>();
			IsCreateMode = true;

			SetupCommands();
		}

		public HabitViewModel(IStorageService storageService, INavigationService navigation, IHabitModel model)
		{
			_storage = storageService;
			_navigation = navigation;
			ModelCreationDate = model.CreationDate;
			ModelId = model.Id;
			_modelRepeatType = model.RepeatType;
			_modelDescription = model.Description;
			ModelFilling =
				new ObservableDictionary<DateTime, int>(
					Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<DateTime, int>>(model.Filling));
			_modelHabitType = model.HabitType;
			_modelDaysToRepeat = model.DaysToRepeat;
			_modelStartDate = model.StartDate;
			_modelTitle = model.Title;
			_modelRepeatsToday = ModelFilling.ContainsKey(DateTime.Today) ? ModelFilling[DateTime.Today] : 0;
			_modelIsRecommended = model.IsRecommended;
			_modelMaxRepeatsADay = model.RepeatsADay;

			SetupCommands();
		}

		private void SetupCommands()
		{
			EditTitleCommand = new Command<string>(EditTitle);
			ConfirmCreationCommand = new Command(async () => await ConfirmCreation());
			EditCommand = new Command(async () => await Edit());

			SelectStartDateCommand =
				new Command(async () => await SelectDate(d => ModelStartDate = d, Resources.SelectStartDate));
			UnsetStartDateCommand = new Command(() => ModelStartDate = null);
			IncreaseTodayCommand = new Command(() => ModelRepeatsToday++);
			RemoveCommand = new Command(async () => await Delete());
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

			ModelTitle = newTitle;
		}

		private async Task Delete()
		{
			// Ask user if he is sure
			var result = await _navigation.DisplayAlert(Resources.DeleteHabitConfirmHeader, Resources.DeleteHabitConfirmText,
				Resources.Yes, Resources.Cancel);

			if (result)
				await _storage.DeleteHabit(this);
		}

		public async Task ConfirmCreation()
		{
			// If user left the entry blank, then remove the habit from collection
			if (string.IsNullOrWhiteSpace(ModelTitle))
			{
				_storage.CancelCreationOfHabit(this);
				return;
			}

			await _storage.ConfirmCreationOfHabit(this);
		}

		private async Task SelectDate(Action<DateTime?> pickedAction, string actionSheetHeader)
		{
			string[] buttons = {Resources.Today, Resources.Tomorrow, Resources.TheDayAfterTomorrow, Resources.PickADate};

			string result = await _navigation.DisplayActionSheet(actionSheetHeader, Resources.Cancel, null, buttons);

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

		// TODO: get rid of this
		private void SelectDateByPicker(Action<DateTime> pickedAction)
		{
			//TODO: Workaround for showing the date picker TodayPage.Instance.ShowGlobalDatePicker(ModelStartDate ?? DateTime.Today + TimeSpan.FromDays(1), pickedAction, DateTime.Today + TimeSpan.FromDays(1));
		}

		protected override async void OnPropertyChanged(string propertyName = null)
		{
			MyLogger.WriteStartMethod();
			base.OnPropertyChanged(propertyName);

			// Init filling if needed and update it
			if (propertyName == nameof(ModelRepeatsToday))
			{
				InitFilling();
				if (ModelFilling == null || !ModelFilling.Any())
				{
					InitFilling();
				}

				var today = DateTime.Today;
				var filling = ModelFilling;
				if (filling.ContainsKey(today))
					filling[today] = ModelRepeatsToday;
			}

			if (!IsCreateMode)
			{
				await _storage.UpdateHabit(this);
				MyLogger.WriteInMethod($"Updated habit, changed property: {propertyName}");
			}

			MyLogger.WriteEndMethod();
		}

		/// <summary>
		/// Fills all dates when the habit will (or supposed to) be performed
		/// </summary>
		private void InitFilling()
		{
			var modelStartDate = ModelStartDate;
			var modelRepeatType = ModelRepeatType;

			if (modelStartDate == null)
				throw new ArgumentException("When a habit Filling is being initialized, it cannot have ModelStartDate set to null");

			var newFilling = new ObservableDictionary<DateTime, int>();
			var startDate = modelStartDate.Value;
			switch (modelRepeatType)
			{
				case RepeatType.Days:
					for (int i = 0; i < ModelDaysToRepeat; i++)
						newFilling.Add(startDate.AddDays(i), 0);
					break;
				case RepeatType.Months:
					for (int i = 0; i < ModelDaysToRepeat; i++)
						newFilling.Add(startDate.AddMonths(i), 0);
					break;
				case RepeatType.Years:
					for (int i = 0; i < ModelDaysToRepeat; i++)
						newFilling.Add(startDate.AddYears(i), 0);
					break;
				default:
					for (int i = 0; i < ModelDaysToRepeat * 7; i++)
					{
						// For every day of a week check if the day is within the ModelRepeatType
						if (modelRepeatType.HasFlag((RepeatType) (1 << startDate.ZeroMondayBasedDay())))
						{
							newFilling.Add(startDate, 0);
						}

						startDate = startDate.AddDays(1);
					}

					break;
			}

			ModelFilling = newFilling;
		}
	}
}