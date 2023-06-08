using PConfigure.Addition;
using PConfigure.Model;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class AdminPanelVM
	{
		public static AdminPanelPage? _instancePage;

		public static readonly RelayCommand _createContentOnTabControl = new(o =>
		{
			AdminPanelPage adminPanelPage = o as AdminPanelPage ?? new();
			_instancePage = adminPanelPage;

			List<IEnumerable<object>> items = DataWorker.GetAllItemWithoutCart(out List<Type> typesItem);

			ControlFromData.AddDataInTabControl(adminPanelPage.ContentTabControl, items, typesItem, ReadyControls.ContextMenuInAdminPanel);

			ReadyControls.InvokeNotify(new(), new());
		});

		public RelayCommand CreateContentOnTabControl { get => _createContentOnTabControl; }
	}
}
