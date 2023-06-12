using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using PConfigure.Data;
using PConfigure.Model.ModelData;
using PConfigure.ViewModel.MainWindowContentPageVM;

namespace PConfigure.Model
{
	internal class DataWorker
	{ 
		private static bool CheckIsNull(object item)
		{
			List<PropertyInfo> listProp = item.GetType().GetProperties().ToList();
			List<Type> listTypeProp = new();

			int i = 0;
			foreach (var o in listProp)
			{
				listTypeProp.Add(o.PropertyType);
				if (listTypeProp[i].ToString().Contains("String"))
				{
					if (o.GetValue(item) is null) return true;
				}
				i++;
			}

			return false;
		}


		#region All item

		public static Cart Cart = new();

		public static List<string> GetNameAllItem()
		{
			List<string> listNameItem = new List<string>();

			List<IEnumerable<object>> listItems = GetAllItem(out List<Type> listNameTypeItem);

			int i = 0;

			foreach (var listItem in listItems)
			{
				List<string> listName = GetNameFromList(listItem);

				string t = listNameTypeItem[i].Name;

				listNameItem.Add(t + "");
				i++;

				listName.Add("\n");

				foreach (var name in listName)
				{
					listNameItem.Add(name);
				}

				listName.Add("\n");
			}

			return listNameItem;
		}
		private static List<string> GetNameFromList(IEnumerable<object> listObj)
		{
			List<string> listName = new List<string>();

			List<string> names = listObj.Select(obj => obj.GetType().GetProperty("Name")?.GetValue(obj, null) as string).ToList();

			return names;
		}

		public static List<IEnumerable<object>> GetAllItem(out List<Type> listNameTypeItem)
		{
			var listBlockpower = GetAllBlockpower(Cart);
			var listCPU = GetAllCPU(Cart);
			var listGPU = GetAllGPU(Cart);
			var listMemory = GetAllMemory();
			var listMotherboard = GetAllMotherboard(Cart);
			var listRAM = GetAllRAM(Cart);

			listNameTypeItem = new List<Type>()
			{
				typeof(Data_Blockpower),
				typeof(Data_CPU),
				typeof(Data_GPU),
				typeof(Data_Memory),
				typeof(Data_Motherboard),
				typeof(Data_RAM)
			};

			return new List<IEnumerable<object>>() { listBlockpower, listCPU, listGPU, listMemory, listMotherboard, listRAM };
		}

		public static List<IEnumerable<object>> GetAllItemWithoutCart(out List<Type> listNameTypeItem)
		{
			var listBlockpower = GetAllBlockpower();
			var listCPU = GetAllCPU();
			var listGPU = GetAllGPU();
			var listMemory = GetAllMemory();
			var listMotherboard = GetAllMotherboard();
			var listRAM = GetAllRAM();

			listNameTypeItem = new List<Type>()
			{
				typeof(Data_Blockpower),
				typeof(Data_CPU),
				typeof(Data_GPU),
				typeof(Data_Memory),
				typeof(Data_Motherboard),
				typeof(Data_RAM)
			};

			return new List<IEnumerable<object>>() { listBlockpower, listCPU, listGPU, listMemory, listMotherboard, listRAM };
		}

		public static List<IEnumerable<object>> GetAllItemByName(string name, out List<Type> listNameTypeItem)
		{
			var listBlockpower = GetAllBlockpower(Cart, name);
			var listCPU = GetAllCPU(Cart, name);
			var listGPU = GetAllGPU(Cart, name);
			var listMemory = GetAllMemory(name);
			var listMotherboard = GetAllMotherboard(Cart, name);
			var listRAM = GetAllRAM(Cart, name);

			listNameTypeItem = new List<Type>()
			{
				typeof(Data_Blockpower),
				typeof(Data_CPU),
				typeof(Data_GPU),
				typeof(Data_Memory),
				typeof(Data_Motherboard),
				typeof(Data_RAM)
			};

			return new List<IEnumerable<object>>() { listBlockpower, listCPU, listGPU, listMemory, listMotherboard, listRAM };
		}

		#endregion

		#region Worker

