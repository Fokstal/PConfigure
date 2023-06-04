﻿using PConfigure.View;
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

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class CatalogVM
	{
		public static CatalogPage? currentCatalogPage;

		#region Item

		public static RelayCommand _selectItemCmd = new(o =>
		{
			CatalogPage catalogPage = o as CatalogPage ?? new();

			currentCatalogPage = catalogPage;

			List<IEnumerable<object>> items = DataWorker.GetAllItem(out List<Type> typesItem);

			ControlFromData.AddDataInTabControl(catalogPage.CatalogTabControl, items, typesItem, ReadyControls.ContextMenuInCatalog);
		});

		public RelayCommand SelectItemCmd { get => _selectItemCmd; set => _selectItemCmd = value; }

		#endregion
	}
}
