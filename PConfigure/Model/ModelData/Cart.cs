using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PConfigure.Model.ModelData
{
	class Cart
	{
		public Data_Blockpower? Blockpower { get; set; }
		public Data_CPU? CPU { get; set; }
		public Data_GPU? GPU { get; set; }
		public Data_Memory? Memory { get; set; }
		public Data_Motherboard? Motherboard { get; set; }
		public Data_RAM? RAM { get; set; }

		#region Current CartPage

		public static CartPage? currentCartPage;

		#endregion

		#region Validation of DEPEND

		public bool CheckSocket(Data_Motherboard motherboard)
		{
			if (CPU is null) return true;

			return CPU?.Socket == motherboard.Socket.Replace(" ", "");
		}
		public bool CheckSocket(Data_CPU cpu)
		{
			if (Motherboard is null) return true;

			return cpu.Socket == Motherboard?.Socket.Replace(" ", "");
		}
		public bool CheckTypeDDR(Data_Motherboard motherboard)
		{
			if (RAM is null) return true;

			return RAM?.TypeDDR == motherboard.TypeDDR;
		}
		public bool CheckTypeDDR(Data_RAM ram)
		{
			if (Motherboard is null) return true;

			return ram.TypeDDR == Motherboard?.TypeDDR;
		}
		public bool CheckTypeGDDR(Data_Motherboard motherboard)
		{
			if (GPU is null) return true;

			return GPU?.TypeGDDR == motherboard.TypeGDDR;
		}
		public bool CheckTypeGDDR(Data_GPU gpu)
		{
			if (Motherboard is null) return true;

			return gpu.TypeGDDR == Motherboard?.TypeGDDR;
		}
		public bool CheckTypePower(Data_Blockpower blockpower)
		{
			if (GPU is null) return true;

			return GPU?.TypePower == blockpower.TypeGPUPower;
		}
		public bool CheckTypePower(Data_GPU gpu)
		{
			if (Blockpower is null) return true;

			return gpu.TypePower == Blockpower?.TypeGPUPower;
		}

		#endregion

		public bool CheckEqualTDP()
		{
			return CPU?.TDP + GPU?.TDP + RAM?.TDP <= Blockpower?.CapacityPower;
		}
		public double GetPrices()
		{
			double prices = 0;

			if (Blockpower is not null)
			{
				prices += Blockpower.Price;
			}

			if (CPU is not null)
			{
				prices += CPU.Price;
			}

			if (GPU is not null)
			{
				prices += GPU.Price;
			}

			if (Memory is not null)
			{
				prices += Memory.Price;
			}

			if (Motherboard is not null)
			{
				prices += Motherboard.Price;
			}

			if (RAM is not null)
			{
				prices += RAM.Price;
			}

			return prices;
		}
	}
}
