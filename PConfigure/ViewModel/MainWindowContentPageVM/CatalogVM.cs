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

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class CatalogVM
	{
		#region BindingData for ListView

		private List<string> _listItem = DataWorker.GetNameAllItem();

		public List<string> ListItem { get => _listItem; set => _listItem = value; }

		#endregion

		#region Item

		private RelayCommand _selectItemCmd = new(o => CatalogM.CreateTabControlFromData(o));

		public RelayCommand SelectItemCmd { get => _selectItemCmd; set => _selectItemCmd = value; }

		#endregion
	}
}
