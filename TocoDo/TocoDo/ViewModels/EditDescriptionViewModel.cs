using System;
using System.Windows.Input;
using TocoDo.Helpers;
using TocoDo.Properties;
using TocoDo.Services;

namespace TocoDo.ViewModels
{
	public class EditDescriptionViewModel : BaseViewModel
	{
		private readonly Action<string> _setDescriptionAction;
		public string Title { get; set; }
		public bool IsReadonly { get; }
		private readonly string _originalDescription;
		public string Description { get; set; }

		public ICommand DiscardCommand { get; set; }
		public ICommand SaveCommand { get; set; }

		public EditDescriptionViewModel(string title, string description, Action<string> setDescriptionAction,
			bool isReadonly)
		{
			_setDescriptionAction = setDescriptionAction;
			Title = title;
			IsReadonly = isReadonly;
			_originalDescription = Description = description;
			DiscardCommand = new MyCommand(Discard);
			SaveCommand = new MyCommand(Save);
		}

		private async void Discard()
		{
			if (Description != _originalDescription)
			{
				var result = await PageService.DisplayAlert(Resources.Warning, Resources.AreYouSureDescardChanges,
					Resources.Yes, Resources.No);

				if (!result) return;
			}

			await PageService.PopModalAsync();
		}

		private async void Save()
		{
			_setDescriptionAction?.Invoke(Description.Trim());

			await PageService.PopModalAsync();
		}
	}
}
