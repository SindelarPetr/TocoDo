using System.Drawing;
using System.Windows.Input;

namespace TocoDo.BusinessLogic.ViewModels
{
	public class TodoBoxViewModel : BaseViewModel
	{
		private bool _isChecked;

		public bool IsChecked
		{
			get => _isChecked;
			set => SetValue(ref _isChecked, value);
		}

		public bool IsBackgroundVisible { get; set; }

		public Color BackgroundImageColor { get; set; }
		public Color CheckedColor { get; set; }
		public Color UncheckedColor { get; set; }

		public ICommand CheckCommand { get; set; }

		public TaskViewModel Task { get; set; }

		public TodoBoxViewModel(TaskViewModel taskViewModel)
		{
			Task = taskViewModel;
		}
	}
}
