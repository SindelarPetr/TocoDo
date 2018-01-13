using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TocoDo.Services
{
	public static class PageService
	{
		private static NavigationPage NavigationPage => (NavigationPage) App.Current.MainPage;
		

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
			var stack = NavigationPage.Navigation.NavigationStack;
			if (stack.Count != 0 && stack[stack.Count - 1].GetType() == page.GetType())
				return;

			Debug.Write("----------- Before PushAsync");
			var nav = NavigationPage;
			var navi = NavigationPage.Navigation;
			await navi.PushAsync(page);
			Debug.Write("----------- After PushAsync");
		}

		public static async Task PushModalAsync(Page page)
		{
			var stack = NavigationPage.Navigation.ModalStack;
			Debug.WriteLine("-------------- Stack count is: " + stack.Count);
			if (stack.Count != 0 && stack[stack.Count - 1].GetType() == page.GetType())
				return;

			Debug.Write("----------- Before PushModalAsync");
			await NavigationPage.Navigation.PushModalAsync(page, true);
			Debug.Write("----------- After PushModalAsync");
		}

		public static async Task PopAsync()
		{
			Debug.Write("----------- Before PopAsync");
			await NavigationPage.PopAsync();
			Debug.Write("----------- After PopAsync");
		}

		public static async Task PopModalAsync()
		{
			Debug.Write("----------- Before PopModalAsync");
			await NavigationPage.Navigation.PopModalAsync(true);
			Debug.Write("----------- After PopModalAsync");
		}
	}
}
