using System.Collections.Generic;
using System.Linq;
using PConfigure.Data;

namespace PConfigure.Model
{
	internal class DataWorker
	{
		private static bool CheckIsNull(params string?[] listArg)
		{
			foreach (var arg in listArg) if (arg is null) return true;

			return false;
		}


		#region BlockPower

		private static readonly string codeBlockpower = "BLOCKPOWER";

		public static bool AddNewValue(out string resultStr, string? name, int capacityPower, double CUA, int typeGPUPower)
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

					db.DataBlockpowers.Add(new Data_Blockpower() { Name = name, CapacityPower = capacityPower, CUA = CUA, TypeGPUPower = typeGPUPower });
					db.SaveChanges();

					return true;
				}

				return false;
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

		public static bool AddNewValue(out string resultStr, string? model, string? modelName, string? socket, int frequency, int core, int cash, int TDP, double Price)
		{
			resultStr = $"Add new {codeCPU} is NOT success";

			if (CheckIsNull(model, modelName, socket))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.DataCPUs.Any(o => o.ModelName == modelName);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeCPU} is SUCCESS";

					db.DataCPUs.Add(new Data_CPU() { Model = model, ModelName = modelName, Cash = cash, Core = core, Frequency = frequency, Price = Price, Socket = socket, TDP = TDP });
					db.SaveChanges();

					return true;
				}

				return false;
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

		public static bool AddNewValue(out string resultStr, string? name, int frequency, int capacityPower, int typeDDR, int typePower, int TDP, double price)
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

					db.DataGPUs.Add(new Data_GPU() { Name = name, CapacityMemory = capacityPower, Frequency = frequency, TDP = TDP, Price = price, TypeDDR = typeDDR, TypePower = typePower });
					db.SaveChanges();

					return true;
				}

				return false;
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

		public static bool AddNewValue(out string resultStr, string? name, string? type, int capacityMemory, string? typeConnect, int speed)
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

					db.DataMemories.Add(new Data_Memory() { Name = name, Type = type, CapacityMemory = capacityMemory, TypeConnect = typeConnect, Speed = speed });
					db.SaveChanges();

					return true;
				}

				return false;
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


		#region Motherboard
		//Motherboard

		private static readonly string codeMotherboard = "MOTHERBOARD";

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

		public static bool AddNewValue(out string resultStr, string? name, int frequency, int typeDDR, int capacityPower, int TDP, double price)
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

					db.DataRAMs.Add(new Data_RAM() { Name = name, Frequency = frequency, TypeDDR = typeDDR, CapacityMemory = capacityPower, TDP = TDP, Price = price });
					db.SaveChanges();

					return true;
				}

				return false;
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
