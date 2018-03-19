using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class PastHabitsViewModel : BaseViewModel
	{
		private readonly IHabitService _habitService;
		private readonly INavigationService _navigationService;
		private bool _isLoading;
		public ReadOnlyObservableCollection<IHabitViewModel> PastHabits { get; set; }
		private readonly ObservableCollection<IHabitViewModel> _pastHabits;
		private bool _loadOnce;

		public bool IsLoading
		{
			get => _isLoading;
			set => SetValue(ref _isLoading, value);
		}

		public IAsyncCommand LoadPastHabitsCommand => _loadPastHabits ?? (_loadPastHabits = new AwaitableCommand(LoadPastHabits));
		private IAsyncCommand _loadPastHabits;

		public PastHabitsViewModel(IHabitService habitService, INavigationService navigationService)
		{
			_habitService = habitService;
			_navigationService = navigationService;
			_isLoading = true;
			_pastHabits = new ObservableCollection<IHabitViewModel>();
			PastHabits = new ReadOnlyObservableCollection<IHabitViewModel>(_pastHabits);
		}

		private async Task LoadPastHabits()
		{
			if(_loadOnce)
				return;
			_loadOnce = true;

			var habits = await _habitService.LoadPastHabitsAsync();
			habits.ForEach(h => _pastHabits.Add(h));
			IsLoading = false;
		}
	}
}
