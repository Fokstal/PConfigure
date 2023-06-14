using PConfigure.View;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PConfigure.Model;
using System.Windows;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using PConfigure.Addition;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using PConfigure.Model.ModelData;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class CatalogVM : INotifyPropertyChanged
	{
		public static CatalogPage? currentCatalogPage;

		#region DataContext to ListView and ComboBox

		private static List<IEnumerable<object>> listItem = DataWorker.GetAllItem(out List<Type> a);

		private static List<Data_Blockpower> _listBlockpower = DataWorker.GetAllBlockpower();
		private static List<Data_CPU> _listCPU = DataWorker.GetAllCPU();
		private static List<Data_GPU> _listGPU = DataWorker.GetAllGPU();
		private static List<Data_Memory> _listMemory = DataWorker.GetAllMemory();
		private static List<Data_Motherboard> _listMotherboard = DataWorker.GetAllMotherboard();
		private static List<Data_RAM> _listRAM = DataWorker.GetAllRAM();

		private readonly static ContextMenu _contextMenu = ReadyControls.ContextMenuInCatalog;

		public List<Data_Blockpower> ListBlockpower { get => _listBlockpower; set { _listBlockpower = value; OnPropertyChanged(nameof(ListBlockpower)); } }
		public List<Data_CPU> ListCPU { get => _listCPU; set { _listCPU = value; OnPropertyChanged(nameof(ListCPU)); } }
		public List<Data_GPU> ListGPU { get => _listGPU; set { _listGPU = value; OnPropertyChanged(nameof(ListGPU)); } }
		public List<Data_Memory> ListMemory { get => _listMemory; set { _listMemory = value; OnPropertyChanged(nameof(ListMemory)); } }
		public List<Data_Motherboard> ListMotherboard { get => _listMotherboard; set { _listMotherboard = value; OnPropertyChanged(nameof(ListMotherboard)); } }
		public List<Data_RAM> ListRAM { get => _listRAM; set { _listRAM = value; OnPropertyChanged(nameof(ListRAM)); } }

		public ContextMenu ContextMenu { get => _contextMenu; }
		#endregion


		#region Item

		public static RelayCommand _selectItemCmd = new(o =>
		{
			CatalogPage catalogPage = o as CatalogPage ?? new();

			currentCatalogPage = catalogPage;

			_listBlockpower = DataWorker.GetAllBlockpower();
			_listCPU = DataWorker.GetAllCPU();
			_listGPU = DataWorker.GetAllGPU();
			_listMemory = DataWorker.GetAllMemory();
			_listMotherboard = DataWorker.GetAllMotherboard();
			_listRAM = DataWorker.GetAllRAM();

			SetFilterList_Item();
		});

		public RelayCommand SelectItemCmd { get => _selectItemCmd; set => _selectItemCmd = value; }

		#endregion


		#region Filter panel

		private static string currentTabItemHeader = "";
		private static string searchBoxValue = "";
		private static string currentSearchType = "";
		public static ListView? currentListView;



		private readonly RelayCommand _setSearchType = new(o =>
		{
			CatalogPage catalogPage = o as CatalogPage ?? new();

			if (catalogPage.FilterComboBox.SelectedValue is not null)
			currentSearchType = catalogPage.FilterComboBox.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem:", "");
		});
		private readonly RelayCommand _searchingCmd = new(o =>
		{
			if (currentSearchType != "")
			{
				CatalogPage catalogPage = o as CatalogPage ?? new();

				ListView listView = currentListView;



				if (currentTabItemHeader == "Blockpower") SetFilterList_Blockpower();
				if (currentTabItemHeader == "CPU") SetFilterList_CPU();
				if (currentTabItemHeader == "GPU") SetFilterList_GPU();
				if (currentTabItemHeader == "Memory") SetFilterList_Memory();
				if (currentTabItemHeader == "Motherboard") SetFilterList_Motherboard();
				if (currentTabItemHeader == "RAM") SetFilterList_RAM();




			}
		});
		private readonly RelayCommand _selectFilterCmd = new(o =>
		{
			if (currentListView is not null)
			{
				ComboBox comboBox = o as ComboBox ?? new();
				List<string> listNameType = new();

				Type type = typeof(object);

				if (currentListView.Items.CurrentItem is not null)
				{
					type = currentListView.Items.CurrentItem.GetType();
				}

				comboBox.ItemsSource = GetParamsByType(type);
			}
		});
		private readonly RelayCommand _selectTabItem = new(o =>
		{
			TabItem tabItem = o as TabItem ?? new();

			currentTabItemHeader = tabItem.Header.ToString() ?? "";

			foreach (var childrenTabItem in LogicalTreeHelper.GetChildren(tabItem))
			{
				if (childrenTabItem is ListView)
				{
					currentListView = childrenTabItem as ListView ?? new();
				}
			}

		searchBoxValue = "";
		currentSearchType = "";
	});



		public string SearchBoxValue { get => searchBoxValue; set => searchBoxValue = value; }

		public RelayCommand SetSearchType { get => _setSearchType; }
		public RelayCommand SearchingCmd { get => _searchingCmd; }
		public RelayCommand SelectFilterCmd { get => _selectFilterCmd; }
		public RelayCommand SelectTabItem { get => _selectTabItem; }


		private static List<string> GetParamsByType(Type type)
		{
			List<string> listParams = new();

			foreach (var e in type.GetProperties())
			{
				if (e.Name != "ID" && e.Name != "Price") listParams.Add(e.Name);
			}

			return listParams;
		}

		#region Trash

		public static void SetCartList_Item()
		{
			var item = DataWorker.GetAllItem(out List<Type> a);
			List<ListView> listViews = new List<ListView>();

			foreach (var child in LogicalTreeHelper.GetChildren(currentCatalogPage))
			{
				if (child is Grid)
				{
					foreach (var childGrid in LogicalTreeHelper.GetChildren(child as Grid))
					{
						if (childGrid is TabControl)
						{
							foreach (var childTabControl in LogicalTreeHelper.GetChildren(childGrid as TabControl))
							{
								if (childTabControl is TabItem)
								{
									foreach (var childTabItem in LogicalTreeHelper.GetChildren(childTabControl as TabItem))
									{
										if (childTabItem is ListView)
										{
											listViews.Add(childTabItem as ListView ?? new());
										}
									}
								}
							}
						}
					}
				}
			}

			for (int i = 0; i < listViews.Count; i++)
			{
				listViews[i].ItemsSource = null;
				listViews[i].Items.Clear();
				listViews[i].ItemsSource = item[i];
				listViews[i].Items.Refresh();
			}

			currentCatalogPage.SearchPanelTextBox.Text = null;
		}

		public static void SetFilterList_Item()
		{
			SetFilterList_Blockpower();
			SetFilterList_CPU();
			SetFilterList_GPU();
			SetFilterList_Memory();
			SetFilterList_Motherboard();
			SetFilterList_RAM();
		}

		private static void SetFilterList_Blockpower()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllBlockpower(CartVM.CurrentCart);
				List<Data_Blockpower> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType) is not null)
					{
						if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
						{
							goodList.Add(e);
						}
					}
				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}
		private static void SetFilterList_CPU()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllCPU(CartVM.CurrentCart);
				List<Data_CPU> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
					{
						goodList.Add(e);
					}

				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}
		private static void SetFilterList_GPU()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllGPU(CartVM.CurrentCart);
				List<Data_GPU> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
					{
						goodList.Add(e);
					}

				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}
		private static void SetFilterList_Memory()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllMemory();
				List<Data_Memory> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
					{
						goodList.Add(e);
					}

				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}
		private static void SetFilterList_Motherboard()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllMotherboard(CartVM.CurrentCart);
				List<Data_Motherboard> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
					{
						goodList.Add(e);
					}

				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}
		private static void SetFilterList_RAM()
		{
			if (currentListView is not null)
			{
				var resultList = DataWorker.GetAllRAM(CartVM.CurrentCart);
				List<Data_RAM> goodList = new();

				currentListView.ItemsSource = null;

				foreach (var e in resultList)
				{
					if (e.GetType().GetProperty(currentSearchType).GetValue(e).ToString().ToLower().Contains(searchBoxValue.ToLower()))
					{
						goodList.Add(e);
					}

				}

				currentListView.ItemsSource = null;
				currentListView.Items.Clear();
				currentListView.ItemsSource = goodList;
				currentListView.Items.Refresh();
			}
		}

		#endregion

		#endregion


		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged is not null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}

		}

		#endregion
	}
}
