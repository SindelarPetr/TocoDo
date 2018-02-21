using System;
// Credits: http://jake.ginnivan.net/awaitable-delegatecommand/

using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TocoDo.BusinessLogic.Helpers.Commands
{
	public interface IRaiseCanExecuteChanged
	{
		void RaiseCanExecuteChanged();
	}

	// And an extension method to make it easy to raise changed events
	public static class CommandExtensions
	{
		public static void RaiseCanExecuteChanged(this ICommand command)
		{
			var canExecuteChanged = command as IRaiseCanExecuteChanged;

			canExecuteChanged?.RaiseCanExecuteChanged();
		}
	}
}
