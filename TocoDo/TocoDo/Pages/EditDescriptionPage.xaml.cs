
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditDescriptionPage : ContentPage
	{
		private readonly TaskViewModel _taskViewModel;

		private EditDescriptionViewModel Vm
		{
			get => (EditDescriptionViewModel)BindingContext;
			set => BindingContext = value;
		}

		public EditDescriptionPage(TaskViewModel taskViewModel)
		{
			Debug.Write("----------------- EditDescriptionPage constructor called.");
			_taskViewModel = taskViewModel;

			Debug.Write("-------------------- Before InitializeComponent.");

			InitializeComponent();
			Debug.Write("-------------------- After InitializeComponent.");

			Vm = new EditDescriptionViewModel(taskViewModel);
			Debug.Write("----------------- Finished calling EditDescriptionPage constructor.");
		}

		protected override void OnAppearing()
		{
			Debug.Write("----------------- OnAppearing of EditDescriptionPage called.");
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(_taskViewModel.Description))
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