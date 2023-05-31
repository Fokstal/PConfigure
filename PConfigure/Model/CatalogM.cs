using PConfigure.View.MainWindowContentPage;
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
	class Basket
	{
		public Data_Blockpower? Blockpower { get; set; }
		public Data_CPU? CPU { get; set; }
		public Data_GPU? GPU { get; set; }
		public Data_Memory? Memory { get; set; }
		public Data_Motherboard? Motherboard { get; set; }
		public Data_RAM? RAM { get; set; }

		public bool CheckSocket()
		{
			return CPU?.Socket == Motherboard?.Socket;
		}
		public bool CheckTypeDDR()
		{
			return "DDR" + RAM?.TypeDDR == Motherboard?.TypeDDR; 
		}
		public bool CheckTypeGDDR()
		{
			return "GDDR" + GPU?.TypeGDDR == Motherboard?.TypeGDDR;
		}
		public bool CheckTypePower()
		{
			return GPU?.TypePower == Blockpower?.TypeGPUPower;
		}
		public bool CheckEqualTDP()
		{
			return CPU.TDP + GPU.TDP + RAM.TDP <= Blockpower.CapacityPower;
		}
	}

	class CatalogM
	{
		#region ListView Worker

		private static CatalogPage currentCatalogPage = new();

		public static void CreateTabControlFromData(object o)
		{
			currentCatalogPage = o as CatalogPage ?? new();
			TabControl tabControl = currentCatalogPage.CatalogTabControl;

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

			var type = listItem[0].GetType();
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

		#region BASKET Worker

		private static Basket _basket = new Basket();
		private static List<string> texts = new List<string>() { "Add to basket" };
		private static List<Action<object, EventArgs>> actions = new() { AddToBasket };

		private static void AddToBasket(object sender, EventArgs e)
		{
			var focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)) as ListViewItem ?? new();
			var focusedItem = focusedControl.Content;

			foreach (var property in _basket.GetType().GetProperties().ToList())
			{
				if (focusedItem.GetType() == property.PropertyType)
				{
					var str = focusedItem.GetType().ToString().Replace("PConfigure.Model.Data_", "");

					_basket.GetType().GetProperty(str).SetValue(_basket, focusedItem);

					MainM.basket = _basket;
				}
			}
		}

		private static void RemoveFromBasket(object sender, EventArgs e)
		{

		}

		#endregion
	}
}
