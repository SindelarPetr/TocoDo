using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TocoDo.BusinessLogic.DependencyInjection;
using Xamarin.Forms;

namespace TocoDo.UI.DependencyInjection
{
	// TODO: Finish the implementation
	public class NavigationService : INavigationService
	{
		private static NavigationPage NavigationPage => (NavigationPage) App.Current.MainPage;

		async Task OldPushAsync(Page page)
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
		async Task OldPushModalAsync(Page page)
		{
			var stack = NavigationPage.Navigation.ModalStack;
			Debug.WriteLine("-------------- Stack count is: " + stack.Count);
			if (stack.Count != 0 && stack[stack.Count - 1].GetType() == page.GetType())
				return;

			Debug.Write("----------- Before PushModalAsync");
			await NavigationPage.Navigation.PushModalAsync(page, true);
			Debug.Write("----------- After PushModalAsync");
		}
		async Task OldPopAsync()
		{
			Debug.Write("----------- Before PopAsync");
			await NavigationPage.PopAsync();
			Debug.Write("----------- After PopAsync");
		}
		async Task OldPopModalAsync()
		{
			Debug.Write("----------- Before PopModalAsync");
			await NavigationPage.Navigation.PopModalAsync(true);
			Debug.Write("----------- After PopModalAsync");
		}

		public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			return await App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}

		public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
		{
			return await App.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
		}

		public Task PushAsync(PageType page, object param = null)
		{
			throw new NotImplementedException();
		}
		public Task PopAsync()
		{
			throw new NotImplementedException();
		}

		public Task PushModalAsync(PageType page, object param = null)
		{
			throw new NotImplementedException();
		}
		public Task PopModalAsync()
		{
			throw new NotImplementedException();
		}

		public Task PushPopupAsync(PopupType page, object param = null)
		{
			throw new NotImplementedException();
		}
		public Task PopPopupAsync()
		{
			throw new NotImplementedException();
		}
	}

}