using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PConfigure.Data;
using PConfigure.Model.ModelData;

namespace PConfigure.Model
{
	internal class DataWorker
	{
		private static bool CheckIsNull(params string?[] listArg)
		{
			foreach (var arg in listArg) if (arg is null) return true;

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

		#endregion

		// !PROBLEM -> Very more Query to DB

		#region BlockPower

		private static readonly string codeBlockpower = "BLOCKPOWER";

		public static bool AddNewValue_Blockpower(out string resultStr, List<object> listParam)
		{
			resultStr = $"Add new {codeBlockpower} is NOT success";

			if (listParam.Count == 5)
			{
				string? name = listParam[0] as String;
				int capacityPower = Convert.ToInt32(listParam[1]);
				double CUA = Convert.ToDouble(listParam[2]);
				int typeGPUPower = Convert.ToInt32(listParam[3]);
				double price = Convert.ToDouble(listParam[4]);

				if (CheckIsNull(name))
				{
					resultStr = $"Your data has a NULL values!";

					return false;
				}

				using (PConfigureContext db = new())
				{
					bool checkIsExist = db.DataBlockpowers.Any(o => o.Name == name);

					if (!checkIsExist)
					{
						resultStr = $"Add new {codeBlockpower} is SUCCESS";

						return true;
					}
				}
			}

			return false;
		}


		#region Worker
		public static bool AddNewValue(out string resultStr, string? name, int capacityPower, double CUA, int typeGPUPower, double price)
		{
			resultStr = $"Add new {codeBlockpower} is NOT success";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataBlockpowers.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeBlockpower} is SUCCESS";

					db.DataBlockpowers.Add(new Data_Blockpower() { Name = name, CapacityPower = capacityPower, CUA = CUA, TypeGPUPower = typeGPUPower, Price = price });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_Blockpower value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeBlockpower} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataBlockpowers.Contains(value); ;

