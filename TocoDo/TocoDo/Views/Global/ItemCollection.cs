using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Habits;
using Xamarin.Forms;

namespace TocoDo.UI.Views.Global
{
	public class ItemCollection<TViewModel, TView> : ContentView
		where TViewModel : ICreateMode
		where TView : View, IEntryFocusable<TViewModel>
	{
		private readonly StackLayout                      _mainLayout;

		public ItemCollection(Func<TViewModel, TView> factoryFunc)
		{
			MyLogger.WriteStartMethod();
			FactoryFunc = factoryFunc;
			Content     = _mainLayout = new StackLayout();
			MyLogger.WriteEndMethod();
		}

		public Func<TViewModel, TView> FactoryFunc { get; set; }

		public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ReadOnlyObservableCollection<TViewModel>), typeof(ReadOnlyObservableCollection<TViewModel>));
		public ReadOnlyObservableCollection<TViewModel> ItemsSource
		{
			get => (ReadOnlyObservableCollection<TViewModel>)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		protected override void OnPropertyChanging(string propertyName = null)
		{
			base.OnPropertyChanging(propertyName);

			if(propertyName == nameof(ItemsSource))
				UnbindSource();
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == nameof(ItemsSource))
				BindSource();
		}

		private void BindSource()
		{
			MyLogger.WriteStartMethod();
			((INotifyCollectionChanged)ItemsSource).CollectionChanged += ItemsSourceOnCollectionChanged;
			MyLogger.WriteEndMethod();
		}

		private void UnbindSource()
		{
			if (ItemsSource != null)
				((INotifyCollectionChanged)ItemsSource).CollectionChanged -= ItemsSourceOnCollectionChanged;
		}

		private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (TViewModel task in e.NewItems) AddItem(task);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (TViewModel task in e.OldItems) RemoveItem(task);
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
			var habitItem       = FindItem(habit);
			habitItem.IsVisible = false;
		}

		private TView FindItem(TViewModel viewModel)
		{
			foreach (var child in _mainLayout.Children)
			{
				if (!(child is TView view))
					continue;

				if (view.ViewModel.Equals(viewModel))
					return view;
			}

			return null;
		}
	}
}