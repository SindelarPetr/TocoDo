using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using TocoDo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TodoSetView : ContentView
	{
		#region TasksSource

		private ObservableCollection<TaskViewModel> _taskSource;
		public ObservableCollection<TaskViewModel> TasksSource
		{
			get => _taskSource;
			set
			{
				UnbindSource();
				_taskSource = value;
				BindSource();
			}
		}

		private void BindSource()
		{
			TasksSource.CollectionChanged += TasksSourceOnCollectionChanged;
		}

		private void UnbindSource()
		{
			if (TasksSource != null)
				TasksSource.CollectionChanged -= TasksSourceOnCollectionChanged;
		}
		#endregion

		#region IsCalendarVisible
		public static BindableProperty IsCalendarVisibleProperty = BindableProperty.Create(
			propertyName: "IsCalendarVisible",
			returnType: typeof(bool),
			declaringType: typeof(bool),
			defaultValue: false);

		public bool IsCalendarVisible
		{
			get => (bool)GetValue(IsCalendarVisibleProperty);
			set => SetValue(IsCalendarVisibleProperty, value);
		}

		#endregion

		#region IsHeaderVisible
		public static BindableProperty IsHeaderVisibleProperty = BindableProperty.Create(
			propertyName: "IsHeaderVisible",
			returnType: typeof(bool),
			declaringType: typeof(bool),
			defaultValue: false);

		public bool IsHeaderVisible
		{
			get => (bool)GetValue(IsHeaderVisibleProperty);
			set => SetValue(IsHeaderVisibleProperty, value);
		}
		#endregion

		#region HeaderTextSize
		public static BindableProperty HeaderTextSizeProperty = BindableProperty.Create(
			propertyName: "HeaderTextSize",
			returnType: typeof(double),
			declaringType: typeof(double),
			defaultValue: 11D);

		public double HeaderTextSize
		{
			get => (double)GetValue(HeaderTextSizeProperty);
			set => SetValue(HeaderTextSizeProperty, value);
		}
		#endregion

		#region HeaderText
		public static BindableProperty HeaderTextProperty = BindableProperty.Create(
			propertyName: "HeaderText",
			returnType: typeof(string),
			declaringType: typeof(string),
			defaultValue: null);

		public string HeaderText
		{
			get => (string)GetValue(HeaderTextProperty);
			set => SetValue(HeaderTextProperty, value);
		}
		#endregion

		#region HeaderBackgroundColor
		public static BindableProperty HeaderBackgroundColorProperty = BindableProperty.Create(
			propertyName: "HeaderBackgroundColor",
			returnType: typeof(Color),
			declaringType: typeof(Color),
			defaultValue: Color.DimGray);

		public Color HeaderBackgroundColor
		{
			get => (Color)GetValue(HeaderBackgroundColorProperty);
			set => SetValue(HeaderBackgroundColorProperty, value);
		}
		#endregion

		protected StackLayout MainLayout => LayoutTodo;

		public TodoSetView()
		{
			Debug.WriteLine("------------------------- Called constructor of TodosetView -------------------------");
			InitializeComponent();
			Debug.WriteLine("------------------------- Created TodosetView -------------------------");
		}

		private void TasksSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			switch (notifyCollectionChangedEventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (TaskViewModel task in notifyCollectionChangedEventArgs.NewItems)
					{
						AddTaskModel(task);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (TaskViewModel task in notifyCollectionChangedEventArgs.OldItems)
					{
						RemoveTaskModel(task);
					}
					break;
			}
		}

		protected virtual void AddTaskModel(TaskViewModel taskModel)
		{
			var todoItem = new TodoItemView(taskModel);
			LayoutTodo.Children.Add(todoItem);
		}

		protected virtual void RemoveTaskModel(TaskViewModel taskModel)
		{
			TodoItemView todoItem = FindTodoItem(taskModel.Id);
			if (todoItem == null) return;

			LayoutTodo.Children.Remove(todoItem);
		}

		private TodoItemView FindTodoItem(int id)
		{
			foreach (var child in LayoutTodo.Children)
			{
				var todo = child as TodoItemView;

				if (todo?.TaskViewModel.Id == id)
					return todo;
			}

			return null;
		}
	}
}