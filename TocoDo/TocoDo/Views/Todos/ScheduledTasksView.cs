using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Converters;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Todos
{
    public class ScheduledTasksView : ContentView
    {
		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ReadOnlyObservableCollection<ITaskViewModel>), typeof(ReadOnlyObservableCollection<ITaskViewModel>));

	    public ReadOnlyObservableCollection<ITaskViewModel> ItemsSource
	    {
		    get => (ReadOnlyObservableCollection<ITaskViewModel>) GetValue(ItemsSourceProperty);
		    set => SetValue(ItemsSourceProperty, value);
	    }

	    public StackLayout MainLayout;

	    public ScheduledTasksView()
	    {
		    Content = MainLayout = new StackLayout {Spacing = 0};
	    }

		#region ItemsSource binding
		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);

			if (propertyName == nameof(ItemsSource))
				UnbindItemsSource();
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == nameof(ItemsSource))
				BindItemsSource();
		}

		private void UnbindItemsSource()
		{
			if(ItemsSource != null)
				((INotifyCollectionChanged)ItemsSource).CollectionChanged -= OnCollectionChanged;
		}

	    private void BindItemsSource()
	    {
			if(ItemsSource != null)
				((INotifyCollectionChanged)ItemsSource).CollectionChanged += OnCollectionChanged;
		}

	    #endregion

	    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
	    {
		    switch (notifyCollectionChangedEventArgs.Action)
		    {
				case NotifyCollectionChangedAction.Add:
					foreach (var newItem in notifyCollectionChangedEventArgs.NewItems)
					{
						if(newItem is ITaskViewModel task && task.ScheduleDate != null) AddTask(task);
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems)
					{
						if (oldItem is ITaskViewModel task && task.ScheduleDate != null) RemoveTask(task);
					}
					break;
		    }
	    }

	    private void AddTask(ITaskViewModel newTask)
	    {
			if(newTask.ScheduleDate == null) 
				return;

			var scheduleDate = newTask.ScheduleDate.Value;

			// Find the right place
		    int i = 0;
		    for (; i < MainLayout.Children.Count; i++)
		    {
			    if(!(MainLayout.Children[i] is TodoItemView item) || item.ViewModel.ScheduleDate.Value.Date < scheduleDate.Date)
				    continue;

				// just add at the bottom of the group if there is already a group for that date
			    if (item.ViewModel.ScheduleDate.Value.Date == scheduleDate.Date)
			    {
					MainLayout.Children.Insert(i + 1, new TodoItemView(newTask));
					return;
			    }

				break;
		    }
			
		    // Insert the Task and then insert also a header for its group
			MainLayout.Children.Insert(i, new TodoItemView(newTask));
			MainLayout.Children.Insert(i, new Label { Text = DateToTextConverter.Convert(scheduleDate), FontSize = 13, TextColor = ((App)App.Current).ColorPrimary});
	    }

	    private void RemoveTask(ITaskViewModel oldItem)
	    {
		    // Find the task

			// If there is a title above, then if there is anothere title below or no items, remove the title above
	    }
    }
}
