using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TocoDo.BusinessLogic.DependencyInjection.Models;
using TocoDo.BusinessLogic.ViewModels;
using TocoDo.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TocoDo.UI.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyRepeatTypePopup : PopupPage
	{
		public ModifyRepeatTypeViewModel ViewModel { get; }
		public ModifyRepeatTypePopup(ModifyRepeatTypeViewModel popupInfo)
		{
			ViewModel = popupInfo;
			InitializeComponent();
		}
	}
}