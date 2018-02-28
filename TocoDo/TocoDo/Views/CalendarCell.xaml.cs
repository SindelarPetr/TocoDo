using System;
using System.Windows.Input;
using TocoDo.BusinessLogic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarCell : ContentView
	{
		#region Backing fields
		public static BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DateTime), default(DateTime));
		public static BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ICommand));
		public static BindableProperty IsSideMonthProperty = BindableProperty.Create(nameof(IsSideMonth), typeof(bool), typeof(bool), false);
		public static BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(bool), false);
		public static BindableProperty IsTodayProperty = BindableProperty.Create(nameof(IsToday), typeof(bool), typeof(bool), false);
		#endregion

		#region Properties
		public DateTime Date
		{
			get => (DateTime)GetValue(DateProperty);
			set => SetValue(DateProperty, value);
		}

		public ICommand TappedCommand
		{
			get => (ICommand) GetValue(TappedCommandProperty);
			set => SetValue(TappedCommandProperty, value);
		}
		
		public bool IsSideMonth
		{
			get => (bool) GetValue(IsSideMonthProperty);
			set => SetValue(IsSideMonthProperty, value);
		}

		public bool IsSelected
		{
			get => (bool) GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}

		public bool IsToday
		{
			get => (bool) GetValue(IsTodayProperty);
			set => SetValue(IsTodayProperty, value);
		}
		#endregion

		public CalendarCell()
		{
			MyLogger.WriteStartMethod();
			InitializeComponent();
			MyLogger.WriteEndMethod();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			if (TappedCommand?.CanExecute(this) ?? false)
			{
				TappedCommand.Execute(this);
			}
		}
	}
}