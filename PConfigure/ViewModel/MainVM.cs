using PConfigure.View;
using PConfigure.View.MainWindowContentPage;
using PConfigure.Model;
using PConfigure.Addition;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System;

namespace PConfigure.ViewModel
{
	class MainVM
	{
		#region Command main

		private RelayCommand _loadConnectToDBCmd = new(o =>
		{
			DataWorker.GetNameAllItem();

			_showInformationPageCmd.Execute(o);
		});
		private readonly RelayCommand _exitWindowCmd = new(o =>
		{
			MainWindow mainWindow = o as MainWindow ?? new();

			new StartupWindow().Show();

			mainWindow.Close();
		});

		public RelayCommand LoadConnectToDBCmd { get => _loadConnectToDBCmd; set => _loadConnectToDBCmd = value; }
		public RelayCommand ExitWindowCmd { get => _exitWindowCmd; }
		#endregion


		#region Cart VM

		private static bool visibleCart = false;

		private readonly RelayCommand _openCartCmd = new(o =>
		{
			MainWindow mainWindow = o as MainWindow ?? new();

			var CartFrame = mainWindow.CartFrame;

			if (visibleCart)
			{
				AnimatingCart(CartFrame, 0);

				visibleCart = false;
			}
			else
			{
				AnimatingCart(CartFrame, 300);

				visibleCart = true;
			}
		});

		public RelayCommand OpenCartCmd { get => _openCartCmd; }

		private static void AnimatingCart(Frame CartFrame, int endWidth)
		{
			DoubleAnimation frameAnimation = new DoubleAnimation();
			frameAnimation.From = CartFrame.ActualWidth;
			frameAnimation.To = endWidth;
			frameAnimation.Duration = TimeSpan.FromSeconds(0.3);

			CartFrame.BeginAnimation(Frame.WidthProperty, frameAnimation);
		}

		#endregion


		#region BurgerMenu VM

		private readonly RelayCommand _expandedBurgerMenuCmd = new(o => MainM.ExpandedBurgerMenu(o));
		private readonly RelayCommand _hideBurgerMenuCmd = new(o => MainM.HideAfterLostFocusBurgerMenu(o));

		public RelayCommand ExpendedBurgerMenuCmd { get => _expandedBurgerMenuCmd; }
		public RelayCommand HideBurgerMenuCmd { get => _hideBurgerMenuCmd; }

		#endregion


		#region Btn BurgerMenu VM

		private static readonly RelayCommand _showInformationPageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new InformationPage();

		});
		private readonly RelayCommand _showCatalogPageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new CatalogPage();
		});

		private readonly RelayCommand _showAdminPanelPageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new AdminPanelPage();
		});

		public RelayCommand ShowInformationPageCmd { get => _showInformationPageCmd; }
		public RelayCommand ShowCatalogPageCmd { get => _showCatalogPageCmd; }
		public RelayCommand ShowAdminPanelPageCmd { get => _showAdminPanelPageCmd; }

		#endregion
	}
}
