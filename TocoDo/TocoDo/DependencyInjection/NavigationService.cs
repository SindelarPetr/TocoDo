using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.DependencyInjection;
using TocoDo.BusinessLogic.Helpers;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Pages;
using TocoDo.UI.Pages.Habits;
using TocoDo.UI.Pages.Tasks;
using TocoDo.UI.Popups;
using Xamarin.Forms;

namespace TocoDo.UI.DependencyInjection
{
	public class NavigationService : INavigationService
	{
		private static NavigationPage NavigationPage => (NavigationPage) Application.Current.MainPage;

		public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			return await NavigationPage.DisplayAlert(title, message, accept, cancel);
		}

		public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
		{
			return await NavigationPage.DisplayActionSheet(title, cancel, destruction, buttons);
		}

		public async Task PushAsync(PageType pageType, object param = null)
		{
			MyLogger.WriteStartMethod();
			var stack = NavigationPage.Navigation.NavigationStack;
			if (stack.Count != 0 && stack[stack.Count - 1].GetType() == GetType(pageType))
				return;
			
			Page page = CreatePage(pageType, param);

			MyLogger.WriteInMethod("Before PushAsync");
			await NavigationPage.PushAsync(page);
			MyLogger.WriteInMethod("----------- After PushAsync");
			MyLogger.WriteEndMethod();
		}

		public async Task PopAsync()
		{
			await NavigationPage.PopAsync();
		}

		public async Task PushModalAsync(PageType pageType, object param = null)
		{
			var stack = NavigationPage.Navigation.ModalStack;
			Debug.WriteLine("-------------- Stack count is: " + stack.Count);
			if (stack.Count != 0 && stack[stack.Count - 1].GetType() == GetType(pageType))
				return;

			var page = CreatePage(pageType, param);

			Debug.Write("----------- Before PushModalAsync");
			await NavigationPage.Navigation.PushModalAsync(page, true);
			Debug.Write("----------- After PushModalAsync");
		}

		public async Task PopModalAsync()
		{
			await NavigationPage.Navigation.PopModalAsync();
		}

		public async Task PushPopupAsync(PageType page, object param = null)
		{
			await NavigationPage.Navigation.PushPopupAsync((PopupPage)CreatePage(page, param));
		}

		public async Task PopPopupAsync()
		{
			await NavigationPage.Navigation.PopPopupAsync();
		}

		private Page CreatePage(PageType pageType, object param)
		{
			MyLogger.WriteStartMethod();
			switch (pageType)
			{
				case PageType.EditDescriptionPage:
					MyLogger.WriteEndMethod();
					return new EditDescriptionPage((EditDescriptionInfo)param);

				case PageType.ModifyHabitPage:
					MyLogger.WriteEndMethod();
					return new ModifyHabitPage((HabitViewModel)param);
				case PageType.HabitsPastPage:
					MyLogger.WriteEndMethod();
					return new HabitsPastPage();
					

				case PageType.HabitProgressPage:
					MyLogger.WriteEndMethod();
					return new HabitProgressPage((HabitViewModel)param);

				case PageType.ModifyTaskPage:
					MyLogger.WriteEndMethod();
					return new ModifyTaskPage((TaskViewModel)param);
				case PageType.RepeatTypePopup:
					return new ModifyRepeatTypePopup((ModifyRepeatTypeViewModel)param);

				case PageType.TasksPastPage:
					MyLogger.WriteEndMethod();
					return new TasksPastPage();

				default:
					throw new ArgumentOutOfRangeException(nameof(pageType), pageType, null);
			}
		}

		private Type GetType(PageType pageType)
		{
			MyLogger.WriteStartMethod();
			switch (pageType)
			{
				case PageType.EditDescriptionPage:
					MyLogger.WriteEndMethod();
					return typeof(EditDescriptionPage);

				case PageType.ModifyHabitPage:
					MyLogger.WriteEndMethod();
					return typeof(ModifyHabitPage);

				case PageType.HabitProgressPage:
					MyLogger.WriteEndMethod();
					return typeof(HabitProgressPage);

				case PageType.ModifyTaskPage:
					MyLogger.WriteEndMethod();
					return typeof(ModifyTaskPage);

				case PageType.RepeatTypePopup:
					MyLogger.WriteEndMethod();
					return typeof(ModifyRepeatTypePopup);

				case PageType.HabitsPastPage:
					MyLogger.WriteEndMethod();
					return typeof(HabitsPastPage);
				case PageType.TasksPastPage:
					MyLogger.WriteEndMethod();
					return typeof(TasksPastPage);

				default:
					throw new ArgumentOutOfRangeException(nameof(pageType), pageType, null);
			}
		}
	}
}