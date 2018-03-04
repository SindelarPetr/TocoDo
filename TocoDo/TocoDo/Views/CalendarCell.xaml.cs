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
		private static readonly BindablePropertyKey BusynessTextPropertyKey = BindableProperty.CreateReadOnly(nameof(BusynessText), typeof(string), typeof(string), "");

		public static BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DateTime), default(DateTime));
		public static BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(ICommand));
		public static BindableProperty IsSideMonthProperty = BindableProperty.Create(nameof(IsSideMonth), typeof(bool), typeof(bool), false);
		public static BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(bool), false);
		public static BindableProperty IsTodayProperty = BindableProperty.Create(nameof(IsToday), typeof(bool), typeof(bool), false);
		public static BindableProperty BusynessProperty = BindableProperty.Create(nameof(Busyness), typeof(int), typeof(int), 0);
		public static BindableProperty BusynessTextProperty = BusynessTextPropertyKey.BindableProperty;
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
		/// <summary>
		/// This number is used for creating the BusynessText according to how the number is big.
		/// </summary>
		public int Busyness
		{
			get => (int) GetValue(BusynessProperty);
			set => SetValue(BusynessProperty, Math.Max(value, 0));
		}
		/// <summary>
		/// Those are the points indicating the amount of tasks and habits for the day
		/// </summary>
		public string BusynessText
		{
			get => (string) GetValue(BusynessTextProperty);
			private set => SetValue(BusynessTextPropertyKey, value);
		}
		#endregion

		public CalendarCell(DateTime date)
		{
			MyLogger.WriteStartMethod();
			Date = date;
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

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == nameof(Busyness))
			{
				BusynessText = GetBusynessText(Busyness);
			}
		}

		private string GetBusynessText(int busyness)
		{
			if (busyness >= 6)
				return "•••";

			if (busyness >= 3)
				return "••";

			if (busyness >= 1)
				return "•";

			return " ";
		}
	}
}