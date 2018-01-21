using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TocoDo.Helpers
{
	public class MyCommand : ICommand
	{
		private readonly Action<object> _executeAction;
		private readonly Func<object, bool> _canExecuteAction;

		public MyCommand(Action executeAction, Func<bool> canExecuteFunc = null) : this(o => executeAction?.Invoke(),
			o => canExecuteFunc?.Invoke() ?? true)
		{

		}

		public MyCommand(Action<object> executeAction, Func<object, bool> canExecuteAction = null)
		{
			_executeAction = executeAction;
			_canExecuteAction = canExecuteAction;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecuteAction?.Invoke(parameter) ?? true;
		}

		public void Execute(object parameter)
		{
			_executeAction?.Invoke(parameter);
		}

		public event EventHandler CanExecuteChanged;
	}
}