using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Services;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class TasksPastViewModel : BaseViewModel
	{
		#region Properties

		public IAsyncCommand LoadPastTasksCommand =>
			_loadPastTasksCommand ?? (_loadPastTasksCommand = new AwaitableCommand(LoadPastTasks));

		public ReadOnlyObservableCollection<ITaskViewModel> PastTasks { get; set; }

		#endregion

		#region Fields

		private readonly INavigationService _navigationService;
		private readonly ITaskService _taskService;


		private IAsyncCommand _loadPastTasksCommand;

		private readonly ObservableCollection<ITaskViewModel> _pastTasks;
		private bool loadOnce = false;

		#endregion

		public TasksPastViewModel(INavigationService navigationService, ITaskService taskService)
		{
			_navigationService = navigationService;
			_taskService       = taskService;
			_pastTasks         = new ObservableCollection<ITaskViewModel>();
			PastTasks          = new ReadOnlyObservableCollection<ITaskViewModel>(_pastTasks);
		}

		#region  private and protected

		private async Task LoadPastTasks()
		{
			if(loadOnce)
				return;
			loadOnce = true;

			var tasks = await _taskService.LoadPastTasksAsync();
			tasks.ForEach(_pastTasks.Add);
		}

		#endregion
	}
}