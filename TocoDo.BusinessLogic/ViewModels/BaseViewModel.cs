using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return false;
			OnPropertyChanging(this, backingField, value, propertyName);

			backingField = value;

			OnPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		///     sender, oldValue, newValue, propertyName
		/// </summary>
		public event Action<object, object, object, string> PropertyChanging;

		protected virtual void OnPropertyChanging(object sender, object oldValue, object newValue,
			[CallerMemberName] string                       propertyName = null)
		{
			PropertyChanging?.Invoke(sender, oldValue, newValue, propertyName);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}