
using System;
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditDescriptionPage : ContentPage
	{
		public EditDescriptionViewModel ViewModel { get; set; }

		public EditDescriptionPage(string title, string description, Action<string> setDescriptionAction, bool isReadonly = false)
		{
			ViewModel = new EditDescriptionViewModel(title, description, setDescriptionAction, isReadonly);
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			Debug.Write("----------------- OnAppearing of EditDescriptionPage called.");
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(ViewModel.Description) && !ViewModel.IsReadonly)
				EditorNote.Focus();
			Debug.Write("----------------- Finished calling of OnAppearing of EditDescriptionPage.");
		}

		protected override bool OnBackButtonPressed()
		{
			ViewModel.DiscardCommand?.Execute(null);
			return true;
		}
	}
}