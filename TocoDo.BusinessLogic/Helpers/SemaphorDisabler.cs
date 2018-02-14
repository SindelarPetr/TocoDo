using System;

namespace TocoDo.BusinessLogic.Helpers
{
	public class SemaphorDisabler
	{
		private readonly Action _enableAction;
		private readonly Action _disableAction;

		private int _counter = 0;

		public SemaphorDisabler(Action enableAction, Action disableAction)
		{
			_enableAction = enableAction;
			_disableAction = disableAction;
		}

		public void Enable()
		{
			if (_counter == 0) return;

			_counter--;

			if (_counter != 0) return;

			_enableAction?.Invoke();
		}

		public void Disable()
		{
			_counter++;
			if (_counter != 1) return;

			_disableAction?.Invoke();
		}
	}
}