				if (checkIsExist)
				{
					db.DataBlockpowers.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeBlockpower} hase been delete";
					return true;
				}
			}

			return false;
		}

		public static bool EditValue(out string resultStr, Data_Blockpower oldValue, string? name, int capacityPower, double CUA, int typeGPUPower, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeBlockpower} is NOT exists";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_Blockpower? value = db.DataBlockpowers.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeBlockpower} has been CHANGED!";

					value.Name = name;
					value.CapacityPower = capacityPower;
					value.CUA = CUA;
					value.TypeGPUPower = typeGPUPower;
					value.Price = price;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		#endregion

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


		#region CPU
		// CPU

		private static readonly string codeCPU = "CPU";

		#region Worker
		public static bool AddNewValue(out string resultStr, string? model, string? name, string? socket, double frequency, int core, int cash, int TDP, double price)
		{
			resultStr = $"Add new {codeCPU} is NOT success";

			if (CheckIsNull(model, name, socket))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataCPUs.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeCPU} is SUCCESS";

					db.DataCPUs.Add(new Data_CPU() { Model = model, Name = name, Cash = cash, Core = core, Frequency = frequency, Price = price, Socket = socket, TDP = TDP });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_CPU value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeCPU} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataCPUs.Contains(value); ;

				if (checkIsExist)
				{
					db.DataCPUs.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeCPU} hase been delete";
					return true;
				}
			}

			return false;
		}

		public static bool EditValue(out string resultStr, Data_CPU oldValue, string? model, string? name, string? socket, double frequency, int core, int cash, int TDP, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeCPU} is NOT exists";

			if (CheckIsNull(model, name, socket))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_CPU? value = db.DataCPUs.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeCPU} has been CHANGED!";

					value.TDP = TDP;
					value.Price = price;
					value.Frequency = frequency;
					value.Model = model;
					value.Name = name;
					value.Cash = cash;
					value.Core = core;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		#endregion

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


		#region GPU
		//GPU

		private static readonly string codeGPU = "GPU";

		#region Worker

		public static bool AddNewValue(out string resultStr, string? name, int frequency, int capacityMemory, int typeDDR, int typePower, int TDP, double price)
		{
			resultStr = $"Add new {codeGPU} is NOT success";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataGPUs.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeGPU} is SUCCESS";

					db.DataGPUs.Add(new Data_GPU() { Name = name, CapacityMemory = capacityMemory, Frequency = frequency, TDP = TDP, Price = price, TypeGDDR = typeDDR, TypePower = typePower });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool EditValue(out string resultStr, Data_GPU oldValue, string? name, int frequency, int capacityMemory, int typeDDR, int typePower, int TDP, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeGPU} is NOT exists";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_GPU? value = db.DataGPUs.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeGPU} has been CHANGED!";

					value.Name = name;
					value.Frequency = frequency;
					value.CapacityMemory = capacityMemory;
					value.TypeGDDR = typeDDR;
					value.TypePower = typePower;
					value.TDP = TDP;
					value.Price = price;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_GPU value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeGPU} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataGPUs.Contains(value); ;

				if (checkIsExist)
				{
					db.DataGPUs.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeGPU} hase been delete";
					return true;
				}
			}

			return false;
		}

		#endregion

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


		#region Memory
		//Memory

		private static readonly string codeMemory = "Memory";

		#region
		public static bool AddNewValue(out string resultStr, string? name, string? type, int capacityMemory, string? typeConnect, int speed, double price)
		{
			resultStr = $"Add new {codeMemory} is NOT success";

			if (CheckIsNull(name, type, typeConnect))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataMemories.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeMemory} is SUCCESS";

					db.DataMemories.Add(new Data_Memory() { Name = name, Type = type, CapacityMemory = capacityMemory, TypeConnect = typeConnect, Speed = speed, Price = price });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_Memory value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeMemory} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataMemories.Contains(value); ;

				if (checkIsExist)
				{
					db.DataMemories.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeMemory} hase been delete";
					return true;
				}
			}

			return false;
		}

		public static bool EditValue(out string resultStr, Data_Memory oldValue, string? name, string? type, int capacityMemory, string? typeConnect, int speed, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeMemory} is NOT exists";

			if (CheckIsNull(name, type, typeConnect))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_Memory? value = db.DataMemories.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeMemory} has been CHANGED!";

					value.Name = name;
					value.Type = type;
					value.CapacityMemory = capacityMemory;
					value.TypeConnect = typeConnect;
					value.Speed = speed;
					value.Price = price;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		#endregion

		public static List<Data_Memory> GetAllMemory()
		{
			using (PConfigureContext db = new())
			{
				return db.DataMemories.ToList();
			}
		}

		#endregion


		#region Motherboard
		//Motherboard

		private static readonly string codeMotherboard = "MOTHERBOARD";

		#region Worker
		public static bool AddNewValue(out string resultStr, string? name, string? typeATX, string? socket, string? chipset, string? typeDDR, string? typeGDDR, int countSATA3, int countM2, double price)
		{
			resultStr = $"Add new {codeMotherboard} is NOT success";

			if (CheckIsNull(name, typeATX, socket, chipset, typeDDR, typeGDDR))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataMotherboards.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeMotherboard} is SUCCESS";

					db.DataMotherboards.Add(new Data_Motherboard() { Name = name, TypeATX = typeATX, Socket = socket, Chipset = chipset, TypeDDR = typeDDR, TypeGDDR = typeGDDR, CountM2 = countM2, CountSATA3 = countSATA3, Price = price });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_Motherboard value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeMotherboard} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataMotherboards.Contains(value); ;

				if (checkIsExist)
				{
					db.DataMotherboards.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeMotherboard} hase been delete";
					return true;
				}
			}

			return false;
		}

		public static bool EditValue(out string resultStr, Data_Motherboard oldValue, string? name, string? typeATX, string? socket, string? chipset, string? typeDDR, string? typeGDDR, int countSATA3, int countM2, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeMotherboard} is NOT exists";

			if (CheckIsNull(name, typeATX, socket, chipset, typeDDR, typeGDDR))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_Motherboard? value = db.DataMotherboards.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeMotherboard} has been CHANGED!";

					value.Name = name;
					value.TypeATX = typeATX;
					value.Socket = socket;
					value.Chipset = chipset;
					value.TypeDDR = typeDDR;
					value.TypeGDDR = typeGDDR;
					value.CountSATA3 = countSATA3;
					value.CountM2 = countM2;
					value.Price = price;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}


		#endregion

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


		#region RAM
		//RAM

		private static readonly string codeRAM = "RAM";

		#region
		public static bool AddNewValue(out string resultStr, string? name, int frequency, int typeDDR, int capacityMemory, double TDP, double price)
		{
			resultStr = $"Add new {codeRAM} is NOT success";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataRAMs.Any(o => o.Name == name);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeRAM} is SUCCESS";

					db.DataRAMs.Add(new Data_RAM() { Name = name, Frequency = frequency, TypeDDR = typeDDR, CapacityMemory = capacityMemory, TDP = TDP, Price = price });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		public static bool DeleteValue(Data_RAM value, out string resultStr)
		{
			resultStr = $"This {value.Name} in {codeRAM} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataRAMs.Contains(value); ;

				if (checkIsExist)
				{
					db.DataRAMs.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Name} in {codeRAM} hase been delete";
					return true;
				}
			}

			return false;
		}

		public static bool EditValue(out string resultStr, Data_RAM oldValue, string? name, int frequency, int typeDDR, int capacityMemory, double TDP, double price)
		{
			resultStr = $"This {oldValue.Name} in {codeRAM} is NOT exists";

			if (CheckIsNull(name))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Data_RAM? value = db.DataRAMs.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Name} in {codeRAM} has been CHANGED!";

					value.Name = name;
					value.Frequency = frequency;
					value.TypeDDR = typeDDR;
					value.CapacityMemory = capacityMemory;
					value.Price = price;
					value.TDP = TDP;

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}

		#endregion

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
