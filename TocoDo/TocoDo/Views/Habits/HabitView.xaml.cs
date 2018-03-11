using System.Threading.Tasks;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views.Habits
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitView : BaseHabitView
	{
		public HabitView(IHabitViewModel habit) 
			: base(habit)
		{
			InitializeComponent();
		}

		public override void FocusEntry()
		{
			EntryEditTitle.Focus();
		}

		private async void EditTitle_OnUnfocused(object sender, FocusEventArgs e)
		{
			MyLogger.WriteStartMethod();
			await ViewModel.FinishCreationCommand.ExecuteAsync(EntryEditTitle.Text);
			MyLogger.WriteEndMethod();
		}
	}
}