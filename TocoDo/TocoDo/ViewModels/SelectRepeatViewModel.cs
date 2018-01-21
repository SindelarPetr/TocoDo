using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TocoDo.Models;
using Xamarin.Forms;

namespace TocoDo.ViewModels
{
    public class SelectRepeatViewModel : BaseViewModel
    {
	    private bool _isOpen;
	    public bool IsOpen
	    {
		    get => _isOpen;
		    set => SetValue(ref _isOpen, value);
	    }

		public RepeatType RepeatType { get; set; }

		public short RepeatsADay { get; set; }

	    public SelectRepeatViewModel(RepeatType repeatType, short repeatsADay)
	    {
			RepeatType = repeatType;
			RepeatsADay = repeatsADay;
		}

	    protected override void OnPropertyChanged(string propertyName = null)
	    {
		    base.OnPropertyChanged(propertyName);

		    switch (propertyName)
		    {
			    case nameof(IsOpen):

				    break;
		    }
	    }
	}
}
