using System;
using System.Windows.Input;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class EditDescriptionViewModel : BaseViewModel
	{
		private readonly INavigationService _navigation;
		private readonly string             _originalDescription;
		private readonly Action<string>     _setDescriptionAction;

		public EditDescriptionViewModel(INavigationService navigation, string         title, string description,
			Action<string>                                    setDescriptionAction, bool isReadonly)
		{
			_navigation           = navigation;
			_setDescriptionAction = setDescriptionAction;
			Title                 = title;
			IsReadonly            = isReadonly;
			_originalDescription  = Description = description;
			DiscardCommand        = new Command(Discard);
			SaveCommand           = new Command(Save);
		}

		public string Title       { get; set; }
		public bool   IsReadonly  { get; }
		public string Description { get; set; }

		public ICommand DiscardCommand { get; set; }
		public ICommand SaveCommand    { get; set; }

		private async void Discard()
		{
			if (Description != _originalDescription)
			{
				var result = await _navigation.DisplayAlert(Resources.Warning, Resources.AreYouSureDescardChanges,
					Resources.Yes, Resources.No);

				if (!result) return;
			}

			await _navigation.PopModalAsync();
		}

		private async void Save()
		{
			_setDescriptionAction?.Invoke(Description.Trim());

			await _navigation.PopModalAsync();
		}
	}
}