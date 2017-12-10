using System.Windows.Input;
using TocoDo.Resources;
using TocoDo.Services;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
	public class EditDescriptionViewModel : BaseViewModel
	{
		private readonly TaskViewModel _taskViewModel;
		public string Title { get; set; }
		public string Description { get; set; }

		public ICommand DiscardCommand { get; set; }
		public ICommand SaveCommand { get; set; }

		public EditDescriptionViewModel(TaskViewModel taskViewModel)
		{
			_taskViewModel = taskViewModel;
			Title = taskViewModel.Title;
			Description = taskViewModel.Description;
			DiscardCommand = new Command(Discard);
			SaveCommand = new Command(() => Save(_taskViewModel));
		}

		private async void Discard()
		{
			if (Description != _taskViewModel.Description)
			{
				var result = await PageService.DisplayAlert(AppResource.Warning, AppResource.AreYouSureDescardChanges,
					AppResource.Yes, AppResource.No);

				if (!result) return;
			}

			await PageService.PopModalAsync();
		}

		private async void Save(TaskViewModel task)
		{
			task.Description = Description.Trim();
			StorageService.UpdateTask(task);
			await PageService.PopModalAsync();
		}
	}
}
