﻿// Credits: http://jake.ginnivan.net/awaitable-delegatecommand/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TocoDo.BusinessLogic.Helpers.Commands
{
	public interface IAsyncCommand : IAsyncCommand<object>
	{
	}

	public interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
	{
		Task     ExecuteAsync(T    obj);
		bool     CanExecute(object obj);
		ICommand Command { get; }
	}
}
