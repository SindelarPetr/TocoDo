using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyTaskPage : ContentPage
	{
		private TaskViewModel TaskViewModel => (TaskViewModel)BindingContext;

		public ModifyTaskPage(TaskViewModel taskToModify)
		{
			BindingContext = taskToModify;
			InitializeComponent();
		}

		private void EntryTitle_OnCompleted(object sender, FocusEventArgs e)
		{
			TaskViewModel.UpdateCommand.Execute(sender);
		}
	}
}