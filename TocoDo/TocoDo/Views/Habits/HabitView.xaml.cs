using System.Threading.Tasks;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitView : ContentView, IEntryFocusable<IHabitViewModel>
	{
		public HabitView(IHabitViewModel habit)
		{
			ViewModel = habit;
			InitializeComponent();
		}

		public IHabitViewModel ViewModel
		{
			get => (IHabitViewModel) BindingContext;
			set => BindingContext = value;
		}

		public void FocusEntry()
		{
			EntryEditTitle.Focus();
		}

		private async Task EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			await ViewModel.FinishCreationCommand.ExecuteAsync(EntryEditTitle.Text);
		}
	}
}