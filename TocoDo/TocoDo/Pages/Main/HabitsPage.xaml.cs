
using TocoDo.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HabitsPage : ContentPage
	{
		public HabitsPage()
		{
			InitializeComponent();
			CurrentHabitSetView.HabitsSource = StorageService.CurrentHabits;
		}
	}
}