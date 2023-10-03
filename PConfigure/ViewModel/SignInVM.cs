using PConfigure.View;
using System.Windows;
using PConfigure.Model;
using System.Windows.Controls;
using PConfigure.Addition;
using System.Windows.Threading;
using System;
using PConfigure.Model.ModelData;

namespace PConfigure.ViewModel
{
    class SignInVM
	{
		#region SignIn VM

		public static StartupWindow? currentStartupWindow;

		public static string? Password { get; set; }


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

		// NoName - passss <- LOGIN AND PASS
		public static readonly RelayCommand signInAccount = new(o =>
		{
			SignInCreatorPage currentPage = o as SignInCreatorPage ?? new();

			string? resultStr = null;

			string passwordFromPasswordBox = currentPage.PasswordBox.Password ?? "";
			string passwordFromPasswordTextBox = Password ?? "";

			string currentPassword = passwordFromPasswordBox.Length > passwordFromPasswordTextBox.Length ? passwordFromPasswordBox : passwordFromPasswordTextBox;

			if (AccountWorker.SignInAccout(out resultStr, "NoName", currentPassword))
			{
				MainWindow mainWindow = new MainWindow();

				mainWindow.AdminPanelTextBlock.Visibility = Visibility.Visible;
				mainWindow.Show();

				currentStartupWindow.Close();
			}
			else
			{
				currentPage.AlarmTextBlock.Visibility = Visibility.Visible;

				DispatcherTimer timer = new DispatcherTimer();
				timer.Interval = TimeSpan.FromSeconds(2);
				timer.Tick += (sender, args) =>
				{
					timer.Stop();
					currentPage.AlarmTextBlock.Visibility = Visibility.Hidden;
				};
				timer.Start();
			}
		});

		public RelayCommand ChangeVisibilityPassCmd { get => _changeVisibilityPass; }
		public RelayCommand SignInAccountCmd { get => signInAccount; }

		#endregion
	}
}
