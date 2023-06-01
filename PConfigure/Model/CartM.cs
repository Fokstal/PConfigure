using PConfigure.Model.ModelData;
using PConfigure.ViewModel.MainWindowContentPageVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PConfigure.Model
{
	class CartM
	{
		private static Dictionary<string, string> iconToItem = new()
		{
			{ "Blockpower", "⚡️" },
			{ "CPU", "⚙️" },
			{ "GPU", "🌄" },
			{ "Memory", "💽" },
			{ "Motherboard", "📻" },
			{ "RAM", "💡" }
		};

		public static List<CartItem> GetCartItem(Cart Cart)
		{
			List<CartItem> CartItems = new();

			foreach (var e in Cart.GetType().GetProperties())
			{
				if (e.GetValue(Cart) is not null)
				{
					var item = Cart.GetType().GetProperty(e.Name)?.GetValue(Cart);

					string nameField = item?.ToString().Replace("PConfigure.Model.Data_", "") ?? "";

					foreach (var field in iconToItem)
					{
						if (nameField == field.Key)
						{
							nameField = field.Value + " " + nameField;
						}
					}

					CartItems.Add(new() 
					{ 
						NameField = nameField, 
						NameItem = item.GetType().GetProperty("Name")?.GetValue(item)?.ToString()
					});
				}
			}

			return CartItems;
		}
	}
}
