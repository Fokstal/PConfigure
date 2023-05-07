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

namespace PConfigure.ViewModel
{
	class MainWindowVM
	{
		private static bool change = true;

		private readonly RelayCommand _expandedBurgerMenuCmd = new(o =>
		{
			MainWindow cMW = o as MainWindow ?? new MainWindow();

			if (change)
			{
				change = false;

				DoubleAnimation buttonAnimation = new DoubleAnimation();
				buttonAnimation.From = cMW.BurgerMenu.ActualWidth;
				buttonAnimation.To = 200;
				buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
				cMW.BurgerMenu.BeginAnimation(Button.WidthProperty, buttonAnimation);

				cMW.InformationTextBlock.Visibility = Visibility.Visible;
				cMW.CatalogTextBlock.Visibility = Visibility.Visible;
				cMW.ConfigureTextBlock.Visibility = Visibility.Visible;

				cMW.BurgerMenuBtn.RenderTransform.BeginAnimation(RotateTransform.AngleProperty,
					new DoubleAnimation
					{
						From = 0,
						To = 180,
						Duration = TimeSpan.FromSeconds(0.7)
					});
			}
			else
			{
				change = true;

				DoubleAnimation buttonAnimation = new DoubleAnimation();
				buttonAnimation.From = cMW.BurgerMenu.ActualWidth;
				buttonAnimation.To = 50;
				buttonAnimation.Duration = TimeSpan.FromSeconds(0.5);
				cMW.BurgerMenu.BeginAnimation(Button.WidthProperty, buttonAnimation);

				cMW.InformationTextBlock.Visibility = Visibility.Hidden;
				cMW.CatalogTextBlock.Visibility = Visibility.Hidden;
				cMW.ConfigureTextBlock.Visibility = Visibility.Hidden;

				cMW.BurgerMenuBtn.RenderTransform.BeginAnimation(RotateTransform.AngleProperty,
					new DoubleAnimation
					{
						From = 180,
						To = 0,
						Duration = TimeSpan.FromSeconds(0.4)
					});
			}
		});


		public RelayCommand ExpendedBurgerMenuCmd { get => _expandedBurgerMenuCmd; }
	}
}
