using PConfigure.View;
using System.Windows;
using PConfigure.Model;
using System.Windows.Controls;
using PConfigure.Addition;

namespace PConfigure.ViewModel
{
    class SignInVM
	{
		#region SignIn VM

		public static StartupWindowVM? currentStartupWindow;

		public static string? Login { get; set; }
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

		// MC - 1
		public static readonly RelayCommand signInAccount = new(o =>
		{
			PasswordBox currentPage = o as PasswordBox ?? new PasswordBox();

			string? resultStr = null;

			string passwordFromPasswordBox = currentPage.Password ?? "";
			string paasswordFromPasswordTextBox = Password ?? "";

			string currentPassword = passwordFromPasswordBox.Length > paasswordFromPasswordTextBox.Length ? passwordFromPasswordBox : paasswordFromPasswordTextBox;

			AccountWorker.SignInAccout(out resultStr, Login, currentPassword);
		});

		public RelayCommand ChangeVisibilityPassCmd { get => _changeVisibilityPass; }
		public RelayCommand SignInAccountCmd { get => signInAccount; }

		#endregion
	}
}
