using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Helpers.Commands;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class EditDescriptionViewModel : BaseViewModel
	{
		private readonly INavigationService _navigation;
		private readonly string             _originalDescription;
		private readonly Action<string>     _setDescriptionAction;

		public EditDescriptionViewModel(INavigationService navigation, EditDescriptionInfo info)
		{
			_navigation           = navigation;
			Title                 = info.Title;
			_originalDescription = Description = info.Description;
			_setDescriptionAction = info.DescriptionSetter;
			IsReadonly            = info.IsReadonly;
			DiscardCommand        = new AwaitableCommand(async () => await Discard());
			SaveCommand           = new AwaitableCommand(async () => await Save());
		}

		public string Title       { get; set; }
		public bool   IsReadonly  { get; }
		public string Description { get; set; }

		public IAsyncCommand DiscardCommand { get; set; }
		public IAsyncCommand SaveCommand    { get; set; }

		private async Task Discard()
		{
			if (Description != _originalDescription)
			{
				var result = await _navigation.DisplayAlert(Resources.Warning, Resources.AreYouSureDescardChanges,
					Resources.Yes, Resources.No);

				if (!result) return;
			}

			await _navigation.PopModalAsync();
		}

		private async Task Save()
		{
			if(!IsReadonly)
				_setDescriptionAction?.Invoke(Description.Trim());

			await _navigation.PopModalAsync();
		}
	}
}