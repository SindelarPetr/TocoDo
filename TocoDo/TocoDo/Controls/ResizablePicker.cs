using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TocoDo.Controls
{
    public class ResizablePicker : Picker
    {
	    public ResizablePicker()
	    {
		    SelectedIndexChanged += (a, b) => InvalidateMeasure();
	    }
    }
}
