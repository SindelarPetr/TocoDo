using System;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditDescriptionPage : ContentPage
	{
		public EditDescriptionPage(EditDescriptionInfo info)
		{
			ViewModel = new EditDescriptionViewModel(((App) Application.Current).Navigation, info);
			InitializeComponent();
		}

		public EditDescriptionViewModel ViewModel { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(ViewModel.Description) && !ViewModel.IsReadonly)
				EditorNote.Focus();
		}

		protected override bool OnBackButtonPressed()
		{
			ViewModel.DiscardCommand.ExecuteAsync(null).GetAwaiter().GetResult();
			return true;
		}
	}
}