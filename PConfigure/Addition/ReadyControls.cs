using PConfigure.Model;
using PConfigure.View.MainWindowContentPage;
using PConfigure.ViewModel.MainWindowContentPageVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using PConfigure.Model.ModelData;
using System.Windows.Media;

namespace PConfigure.Addition
{
	/// <summary>
	/// Instruction to adding of CONTEXT MENU
	/// 1. Create your Action (Delete, Add and more)
	/// 2. Add your Action in Dictionary with Name of action
	/// 3. Creatre property of type MenuItem and making Dictionary
	/// 4. Create property of type ContextMenu with your property MenuItem
	/// 5. READY!
	/// </summary>

	class ReadyControls
	{
		#region ContextMenu

		public static ContextMenu ContextMenuInCatalog
		{
			get
			{
				return ControlFromData.CreateContextMenu(new() { AddItemToCart });
			}
		}

		public static ContextMenu ContextMenuInAdminPanel
		{
			get
			{
				return ControlFromData.CreateContextMenu(new() { DeleteItemFromList, AddItemToList, EditItemInList });
			}
		}

		#endregion


		#region ContextMenu MenuItem

		private static readonly Dictionary<string, Action<object, EventArgs>> _actionsContextMenu = new()
		{
			{ "Add to CART", AddToCart },
			{ "Delete ITEM", DeleteItem },
			{ "Add ITEM", AddItem },
			{ "Edit ITEM", EditItem },
		};

		private static MenuItem AddItemToCart { get => ControlFromData.CreateMenuItem("Add to CART", _actionsContextMenu["Add to CART"]); }
		private static MenuItem DeleteItemFromList { get => ControlFromData.CreateMenuItem("Delete ITEM", _actionsContextMenu["Delete ITEM"]); }
		private static MenuItem AddItemToList {	get => ControlFromData.CreateMenuItem("Add ITEM", _actionsContextMenu["Add ITEM"]);	}
		private static MenuItem EditItemInList { get => ControlFromData.CreateMenuItem("Edit ITEM", _actionsContextMenu["Edit ITEM"]); }


		#region Action

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

					if (Cart.currentCartPage is not null)
					{
						Cart.currentCartPage.ListItem.ItemsSource = CartM.GetCartItem(CartVM.CurrentCart);
					}

					DataWorker.Cart = CartVM.CurrentCart;

					CatalogVM.SetCartList_Item();

					Cart.currentCartPage.PriceTextBlock.Text = CartVM.CurrentCart.GetPrices().ToString();

					new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive), "Added to cart", Brushes.DarkOliveGreen);
				}
			}
		}

		private static void DeleteItem(object sender, EventArgs e)
		{
			var focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)) as ListViewItem ?? new();
			var focusedItem = focusedControl.Content;

			DataWorker.DeleteValue(focusedItem, out string resultStr);

			AdminPanelVM._createContentOnTabControl.Execute(AdminPanelVM._instancePage);

			new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive), "Item deleted", Brushes.Crimson);
		}

		private static void AddItem(object sender, EventArgs e)
		{
			var focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)) as ListViewItem ?? new();
			var focusedItem = focusedControl.Content;

			WindowFromData.OpenAddNewValue(focusedItem);

			AdminPanelVM._createContentOnTabControl.Execute(AdminPanelVM._instancePage);

		}

		private static void EditItem(object sender, EventArgs e)
		{
			var focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)) as ListViewItem ?? new();
			var focusedItem = focusedControl.Content;

			WindowFromData.OpenEditValue(focusedItem);

			AdminPanelVM._createContentOnTabControl.Execute(AdminPanelVM._instancePage);
		}


		public static void InvokeNotify(object sender, EventArgs e)
		{
			new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive));
		}

		#endregion

		#endregion
	}
}
