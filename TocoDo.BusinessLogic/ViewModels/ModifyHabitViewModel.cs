using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
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

		public IAsyncCommand WriteAndUpdateCommand
		{
			get
			{
				return _writeAndUpdateCommand ?? (_writeAndUpdateCommand =
						   new AwaitableCommand(async () => { await WriteAndUpdateAsync(); }));
			}
		}

		public IAsyncCommand ModifyRepeatTypeCommand => _modifyRepeatTypeCommand ??
														(_modifyRepeatTypeCommand = new AwaitableCommand(async () => await ModifyRepeatType()));

		public ICommand ChangeHabitTypeCommand => _changeHabitTypeCommand ??
		                                          (_changeHabitTypeCommand =
			                                          new Command(t => HabitType = (HabitType)t));

		public ICommand EditTitleCommand =>
			_editTitleCommand ?? (_editTitleCommand = new Command(t => EditTitle((string) t)));

		public IAsyncCommand DeleteCommand =>
			_deleteCommand ?? (_deleteCommand = new AwaitableCommand(async () => await DeleteAsync()));
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
		private IAsyncCommand _deleteCommand;
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

		private async Task ModifyRepeatType()
		{
			await _navigation.PushPopupAsync(PageType.RepeatTypePopup, new ModifyRepeatTypeViewModel(_navigation, this));
		}

		private void EditTitle(string title)
		{
			if(title == null)return;

			if (!string.IsNullOrWhiteSpace(title))
			{
				Title = title.Trim();
			}
		}

		private async Task DeleteAsync()
		{
			// Ask user if he is sure
			var userIsSure = await _navigation.DisplayAlert(Resources.DeleteHabitConfirmHeader, Resources.DeleteHabitConfirmText,
			                                            Resources.Yes, Resources.Cancel);

			if (!userIsSure)
				return;

			await _habitService.DeleteAsync(_habit);
			await _navigation.PopAsync();
		}
	}
}