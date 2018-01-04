
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

		private EditDescriptionViewModel Vm
		{
			get => (EditDescriptionViewModel)BindingContext;
			set => BindingContext = value;
		}

		public EditDescriptionPage(string title, string description, Action<string> setDescriptionAction)
		{
			Debug.Write("----------------- EditDescriptionPage constructor called.");

			Debug.Write("-------------------- Before InitializeComponent.");
			InitializeComponent();
			Debug.Write("-------------------- After InitializeComponent.");

			Vm = new EditDescriptionViewModel(title, description, setDescriptionAction);
			Debug.Write("----------------- Finished calling EditDescriptionPage constructor.");
		}

		protected override void OnAppearing()
		{
			Debug.Write("----------------- OnAppearing of EditDescriptionPage called.");
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(Vm.Description))
				EditorNote.Focus();
			Debug.Write("----------------- Finished calling of OnAppearing of EditDescriptionPage.");
		}

		protected override bool OnBackButtonPressed()
		{
			Vm.DiscardCommand?.Execute(null);
			return true;
		}
	}
}