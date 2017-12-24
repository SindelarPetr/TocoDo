
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
			_taskViewModel = taskViewModel;
			InitializeComponent();

			Vm = new EditDescriptionViewModel(taskViewModel);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// In case there is no description focus the Edit
			if (string.IsNullOrWhiteSpace(_taskViewModel.Description))
				EditorNote.Focus();
		}

		protected override bool OnBackButtonPressed()
		{
			Vm.DiscardCommand?.Execute(null);
			return true;
		}
	}
}