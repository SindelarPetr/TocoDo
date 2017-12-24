using System.Threading.Tasks;
using Xamarin.Forms;

namespace TocoDo.Services
{
	public static class PageService
	{
		private static NavigationPage NavigationPage => (NavigationPage) ((MasterDetailPage) App.Current.MainPage).Detail;
		

		public static async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}

		public static async Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
		{
			return await App.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
		}

		public static async Task PushAsync(Page page)
		{
			await NavigationPage.PushAsync(page);
		}

		public static async Task PushModalAsync(Page page)
		{
			await NavigationPage.Navigation.PushModalAsync(page, true);
		}

		public static async Task PopAsync()
		{
			await NavigationPage.PopAsync();
		}

		public static async Task PopModalAsync()
		{
			await NavigationPage.Navigation.PopModalAsync(true);
		}
	}
}
