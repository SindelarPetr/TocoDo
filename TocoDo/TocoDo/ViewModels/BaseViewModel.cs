using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TocoDo.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
		{
			if (propertyName == "ModelRepeatType")
			{

			}

			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return false;
			OnPropertyChanging(propertyName);

			backingField = value;

			OnPropertyChanged(propertyName);
			return true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event PropertyChangingEventHandler PropertyChanging;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
		{
			PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
		}
	}
}
