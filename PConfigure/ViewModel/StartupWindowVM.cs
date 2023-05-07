using PConfigure.View;

namespace PConfigure.ViewModel
{
    class StartupWindowVM
    {
		// Fields
		private static string choiceAccount;


		private readonly RelayCommand _closeWindowCmd = new(o =>
		{
			StartupWindow currentWnd = o as StartupWindow ?? new StartupWindow();
			currentWnd.Close();
		});
		private static readonly RelayCommand continueCmd = new(o =>
		{
			StartupWindow currentStartupWindow = o as StartupWindow ?? new StartupWindow();

			if (choiceAccount == "2")
			{
				currentStartupWindow.ContentFrame.Content = new SignInCreatorPage();
				currentStartupWindow.ContinueBtn.Content = "Back";
				currentStartupWindow.ContinueBtn.Command = backCmd;

				return;
			}

			if (choiceAccount == "1")
			{
				new MainWindow().Show();
				currentStartupWindow.Close();

				return;
			}
			
		});
		private static readonly RelayCommand backCmd = new(o =>
		{
			StartupWindow currentStartupWindow = o as StartupWindow ?? new StartupWindow();

			currentStartupWindow.ContentFrame.Content = new StartupPage();
			currentStartupWindow.ContinueBtn.Content = "Continue";
			currentStartupWindow.ContinueBtn.Command = continueCmd;
		});



		// Properties
		public RelayCommand CloseWindowCmd { get => _closeWindowCmd; }
		public RelayCommand ContinueCmd { get => continueCmd; }
		public static string ChoiceAccount { set => choiceAccount = value; }
	}
}
