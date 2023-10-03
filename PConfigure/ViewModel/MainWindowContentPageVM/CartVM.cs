using PConfigure.Addition;
using PConfigure.Model;
using PConfigure.Model.ModelData;
using PConfigure.View;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class CartVM : INotifyPropertyChanged
	{
		private static Cart currentCart = new();

		private List<CartItem> _listCartItem = CartM.GetCartItem(currentCart);

		private RelayCommand _command = new(o => { Cart.currentCartPage = o as CartPage ?? new(); });

		private double _priceValue = currentCart.GetPrices();

		private readonly RelayCommand _deleteItemFromCartCmd = new(o =>
		{
			CartM.DeleteItemFromCart(currentCart, o.ToString());

			Cart.currentCartPage.ListItem.ItemsSource = CartM.GetCartItem(currentCart);

			CatalogVM.SetCartList_Item();

			Cart.currentCartPage.PriceTextBlock.Text = CartVM.CurrentCart.GetPrices().ToString();

			new MessageAlarmVM().Open(Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive), "Item deleted", Brushes.DarkOrange);
		});


		public static Cart CurrentCart { get => currentCart; set => currentCart = value; }

		public List<CartItem> ListCartItem { get => _listCartItem; set => _listCartItem = value; }

		public RelayCommand Command { get => _command; set => _command = value; }

		public double Pricevalue { get => _priceValue; }

		public RelayCommand DeleteItemFromCart { get => _deleteItemFromCartCmd; }

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged is not null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			}

		}

		#endregion
	}
}
