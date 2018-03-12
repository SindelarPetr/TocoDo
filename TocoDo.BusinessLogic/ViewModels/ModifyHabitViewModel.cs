using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Properties;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class ModifyHabitViewModel : BaseViewModel
	{
		#region Properties

		public string Title
		{
			get => _title;
			set => SetValue(ref _title, value);
		}

		public DateTime? StartDate
		{
			get => _startDate;
			set => SetValue(ref _startDate, value);
		}

		public RepeatType RepeatType
		{
			get => _repeatType;
			set => SetValue(ref _repeatType, value);
		}
		public int DaysToRepeat
		{
			get => _daysToRepeat;
			set => SetValue(ref _daysToRepeat, value);
		}

		public HabitType HabitType
		{
			get => _habitType;
			set => SetValue(ref _habitType, value);
		}

		public int MaxRepeatsADay
		{
			get => _maxRepeatsADay;
			set => SetValue(ref _maxRepeatsADay, value);
		}

		public string Description
		{
			get => _description;
			set => SetValue(ref _description, value);
		}
		public DateTime CreationDate { get; }

		public IAsyncCommand WriteAndUpdateCommand => _writeAndUpdateCommand ?? (_writeAndUpdateCommand =
			                                              new AwaitableCommand(WriteAndUpdateAsync));

		public IAsyncCommand ModifyRepeatTypeCommand => _modifyRepeatTypeCommand ??
														(_modifyRepeatTypeCommand = new AwaitableCommand(async () => await ModifyRepeatType()));

		public ICommand ChangeHabitTypeCommand => _changeHabitTypeCommand ??
		                                          (_changeHabitTypeCommand =
			                                          new Command(t => HabitType = (HabitType)t));

		public ICommand EditTitleCommand =>
			_editTitleCommand ?? (_editTitleCommand = new Command(t => EditTitle((string) t)));

		public IAsyncCommand DeleteCommand => _habit.DeleteCommand;
		#endregion

		#region Fields

		private readonly IHabitService _habitService;
		private readonly INavigationService _navigation;
		private readonly HabitViewModel _habit;
		private string _description;
		private HabitType _habitType;
		private RepeatType _repeatType;
		private int _daysToRepeat;
		private DateTime? _startDate;

		private IAsyncCommand _writeAndUpdateCommand;
		private IAsyncCommand _modifyRepeatTypeCommand;
		private ICommand _changeHabitTypeCommand;
		private ICommand _editTitleCommand;
		private string _title;
		private int _maxRepeatsADay;

		#endregion

		public ModifyHabitViewModel(IHabitService habitService, INavigationService navigation, HabitViewModel habit)
		{
			_habitService = habitService;
			_navigation = navigation;
			_habit = habit;

			CreationDate = _habit.CreationDate;
			_title = _habit.Title;
			_description = _habit.Description;
			_habitType = _habit.HabitType;
			_maxRepeatsADay = _habit.MaxRepeatsADay;
			_repeatType = _habit.RepeatType;
			_daysToRepeat = _habit.DaysToRepeat;
			_startDate = _habit.StartDate;

		}

		private async Task WriteAndUpdateAsync()
		{
			// if there is no change -> finish
			if (_habit.Title == _title && _habit.Description == _description && _habit.HabitType == _habitType &&
			    _habit.MaxRepeatsADay == _maxRepeatsADay && _habit.RepeatType == _repeatType &&
			    _habit.DaysToRepeat == _daysToRepeat && _habit.StartDate == _startDate)
			{
				return;
			}

			// Write changes
			_habit.EditTitleCommand.Execute(_title);
			_habit.Description = _description;
			_habit.HabitType = _habitType;
			_habit.MaxRepeatsADay = _maxRepeatsADay;
			_habit.RepeatType = _repeatType;
			_habit.DaysToRepeat = _daysToRepeat;
			_habit.StartDate = _startDate;

			await _habit.UpdateCommand.ExecuteAsync(null);
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if ((propertyName == nameof(RepeatType) && StartDate != null) || propertyName == nameof(StartDate))
			{
				// Synchronise StartDate with selected RepeatType
				// Makes sense just when the RepeatType are days in week
				if (RepeatType < RepeatType.CompleteWeek)
				{
					StartDate = RepeatTypeHelper.AdjustDateToRepeatType(StartDate, RepeatType);
				}
			}
		}

		private async Task ModifyRepeatType()
		{
			var modifyRepeatTypeViewModel = new ModifyRepeatTypeViewModel(_navigation, this);
			await _navigation.PushPopupAsync(PageType.RepeatTypePopup, modifyRepeatTypeViewModel);
		}

		private void EditTitle(string title)
		{
			if(title == null)return;

			if (!string.IsNullOrWhiteSpace(title))
			{
				Title = title.Trim();
			}
		}
	}
}