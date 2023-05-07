using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PConfigure.View;

namespace PConfigure.ViewModel
{
	internal class StartupWindowVM
	{
		private static StartupWindow? currentStartupWindow;

		public static bool ChoiceAccount = false;

		private static readonly RelayCommand _closeWindowCmd = new(o =>
		{
			StartupWindow currentWnd = o as StartupWindow ?? new StartupWindow();
			currentWnd.Close();
		});

		private static readonly RelayCommand _openSignInCreatorPage = new(o =>
		{
			currentStartupWindow = o as StartupWindow ?? new StartupWindow();

			if (ChoiceAccount)
			{
				currentStartupWindow.ContentFrame.Content = new SignInCreatorPage();

				currentStartupWindow.ContinueBtn.Command = _closeWindowCmd;

				ChoiceAccount = false;
			}
		});

		private readonly RelayCommand _changeVisibilityPass = new(o =>
		{
			SignInCreatorPage currentPage = o as SignInCreatorPage ?? new SignInCreatorPage();

			PasswordBox passBox = currentPage.PasswordBox;
			TextBox textBox = currentPage.PasswordTextBox;

			if (passBox.Visibility == Visibility.Visible)
			{
				passBox.Visibility = Visibility.Hidden;
				textBox.Visibility = Visibility.Visible;

				textBox.Text = passBox.Password;

				return;
			}

			if (textBox.Visibility == Visibility.Visible)
			{
				textBox.Visibility = Visibility.Hidden;
				passBox.Visibility = Visibility.Visible;

				passBox.Password = textBox.Text;

				return;
			}
		});

		private readonly RelayCommand _openStartupPage = new(o =>
		{
			if (currentStartupWindow is not null)
			{
				currentStartupWindow.ContentFrame.Content = new StartupPage();

				currentStartupWindow.ContinueBtn.Command = _openSignInCreatorPage;
			}
		});

		public RelayCommand CloseWindowCmd { get => _closeWindowCmd; }
		public RelayCommand OpenSignInCreatorPage { get => _openSignInCreatorPage; }
		public RelayCommand OpenStartupPage { get => _openStartupPage; }
		public RelayCommand ChangeVisibilityPass { get => _changeVisibilityPass; }

		//Select account methods
		public RelayCommand SelectUserAccCmd { get => new RelayCommand(o => 
		{
			StartupPage currentPage = o as StartupPage ?? new StartupPage();
			currentPage.SelectUserAccBtn.IsEnabled = false;
			currentPage.SelectCreatorAccBtn.IsEnabled = true;

			ChoiceAccount = false; 
		}); }
		public RelayCommand SelectCreatorAccCmd
		{
			get => new RelayCommand(o =>
			{
				StartupPage currentPage = o as StartupPage ?? new StartupPage();
				currentPage.SelectCreatorAccBtn.IsEnabled = false;
				currentPage.SelectUserAccBtn.IsEnabled = true;

				ChoiceAccount = true;
			});
		}
	}
}
