namespace TocoDo.UI.Views.Habits
{
	public interface IEntryFocusable<TViewModel>
	{
		TViewModel ViewModel { get; set; }
		void       FocusEntry();
	}
}