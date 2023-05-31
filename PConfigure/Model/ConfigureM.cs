using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Themes;
using PConfigure.View.MainWindowContentPage;
using PConfigure.ViewModel.MainWindowContentPageVM;

namespace PConfigure.Model.ModelData
{
	class ConfigureM
	{
		//private static int countTDP = 0;


		//public static void SelectCPU(object o)
		//{
		//	ConfigurePage currentCP = o as ConfigurePage ?? new ConfigurePage();
		//	ComboBox comboBox = currentCP.CPUComboBox;


		//	List<Data_Motherboard> allTrueMotherboards = new();
		//	List<Data_Motherboard> allMotherboards = DataWorker.GetAllMotherboard();

		//	string value = comboBox.SelectedItem.GetType().GetProperty("Socket").GetValue(comboBox.SelectedItem, null).ToString();
		//	countTDP += Convert.ToInt32(comboBox.SelectedItem.GetType().GetProperty("TDP").GetValue(comboBox.SelectedItem, null));

		//	foreach (var motherboard in allMotherboards)
		//	{
		//		if (motherboard.Socket.Replace(" ", "") == value.Replace(" ", ""))
		//		{
		//			allTrueMotherboards.Add(motherboard);
		//		}

		//		if (motherboard.Socket is null)
		//		{
		//			allTrueMotherboards.Add(motherboard);
		//		}
		//	}

		//	currentCP.MotherboardComboBox.ItemsSource = allTrueMotherboards;
		//}

		//public static void SelectGPU(object o)
		//{
		//	ConfigurePage currentCP = o as ConfigurePage ?? new ConfigurePage();
		//	ComboBox comboBox = currentCP.CPUComboBox;


		//	List<Data_Motherboard> allTrueMotherboards = new();
		//	List<Data_Motherboard> allMotherboards = DataWorker.GetAllMotherboard();

		//	List<Data_Blockpower> allTrueBlockpower = new();
		//	List<Data_Blockpower> allBlockpower = DataWorker.GetAllBlockpower();

		//	string valueTypeGDDR = comboBox.SelectedItem.GetType().GetProperty("Type_GDDR").GetValue(comboBox.SelectedItem, null).ToString();
		//	int valueTypePower = Convert.ToInt32(comboBox.SelectedItem.GetType().GetProperty("Type_Power").GetValue(comboBox.SelectedItem, null).ToString());
		//	countTDP += Convert.ToInt32(comboBox.SelectedItem.GetType().GetProperty("TDP").GetValue(comboBox.SelectedItem, null));

		//	foreach (var motherboard in allMotherboards)
		//	{
		//		if (motherboard.TypeGDDR is null)
		//		{
		//			allTrueMotherboards.Add(motherboard);

		//			continue;
		//		}

		//		if (motherboard.TypeGDDR.Substring(2, 3) == valueTypeGDDR)
		//		{
		//			allTrueMotherboards.Add(motherboard);
		//		}
		//	}

		//	currentCP.MotherboardComboBox.ItemsSource = allTrueMotherboards;
		//}
	}
}
