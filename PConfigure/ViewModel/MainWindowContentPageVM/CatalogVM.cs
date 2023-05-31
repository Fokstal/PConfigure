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
        private List<Data_Blockpower> _listBlockpower = DataWorker.GetAllBlockpower();
        private List<Data_CPU> _listCPU = DataWorker.GetAllCPU();
        private List<Data_GPU> _listGPU = DataWorker.GetAllGPU();
        private List<Data_Memory> _listMemory = DataWorker.GetAllMemory();
        private List<Data_Motherboard> _listMotherboard = DataWorker.GetAllMotherboard();
        private List<Data_RAM> _listRAM = DataWorker.GetAllRAM();

        public List<string> ListItem { get => _listItem; set => _listItem = value; }
        public List<Data_Blockpower> ListBlockpower { get => _listBlockpower; set => _listBlockpower = value; }
        public List<Data_CPU> ListCPU { get => _listCPU; set => _listCPU = value; }
        public List<Data_GPU> ListGPU { get => _listGPU; set => _listGPU = value; }
        public List<Data_Memory> ListMemory { get => _listMemory; set => _listMemory = value; }
        public List<Data_Motherboard> ListMotherboard { get => _listMotherboard; set => _listMotherboard = value; }
        public List<Data_RAM> ListRAM { get => _listRAM; set => _listRAM = value; }

		#endregion

		#region Item

		private RelayCommand _selectItemCmd = new(o => CatalogM.CreateTabControlFromData(o));

		public RelayCommand SelectItemCmd { get => _selectItemCmd; set => _selectItemCmd = value; }

		#endregion
	}
}
