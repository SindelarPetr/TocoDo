using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
	public class BaseViewModel : BindableObject, INotifyPropertyChanged
	{
		protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return;

			backingField = value;

			OnPropertyChanged(propertyName);
		}
	}
}
