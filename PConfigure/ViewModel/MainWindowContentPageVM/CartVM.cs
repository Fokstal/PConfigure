using PConfigure.Addition;
using PConfigure.Model;
using PConfigure.Model.ModelData;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class CartVM
	{
		private static Cart currentCart = new();

		private List<CartItem> _listCartItem = CartM.GetCartItem(currentCart);

		private RelayCommand _command = new(o => { CatalogM.CartPage = o as CartPage ?? new(); });

		private double _priceValue = currentCart.GetPrices();

		private RelayCommand _deleteItemFromCartCmd = new(o =>
		{
			MessageBox.Show(o + "");

		});


		public static Cart CurrentCart { get => currentCart; set => currentCart = value; }

		public List<CartItem> ListCartItem { get => _listCartItem; set => _listCartItem = value; }

		public RelayCommand Command { get => _command; set => _command = value; }

		public double Pricevalue { get => _priceValue; }

		public RelayCommand DeleteItemFromCart { get => _deleteItemFromCartCmd; }
	}
}
