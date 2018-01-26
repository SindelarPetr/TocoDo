using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TocoDo.ViewModels;
using TocoDo.Views.Habits;
using Xamarin.Forms;

namespace TocoDo.Views.Todos
{
	public class ItemCollection<TViewModel, TView> : ContentView
	where TViewModel : ICreateMode
	where TView : View, IEntryFocusable<TViewModel>
	{
		public Func<TViewModel, TView> FactoryFunc { get; set; }
		private readonly StackLayout _mainLayout;
		private ObservableCollection<TViewModel> _itemsSource;
		public ObservableCollection<TViewModel> ItemsSource
		{
			get => _itemsSource;
			set
			{
				UnbindSource();
				_itemsSource = value;
				BindSource();
			}
		}
		
		public ItemCollection (Func<TViewModel, TView> factoryFunc)
		{
			FactoryFunc = factoryFunc;
			Content = _mainLayout = new StackLayout();
		}
		private void BindSource()
		{
			ItemsSource.CollectionChanged += HabitsSourceOnCollectionChanged;
		}

		private void UnbindSource()
		{
			if (ItemsSource != null)
				ItemsSource.CollectionChanged -= HabitsSourceOnCollectionChanged;
		}

		private void HabitsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (TViewModel task in e.NewItems)
					{
						AddItem(task);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (TViewModel task in e.OldItems)
					{
						RemoveItem(task);
					}
					break;

			}
		}

		private void AddItem(TViewModel habit)
		{
			var habitView = FactoryFunc(habit);
			_mainLayout.Children.Add(habitView);

			if (habit.IsCreateMode)
				habitView.FocusEntry();
		}

		private void RemoveItem(TViewModel habit)
		{
			TView habitItem = FindItem(habit);
			habitItem.IsVisible = false;
		}

		private TView FindItem(TViewModel viewModel)
		{
			foreach (var child in _mainLayout.Children)
			{
				if(!(child is TView view))
					continue;

				if (view.ViewModel.Equals(viewModel))
					return view;
			}

			return null;
		}
	}
}