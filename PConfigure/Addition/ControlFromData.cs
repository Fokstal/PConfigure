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

namespace PConfigure.Addition
{
	class ControlFromData
	{

		#region TabControl

		public static void AddDataInTabControl(TabControl tabControl, List<IEnumerable<object>> items, List<Type> typesItem, ContextMenu? contextMenu = null)
		{
			tabControl.Items.Clear();

			int count = 0;

			foreach (var e in items)
			{
				TabItem tabItem = CreateTabItemFromListItem(e, typesItem[count].Name.ToString(), contextMenu);

				tabControl.Items.Add(tabItem);
				count++;
			}
		}
		private static TabItem CreateTabItemFromListItem(IEnumerable<object> item, string nameItem, ContextMenu? contextMenu)
		{
			TabItem tabItem = new();
			tabItem.Header = nameItem;

			ListView listView = new();
			listView.Foreground = new SolidColorBrush(Colors.GhostWhite);
			listView.ContextMenu = contextMenu;
			tabItem.Content = listView;

			GridView gridView = new();

			var listItem = item.ToList();

			var type = listItem.Count > 0 ? listItem[0].GetType() : typeof(object);
			var props = type.GetProperties();

			foreach (var prop in props)
			{
				GridViewColumn column = new();

				column.DisplayMemberBinding = new Binding(prop.Name.ToString());
				column.Header = prop.Name;

				gridView.Columns.Add(column);
			}

			listView.ItemsSource = item;
			listView.View = gridView;

			return tabItem;
		}

		#endregion


		#region ContextMenu
		public static MenuItem CreateMenuItem(string nameHeader, Action<object, EventArgs> action)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.Header = nameHeader;
			menuItem.Click += new(action);
			menuItem.Click += ReadyControls.InvokeNotify;

			return menuItem;
		}
		public static ContextMenu CreateContextMenu(List<MenuItem> menuItems)
		{
			ContextMenu contextMenu = new();

			foreach (MenuItem menuItem in menuItems)
			{ 
				contextMenu.Items.Add(menuItem);
			}

			return contextMenu;
		}

		#endregion
	}
}
