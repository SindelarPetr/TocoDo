using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class ModifyRepeatTypeViewModel : BaseViewModel
	{
		#region Properties

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

		public List<string> PickValues { get; }

		public string SelectedItem
		{
			get => _selectedItem;
			set => SetValue(ref _selectedItem, value);
		}

		public ICommand SelectDayCommand
			=> _selectDayCommand ?? (_selectDayCommand = new Command(o => SelectDay((RepeatType) o)));

		public IAsyncCommand ApplyAndPopCommand => _applyAndPopCommand ??
		                                           (_applyAndPopCommand =
			                                           new AwaitableCommand(ApplyAndPop));

		public IAsyncCommand CancelCommand =>
			_cancelCommand ?? (_cancelCommand = new AwaitableCommand(async () => await Cancel()));

		#endregion

		#region Fields

		private readonly ModifyHabitViewModel _modifyVm;
		private readonly INavigationService _navigation;

		private IAsyncCommand _applyAndPopCommand;
		private IAsyncCommand _cancelCommand;

		private int _daysToRepeat;
		private RepeatType _lastRepeatTypeWeeks;
		private RepeatType _repeatType;
		private ICommand _selectDayCommand;
		private string _selectedItem;

		#endregion

		public ModifyRepeatTypeViewModel(INavigationService navigation, ModifyHabitViewModel modifyVm)
		{
			_navigation = navigation;
			_modifyVm   = modifyVm;

			RepeatType = _modifyVm.RepeatType;
			_daysToRepeat = _modifyVm.DaysToRepeat;

			PickValues = new List<string>
			             {
				             Resources.Days,
				             Resources.Weeks,
				             Resources.Years
			             };

			_lastRepeatTypeWeeks = _repeatType < RepeatType.Days ? _repeatType : RepeatType.WorkWeek;
			SelectedItem = _repeatType == RepeatType.Days ? Resources.Days :
							_repeatType == RepeatType.Years              ? Resources.Years : Resources.Weeks;
		}

		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);

			if (propertyName == nameof(RepeatType))
				if (RepeatType < RepeatType.Days)
					_lastRepeatTypeWeeks = RepeatType;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == nameof(SelectedItem))
				if (SelectedItem == Resources.Weeks)
					RepeatType = _lastRepeatTypeWeeks;
				else if (SelectedItem == Resources.Years)
					RepeatType = RepeatType.Years;
				else if (SelectedItem == Resources.Days)
					RepeatType = RepeatType.Days;
		}

		private void SelectDay(RepeatType day)
		{
			if (RepeatType.HasFlag(day))
				RepeatType &= ~day;
			else
				RepeatType |= day;
		}

		private async Task ApplyAndPop()
		{
			// Apply
			_modifyVm.DaysToRepeat = DaysToRepeat;
			_modifyVm.RepeatType   = RepeatType;

			// Pop
			await _navigation.PopPopupAsync();
		}

		private async Task Cancel()
		{
			await _navigation.PopPopupAsync();
		}
	}
}