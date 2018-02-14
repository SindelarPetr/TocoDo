using Xamarin.Forms;

namespace TocoDo.UI.Controls
{
    public class ResizablePicker : Picker
    {
	    public ResizablePicker()
	    {
		    SelectedIndexChanged += (a, b) => InvalidateMeasure();
	    }
    }
}
