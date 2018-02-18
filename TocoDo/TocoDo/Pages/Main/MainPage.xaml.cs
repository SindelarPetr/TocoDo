using System;
using TocoDo.BusinessLogic;
using Xamarin.Forms;

namespace TocoDo.UI.Pages.Main
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			MyLogger.WriteStartMethod();
			try
			{
				InitializeComponent();
			}
			catch (Exception e)
			{
				MyLogger.WriteException(e);
				throw;
			}

			MyLogger.WriteEndMethod();
		}
	}
}