		public static bool AddNewValue(object item, out string resultStr)
		{
			string nameType = GetNameByType(item.GetType());
			string answer = "NOT success";

			if (CheckIsNull(item))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				string nameItem = item.GetType().GetProperty("Name")?.GetValue(item)?.ToString() ?? "";
				int[] lengthListsBefore = GetLengthAllListsFromDB();

				if (nameType == "Blockpower" && !db.DataBlockpowers.Any(o => o.Name == nameItem)) db.DataBlockpowers.Add(item as Data_Blockpower ?? new());
				if (nameType == "CPU" && !db.DataCPUs.Any(o => o.Name == nameItem)) db.DataCPUs.Add(item as Data_CPU ?? new());
				if (nameType == "GPU" && !db.DataGPUs.Any(o => o.Name == nameItem)) db.DataGPUs.Add(item as Data_GPU ?? new());
				if (nameType == "Memory" && !db.DataMemories.Any(o => o.Name == nameItem)) db.DataMemories.Add(item as Data_Memory ?? new());
				if (nameType == "Motherboard" && !db.DataMotherboards.Any(o => o.Name == nameItem)) db.DataMotherboards.Add(item as Data_Motherboard ?? new());
				if (nameType == "RAM" && !db.DataRAMs.Any(o => o.Name == nameItem)) db.DataRAMs.Add(item as Data_RAM ?? new());

				db.SaveChanges();

				if (IsElementsAdd(lengthListsBefore, GetLengthAllListsFromDB()))
				{
					answer = "SUCCESS";
					resultStr = $"Add new {nameType} is {answer}!";

					new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive), "Item added", Brushes.ForestGreen);

