using PConfigure.View;
using PConfigure.View.MainWindowContentPage;
using PConfigure.Model;
using PConfigure.Addition;

namespace PConfigure.ViewModel
{
	class MainVM
	{
		#region Load Connect to DB

		private RelayCommand _loadConnectToDBCmd = new(o => DataWorker.GetNameAllItem());

		public RelayCommand LoadConnectToDBCmd { get => _loadConnectToDBCmd; set => _loadConnectToDBCmd = value; }

		#endregion


		#region Basket VM

		public static BasketWindow currentBasketWnd;

		private readonly RelayCommand _openBasketCmd = new(o =>
		{
			currentBasketWnd = new();
			currentBasketWnd.Show();
		});

		public RelayCommand OpenBasketCmd { get => _openBasketCmd; }

		#endregion


		#region BurgerMenu VM

		private readonly RelayCommand _expandedBurgerMenuCmd = new(o => MainM.ExpandedBurgerMenu(o));
		private readonly RelayCommand _hideBurgerMenuCmd = new(o => MainM.HideBurgerMenu(o));

		public RelayCommand ExpendedBurgerMenuCmd { get => _expandedBurgerMenuCmd; }
		public RelayCommand HideBurgerMenuCmd { get => _hideBurgerMenuCmd; }

		#endregion


		#region Btn BurgerMenu VM

		private readonly RelayCommand _showInformationPageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new InformationPage();
		});
		private readonly RelayCommand _showCatalogPageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new CatalogPage();
		});

		public RelayCommand ShowInformationPageCmd { get => _showInformationPageCmd; }
		public RelayCommand ShowCatalogPageCmd { get => _showCatalogPageCmd; }

		#endregion
	}
}
