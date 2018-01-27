using System;
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
using TocoDo.Views.Habits;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
	public class HabitViewModel : BaseViewModel, ICreateMode
	{

		#region Backing fields
		private bool _modelIsRecommended;
		private int _modelRepeatsToday;
		private HabitType _modelHabitType;
		private int _modelDaysToRepeat;
		private DateTime? _modelStartDate;
		private string _modelTitle;
		private RepeatType _modelRepeatType;
		private string _modelDescription;
		private int _modelRepeatsADay;
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
				SetValue(ref _modelRepeatsToday, value);
			}
		}

		public ObservableDictionary<DateTime, int> ModelFilling { get; }

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
			get => _modelRepeatsADay;
			set
			{
				SetValue(ref _modelRepeatsADay, value);
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
		public ICommand EditCommand { get; private set; }
		public ICommand UpdateCommand { get; private set; }
		public ICommand InsertCommand { get; private set; }
		public ICommand SelectStartDateCommand { get; private set; }
		public ICommand UnsetStartDateCommand { get; private set; }
		public ICommand SelectRepeatCommand { get; private set; }
		public ICommand IncreaseTodayCommand { get; private set; }
		#endregion

		public HabitViewModel()
		{
			// Set initial values
			ModelRepeatType = RepeatType.Days;
			ModelHabitType = HabitType.Daylong;
			ModelMaxRepeatsADay = 1;
			ModelDaysToRepeat = 21;

			ModelFilling = new ObservableDictionary<DateTime, int>();
			IsCreateMode = true;

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
			_modelRepeatsToday = model.RepeatsToday;
			_modelIsRecommended = model.IsRecommended;
			_modelRepeatsADay = model.RepeatsADay;

			SetupCommands();
		}

		private void SetupCommands()
		{
			InsertCommand = new Command<string>(async s => await InsertToStorage(s));
			EditCommand = new Command(async () => await PageService.PushAsync(IsStarted? (Page)new HabitProgressPage(this) : (Page)new ModifyHabitPage(this)));

			SelectStartDateCommand = new Command(async () => await SelectDate(d => ModelStartDate = d, Resources.SelectStartDate));
			UnsetStartDateCommand = new Command(() => ModelStartDate = null);
			IncreaseTodayCommand = new Command(() => ModelRepeatsToday++);
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

		// TODO: get rid of this
		private void SelectDateByPicker(Action<DateTime> pickedAction)
		{
			TodayPage.Instance.ShowGlobalDatePicker(ModelStartDate ?? DateTime.Today + TimeSpan.FromDays(1), pickedAction, DateTime.Today + TimeSpan.FromDays(1));
		}

		public async Task InsertToStorage(string title)
		{
			_modelTitle = title;
			OnPropertyChanged(nameof(ModelTitle));
			IsCreateMode = false;
			await StorageService.InsertHabit(this);
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
				RepeatsToday = ModelRepeatsToday,
				IsRecommended = ModelIsRecommended,
				RepeatsADay = ModelMaxRepeatsADay
			};
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			MyLogger.WriteStartMethod();
			base.OnPropertyChanged(propertyName);

			if(!IsCreateMode)
			StorageService.UpdateHabit(this);
			MyLogger.WriteEndMethod();
		}
	}
}