					return true;
				}
			}

			resultStr = $"Add new {nameType} is {answer}!";
			return false;
		}
		public static bool EditValue(object currentItem, object newItem, out string resultStr)
		{
			string nameType = GetNameByType(currentItem.GetType());
			string answer = "hase been CHANGED";

			if (CheckIsNull(newItem))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				int[] lengthListsBefore = GetLengthAllListsFromDB();


				DeleteValue(currentItem, out resultStr);

				db.SaveChanges();
			}

			using (PConfigureContext db = new())
			{

				string nameItem = currentItem.GetType().GetProperty("Name")?.GetValue(currentItem)?.ToString() ?? "";
				int[] lengthListsBefore = GetLengthAllListsFromDB();

				AddNewValue(newItem, out resultStr);

				db.SaveChanges();
			}

			new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive), "Item changed", Brushes.CadetBlue);
			resultStr = $"Edit {nameType} {answer}!";
			return false;
		}
		public static bool DeleteValue(object item, out string resultStr)
		{
			string nameType = GetNameByType(item.GetType());
			string answer = "is NOT exists";

			using (PConfigureContext db = new())
			{
				string nameItem = item.GetType().GetProperty("Name")?.GetValue(item)?.ToString() ?? "";
				int[] lengthListsBefore = GetLengthAllListsFromDB();

				if (nameType == "Blockpower" && db.DataBlockpowers.Contains(item as Data_Blockpower ?? new())) db.DataBlockpowers.Remove(item as Data_Blockpower ?? new());
				if (nameType == "CPU" && db.DataCPUs.Contains(item as Data_CPU ?? new())) db.DataCPUs.Remove(item as Data_CPU ?? new());
				if (nameType == "GPU" && db.DataGPUs.Contains(item as Data_GPU ?? new())) db.DataGPUs.Remove(item as Data_GPU ?? new());
				if (nameType == "Memory" && db.DataMemories.Contains(item as Data_Memory ?? new())) db.DataMemories.Remove(item as Data_Memory ?? new());
				if (nameType == "Motherboard" && db.DataMotherboards.Contains(item as Data_Motherboard ?? new())) db.DataMotherboards.Remove(item as Data_Motherboard ?? new());
				if (nameType == "RAM" && db.DataRAMs.Contains(item as Data_RAM ?? new())) db.DataRAMs.Remove(item as Data_RAM ?? new());


				db.SaveChanges();

				if (IsElementsAdd(lengthListsBefore, GetLengthAllListsFromDB()))
				{
					answer = "hase been DELETED";
					resultStr = $"Delete {nameType} is {answer}!";

					return true;
				}
			}

			resultStr = $"Delete {nameType} is {answer}!";
			return false;
		}

		#region Addition methods

		private static string GetNameByType(Type type)
		{
			var listMatch = new Regex(@"_\D+").Matches(type.ToString());
			return listMatch.Count > 0 ? listMatch[0].ToString().Replace("_", "") : "";
		}
		private static int[] GetLengthAllListsFromDB()
		{
			using (PConfigureContext db = new())
			{
				return new[]
					{
					db.DataBlockpowers.ToList().Count,
					db.DataCPUs.ToList().Count,
					db.DataGPUs.ToList().Count,
					db.DataMemories.ToList().Count,
					db.DataMotherboards.ToList().Count,
					db.DataRAMs.ToList().Count,
				};
			}
		}
		private static bool IsElementsAdd(int[] lengthListsBefore, int[] lengthListsAfter)
		{
			for (int i = 0; i < lengthListsBefore.Length; i++)
			{
				if (lengthListsBefore[i] != lengthListsAfter[i]) return true;
			}

			return false;
		}

		#endregion

		#endregion


		#region Get Blockpower

		public static List<Data_Blockpower> GetAllBlockpower(Cart cart, string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataBlockpowers.ToList().Where(o => o.Name.Contains(name) && cart.CheckTypePower(o)).ToList();
			}
		}

		public static List<Data_Blockpower> GetAllBlockpower(Cart Cart)
		{
			using (PConfigureContext db = new())
			{
				return db.DataBlockpowers.ToList().Where(o => Cart.CheckTypePower(o)).ToList();
			}
		}

		public static List<Data_Blockpower> GetAllBlockpower()
		{
			using (PConfigureContext db = new())
			{
				return db.DataBlockpowers.ToList();
			}
		}

		#endregion

		#region Get CPU

		public static List<Data_CPU> GetAllCPU(Cart cart, string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataCPUs.ToList().Where(o => o.Name.Contains(name) && cart.CheckSocket(o)).ToList();
			}
		}

		public static List<Data_CPU> GetAllCPU(Cart Cart)
		{
			using (PConfigureContext db = new())
			{
				return db.DataCPUs.ToList().Where(o => Cart.CheckSocket(o)).ToList();
			}
		}

		public static List<Data_CPU> GetAllCPU()
		{
			using (PConfigureContext db = new())
			{
				return db.DataCPUs.ToList();
			}
		}


		#endregion

		#region Get GPU

		public static List<Data_GPU> GetAllGPU(Cart cart, string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataGPUs.ToList().Where(o => o.Name.Contains(name) && cart.CheckTypeGDDR(o)).ToList();
			}
		}

		public static List<Data_GPU> GetAllGPU(Cart Cart)
		{
			using (PConfigureContext db = new())
			{
				return db.DataGPUs.ToList().Where(o => Cart.CheckTypePower(o) && Cart.CheckTypeGDDR(o)).ToList();
			}
		}

		public static List<Data_GPU> GetAllGPU()
		{
			using (PConfigureContext db = new())
			{
				return db.DataGPUs.ToList();
			}
		}


		#endregion

		#region Get Memory

		public static List<Data_Memory> GetAllMemory(string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataMemories.ToList().Where(o => o.Name.Contains(name)).ToList();
			}
		}

		public static List<Data_Memory> GetAllMemory()
		{
			using (PConfigureContext db = new())
			{
				return db.DataMemories.ToList();
			}
		}

		#endregion

		#region Get MotherBoard

		public static List<Data_Motherboard> GetAllMotherboard(Cart cart, string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataMotherboards.ToList().Where(o => o.Name.Contains(name) && cart.CheckSocket(o) && cart.CheckTypeGDDR(o) && cart.CheckTypeDDR(o)).ToList();
			}
		}

		public static List<Data_Motherboard> GetAllMotherboard(Cart Cart)
		{
			using (PConfigureContext db = new())
			{
				return db.DataMotherboards.ToList().Where(o => Cart.CheckSocket(o) && Cart.CheckTypeGDDR(o) && Cart.CheckTypeDDR(o)).ToList();
			}
		}
		public static List<Data_Motherboard> GetAllMotherboard()
		{
			using (PConfigureContext db = new())
			{
				return db.DataMotherboards.ToList();
			}
		}

		#endregion

		#region Get RAM

		public static List<Data_RAM> GetAllRAM(Cart cart, string name)
		{
			using (PConfigureContext db = new())
			{
				return db.DataRAMs.ToList().Where(o => o.Name.Contains(name) && cart.CheckTypeDDR(o)).ToList();
			}
		}

		public static List<Data_RAM> GetAllRAM(Cart Cart)
		{
			using (PConfigureContext db = new())
			{
				return db.DataRAMs.ToList().Where(o => Cart.CheckTypeDDR(o)).ToList();
			}
		}

		public static List<Data_RAM> GetAllRAM()
		{
			using (PConfigureContext db = new())
			{
				return db.DataRAMs.ToList();
			}
		}


		#endregion
	}
}
