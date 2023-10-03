using PConfigure.View;
using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace PConfigure.Model
{
	class MainM
	{
		#region BurgerMenu worker

		private static bool change = true;
		private static bool burgerMenuIsShow = false;

		public static void ShowBurgerMenu(MainWindow currentMainWindow)
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
		public static void HideBurgerMenu(MainWindow currentMainWindow)
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

		public static void ExpandedBurgerMenu(object o)
		{
			MainWindow currentMainWindow = o as MainWindow ?? new();

			if (change)
			{
				ShowBurgerMenu(currentMainWindow);
			}
			else
			{
				HideBurgerMenu(currentMainWindow);
			}
		}

		public static void HideAfterLostFocusBurgerMenu(object o)
		{
			MainWindow currentMainWindow = o as MainWindow ?? new MainWindow();

			HideBurgerMenu(currentMainWindow);
		}

		#endregion
	}
}
