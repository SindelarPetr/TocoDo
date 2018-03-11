using System;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers.Commands;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class ModifyHabitViewModel : BaseViewModel
	{
		#region Properties

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

		public HabitType HabitType
		{
			get => _habitType;
			set => SetValue(ref _habitType, value);
		}

		public string Description
		{
			get => _description;
			set => SetValue(ref _description, value);
		}

		public IAsyncCommand WriteAndSaveCommand
		{
			get
			{
				return _writeAndSaveCommand ?? (_writeAndSaveCommand =
					       new AwaitableCommand(async () => { await _habit.UpdateCommand.ExecuteAsync(null); }));
			}
		}

		#endregion

		#region Fields

		private readonly HabitViewModel _habit;
		private string _description;
		private HabitType _habitType;
		private RepeatType _repeatType;
		private DateTime? _startDate;

		private IAsyncCommand _writeAndSaveCommand;

		#endregion

		public ModifyHabitViewModel(HabitViewModel habit)
		{
			_habit       = habit;
			_description = habit.Description;
		}
	}
}