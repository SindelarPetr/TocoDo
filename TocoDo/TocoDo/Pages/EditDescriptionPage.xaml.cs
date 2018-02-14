using System;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditDescriptionPage : ContentPage
	{
		public EditDescriptionViewModel ViewModel { get; set; }

		public EditDescriptionPage(string title, string description, Action<string> setDescriptionAction, bool isReadonly = false)
		{
			ViewModel = new EditDescriptionViewModel(((App)App.Current).Navigation, title, description, setDescriptionAction, isReadonly);
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(ViewModel.Description) && !ViewModel.IsReadonly)
				EditorNote.Focus();
		}

		protected override bool OnBackButtonPressed()
		{
			ViewModel.DiscardCommand?.Execute(null);
			return true;
		}
	}
}