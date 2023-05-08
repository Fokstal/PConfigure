using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using PConfigure.View;
using PConfigure.View.MainWindowContentPage;
using System.Windows.Input;

namespace PConfigure.ViewModel
{
	class MainWindowVM
	{
		private static bool change = true;
		private static bool burgerMenuIsShow = false;

		private readonly RelayCommand _expandedBurgerMenuCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			if (change)
			{
				ShowBurgerMenu(currentMainWindow);
			}
			else
			{
				HideBurgerMenu(currentMainWindow);
			}
		});
		private readonly RelayCommand _hideBurgerMenuCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			HideBurgerMenu(currentMainWindow);
		});

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
		private readonly RelayCommand _showConfigurePageCmd = new(o =>
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			currentMainWindow.ContentFrame.Content = new ConfigurePage();
		});


		public RelayCommand ExpendedBurgerMenuCmd { get => _expandedBurgerMenuCmd; }
		public RelayCommand HideBurgerMenuCmd { get => _hideBurgerMenuCmd; }

		public RelayCommand ShowInformationPageCmd { get => _showInformationPageCmd; }
		public RelayCommand ShowCatalogPageCmd { get => _showCatalogPageCmd; }
		public RelayCommand ShowConfigurePageCmd { get => _showConfigurePageCmd; }

		//Methods
		private static void ShowBurgerMenu(MainWindow currentMainWindow)
		{
			change = false;
			burgerMenuIsShow = true;

			DoubleAnimation buttonAnimation = new DoubleAnimation();
			buttonAnimation.From = currentMainWindow.BurgerMenu.ActualWidth;
			buttonAnimation.To = 200;
			buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
			currentMainWindow.BurgerMenu.BeginAnimation(Button.WidthProperty, buttonAnimation);

			currentMainWindow.BtnListStackpanel.BeginAnimation(FrameworkElement.MarginProperty,
				new ThicknessAnimation
				{
					From = new(0, 0, 100, 0),
					To = new(0, 0, 0, 0),
					Duration = TimeSpan.FromSeconds(0.5)
				});
			currentMainWindow.BtnListStackpanel.Visibility = Visibility.Visible;

			currentMainWindow.BurgerMenuBtn.RenderTransform.BeginAnimation(RotateTransform.AngleProperty,
				new DoubleAnimation
				{
					From = 0,
					To = 180,
					Duration = TimeSpan.FromSeconds(0.7)
				});
		}
		private static void HideBurgerMenu(MainWindow currentMainWindow)
		{
			if (burgerMenuIsShow)
			{
				change = true;

				DoubleAnimation buttonAnimation = new DoubleAnimation();
				buttonAnimation.From = currentMainWindow.BurgerMenu.ActualWidth;
				buttonAnimation.To = 50;
				buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);

				currentMainWindow.BurgerMenu.BeginAnimation(Button.WidthProperty, buttonAnimation);

				currentMainWindow.BtnListStackpanel.BeginAnimation(FrameworkElement.MarginProperty,
					new ThicknessAnimation
					{
						From = new(0, 0, 0, 0),
						To = new(0, 0, 100, 0),
						Duration = TimeSpan.FromSeconds(0.5)
					});

				currentMainWindow.BtnListStackpanel.Visibility = Visibility.Visible;



				currentMainWindow.BurgerMenuBtn.RenderTransform.BeginAnimation(RotateTransform.AngleProperty,
					new DoubleAnimation
					{
						From = 180,
						To = 0,
						Duration = TimeSpan.FromSeconds(0.4)
					});

				burgerMenuIsShow = false;
			}
		}


	}
}
