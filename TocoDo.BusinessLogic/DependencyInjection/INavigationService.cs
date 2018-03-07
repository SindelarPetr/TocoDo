using System.Threading.Tasks;
using TocoDo.BusinessLogic.Properties;

namespace TocoDo.BusinessLogic.DependencyInjection
{
	public enum PageType
	{
		EditDescriptionPage,
		ModifyHabitPage,
		HabitProgressPage,
		ModifyTaskPage,
		RepeatTypePopup
	}

	public interface INavigationService
	{
		Task<bool>   DisplayAlert(string       title, string message, string accept, string        cancel);
		Task<string> DisplayActionSheet(string title, string cancel, string  destruction, string[] buttons);

		Task PushAsync(PageType page, object param = null);
		Task PopAsync();

		Task PushModalAsync(PageType page, object param = null);
		Task PopModalAsync();

		Task PushPopupAsync(PageType page, object param = null);
		Task PopPopupAsync();
	}

	// TODO: Use the DisplayDatePickingActionSheet where is needed
	public enum DatePickingActionSheetResult
	{
		Canceled,
		Today,
		Tomorrow,
		InOneWeek,
		PickADate
	}

	public static class NavigationServiceHelper
	{
		public static async Task<DatePickingActionSheetResult> DisplayDatePickingActionSheet(
			this INavigationService navigation, bool todayIncluded = true)
		{
			var choices = todayIncluded
				? new[]
				  {
					  Resources.Today, Resources.Tomorrow, Resources.InOneWeek,
					  Resources.PickADate
				  }
				: new[]
				  {
					  Resources.Tomorrow, Resources.InOneWeek,
					  Resources.PickADate
				  };
			var result = await navigation.DisplayActionSheet(Resources.PickADate, Resources.Cancel, null, choices);
			if (result == Resources.Cancel)
				return DatePickingActionSheetResult.Canceled;

			if (result == Resources.Today)
				return DatePickingActionSheetResult.Today;

			if (result == Resources.Tomorrow)
				return DatePickingActionSheetResult.Tomorrow;

			if (result == Resources.InOneWeek)
				return DatePickingActionSheetResult.InOneWeek;

			return DatePickingActionSheetResult.PickADate;
		}
	}
}