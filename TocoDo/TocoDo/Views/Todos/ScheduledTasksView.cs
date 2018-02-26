using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ItemFilters;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Converters;
using TocoDo.UI.Views.Global;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Todos
{
    public class ScheduledTasksView : TaskCollection
    {
	    public StackLayout MainLayout;

	    public ScheduledTasksView()
	    {
		    Content = MainLayout = new StackLayout {Spacing = 0};
			ItemFilter = new TaskScheduledFilter();
	    }

	    protected override void AddItem(ITaskViewModel newTask)
	    {
			MyLogger.WriteStartMethod();
			if(newTask.ScheduleDate == null) 
				return;

			var scheduleDate = newTask.ScheduleDate.Value.Date;

			// Find the right place
		    int i = 0;
		    for (; i < MainLayout.Children.Count; i++)
		    {
				// Skip all the elements with lower date or which are headers
			    if (!(MainLayout.Children[i] is TodoItemView taskView) || taskView.ViewModel.ScheduleDate.Value.Date < scheduleDate)
					continue;

			    if (taskView.ViewModel.ScheduleDate.Value.Date == scheduleDate)
			    {
					// Iterate to the end of the section of TaskViews with the same ScheduleDate
				    do
				    {
					    i++;
				    } while (i < MainLayout.Children.Count && MainLayout.Children[i] is TodoItemView);

					// Add at the end
					MainLayout.Children.Insert(i, new TodoItemView(newTask));

					return;
			    }

			    i--;
			    break;
		    }
			
			// Add it one index above
			InsertWithHeader(i, newTask);
			MyLogger.WriteEndMethod();
	    }
		
	    private void InsertWithHeader(int index, ITaskViewModel newTask)
	    {
			MainLayout.Children.Insert(index, new TodoItemView(newTask));
			MainLayout.Children.Insert(index, new Label { Text = DateToTextConverter.Convert(newTask.ScheduleDate), FontSize = 13, TextColor = ((App)App.Current).ColorPrimary});
	    }

	    protected override void RemoveItem(ITaskViewModel oldItem)
	    {
			MyLogger.WriteStartMethod();
		    // Find the view of the task
		    for (int i = 0; i < MainLayout.Children.Count; i++)
		    {
			    if (MainLayout.Children[i] is TodoItemView taskView && taskView.IsVisible && taskView.ViewModel.Id == oldItem.Id)
			    {
					// Remove item on this index.
					// If there is a title above, then if there is anothere title below or no items, remove the title above
					RemoveItemWithHeader(i, oldItem);
			    }
		    }
			MyLogger.WriteEndMethod();
	    }

	    private void RemoveItemWithHeader(int index, ITaskViewModel task)
	    {
			// Remove item from the main layout
			MainLayout.Children.RemoveAt(index);

			// If there is a header above and there is a header or no items below, remove the above header
			if(MainLayout.Children.Count > 0 && MainLayout.Children[index - 1] is Label && (MainLayout.Children.Count == index || MainLayout.Children[index] is Label))
				MainLayout.Children.RemoveAt(index - 1);
	    }
    }
}
