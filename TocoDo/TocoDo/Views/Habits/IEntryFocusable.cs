namespace TocoDo.UI.Views.Habits
{
    public interface IEntryFocusable<TViewModel>
    {
	    void FocusEntry();
		TViewModel ViewModel { get; set; }

    }
}
