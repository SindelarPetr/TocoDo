using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers.Commands;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class MasterViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;


		private IAsyncCommand _openHabitsPastCommand;
		public IAsyncCommand OpenHabitsPastCommand =>
			_openHabitsPastCommand ?? (_openHabitsPastCommand = new AwaitableCommand(OpenHabitsPast));
		private IAsyncCommand _openTasksPastCommand;
		public IAsyncCommand OpenTasksPastCommand =>
			_openTasksPastCommand ?? (_openTasksPastCommand = new AwaitableCommand(OpenTasksPast));

		public MasterViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		private async Task OpenHabitsPast()
		{
			await _navigationService.PushAsync(PageType.HabitsPastPage);
		}
		private async Task OpenTasksPast()
		{
			await _navigationService.PushAsync(PageType.TasksPastPage);
		}
	}
}