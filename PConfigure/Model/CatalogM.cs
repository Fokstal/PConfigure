using PConfigure.View.MainWindowContentPage;
using PConfigure.ViewModel.MainWindowContentPageVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace PConfigure.Model
{
	class CatalogM
	{
		#region ListView Worker

		private static CatalogPage currentCatalogPage = new();

		public static void CreateTabControlFromData(object o)
		{
			currentCatalogPage = o as CatalogPage ?? new();
			TabControl tabControl = currentCatalogPage.CatalogTabControl;

			tabControl.Items.Clear();
			

			List<IEnumerable<object>> items = DataWorker.GetAllItem(out List<Type> vl);

			int count = 0;

			foreach (var e in items)
			{
				TabItem tabItem = CatalogM.CreateListViewFromListObject(e, vl[count].Name.ToString());

				tabControl.Items.Add(tabItem);
				count++;
			}
		}
		private static TabItem CreateListViewFromListObject(IEnumerable<object> item, string nameItem)
		{
			TabItem tabItem = new TabItem();
			tabItem.Header = nameItem;

			ListView listView = new ListView();
			listView.Foreground = new SolidColorBrush(Colors.GhostWhite);
			listView.ContextMenu = CreateContextMenu();
			tabItem.Content = listView;

			GridView gridView = new GridView();

			var listItem = item.ToList();

			var type = listItem.Count > 0 ? listItem[0].GetType() : typeof(object);
			var props = type.GetProperties();

			foreach (var prop in props)
			{
				GridViewColumn column = new GridViewColumn();

				column.DisplayMemberBinding = new Binding(prop.Name.ToString());
				column.Header = prop.Name;

				gridView.Columns.Add(column);
			}

			listView.ItemsSource = item;
			listView.View = gridView;

			return tabItem;
		}

		private static ContextMenu CreateContextMenu()
		{
			ContextMenu contextMenu = new ContextMenu();

			for (int i = 0; i < texts.Count; i++)
			{
				MenuItem menuItem = new MenuItem();
				menuItem.Header = texts[i];
				menuItem.Click += new(actions[i]);

				contextMenu.Items.Add(menuItem);
			}

			return contextMenu;
		}

		#endregion

		#region Cart Worker

		public static CartPage? CartPage;

		private static readonly List<string> texts = new List<string>() { "Add to Cart" };
		private static readonly List<Action<object, EventArgs>> actions = new() { AddToCart };

		private static void AddToCart(object sender, EventArgs e)
		{
			var focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)) as ListViewItem ?? new();
			var focusedItem = focusedControl.Content;

			foreach (var property in CartVM.CurrentCart.GetType().GetProperties().ToList())
			{
				if (focusedItem.GetType() == property.PropertyType)
				{
					var str = focusedItem.GetType().ToString().Replace("PConfigure.Model.Data_", "");

					CartVM.CurrentCart.GetType().GetProperty(str)?.SetValue(CartVM.CurrentCart, focusedItem);

					if (CartPage is not null)
					{
						CartPage.ListItem.ItemsSource = CartM.GetCartItem(CartVM.CurrentCart);
					}

					DataWorker.Cart = CartVM.CurrentCart;

					CreateTabControlFromData(currentCatalogPage);

					CartPage.PriceTextBlock.Text = CartVM.CurrentCart.GetPrices().ToString();
				}
			}
		}

		private static void RemoveFromCart(object sender, EventArgs e)
		{

		}

		#endregion
	}
}
