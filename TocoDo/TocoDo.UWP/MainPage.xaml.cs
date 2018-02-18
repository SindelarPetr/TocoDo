namespace TocoDo.UWP
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();

			LoadApplication(new UI.App());
		}
	}
}