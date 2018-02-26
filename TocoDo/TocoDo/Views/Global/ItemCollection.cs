using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using NetBox.Extensions;
using TocoDo.BusinessLogic;
using TocoDo.BusinessLogic.ItemFilters;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Views.Habits;
using Xamarin.Forms;
using PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;

namespace TocoDo.UI.Views.Global
{
	public class ItemCollection<TViewModel, TView> : ContentView
		where TViewModel : ICreateMode, INotifyPropertyChanged, INotifyPropertyChanging
		where TView : View, IEntryFocusable<TViewModel>
	{
		private readonly string _schedulePropertyName;
		private readonly StackLayout                      _mainLayout;
		public ItemFilter<TViewModel> ItemFilter { get; set; } 

		public ItemCollection(Func<TViewModel, TView> factoryFunc, string schedulePropertyName)
		{
			_schedulePropertyName = schedulePropertyName;
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
				// Subscribe to every Habit for schedule date chnaged and changing, add or remove the habit from the list
				case NotifyCollectionChangedAction.Add:
					foreach (TViewModel item in e.NewItems)
					{
						item.PropertyChanging += ItemOnPropertyChanging;
						item.PropertyChanged += ItemOnPropertyChanged;
						if (ItemFilter?.Filter(item) ?? false) AddItem(item);
					}

					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (TViewModel item in e.OldItems)
					{
						item.PropertyChanged -= ItemOnPropertyChanged;
						item.PropertyChanging -= ItemOnPropertyChanging;
						if (ItemFilter?.Filter(item) ?? false) RemoveItem(item);
					}
					break;
			}
		}

		protected virtual void AddItem(TViewModel viewModel)
		{
			MyLogger.WriteStartMethod();
			var itemView = FactoryFunc(viewModel);
			_mainLayout.Children.Add(itemView);

			if (viewModel.IsCreateMode)
				itemView.FocusEntry();
			MyLogger.WriteEndMethod();
		}

		protected virtual void RemoveItem(TViewModel viewModel)
		{
			MyLogger.WriteStartMethod();
			var itemView       = FindItem(viewModel);
			_mainLayout.Children.Remove(itemView);//itemView.IsVisible = false;
			MyLogger.WriteEndMethod();
		}

		private void ItemOnPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
		{
			MyLogger.WriteStartMethod();
			if (propertyChangingEventArgs.PropertyName == _schedulePropertyName && sender is TViewModel vm)
			{
				MyLogger.WriteInMethod("Before if");
				if (ItemFilter.Filter(vm))
				{
					MyLogger.WriteInMethod("In if");
					RemoveItem(vm);
				}
			}
			MyLogger.WriteEndMethod();
		}

		private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			MyLogger.WriteStartMethod();
			if (propertyChangedEventArgs.PropertyName == _schedulePropertyName && sender is TViewModel vm)
			{
				if (ItemFilter.Filter(vm))
					AddItem(vm);
			}
			MyLogger.WriteEndMethod();
		}

		private TView FindItem(TViewModel viewModel)
		{
			foreach (var child in _mainLayout.Children)
			{
				if (!(child is TView view) || !view.IsVisible)
					continue;

				if (view.ViewModel.Equals(viewModel))
					return view;
			}

			return null;
		}
	}
}