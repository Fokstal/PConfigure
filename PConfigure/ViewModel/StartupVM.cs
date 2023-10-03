using PConfigure.Addition;
using PConfigure.View;

namespace PConfigure.ViewModel
{
    internal class StartupVM
	{
		#region Select Acc VM

		public static bool rememberChoice = false;

		private readonly RelayCommand _checkedRememberChoice = new(o =>
		{
			if (rememberChoice)
			{
				rememberChoice = false;
				return;
			}

			if (!rememberChoice)
			{
				rememberChoice = true;
				return;
			}

		});


		public RelayCommand CheckedRememberChoice { get => _checkedRememberChoice; }
		public RelayCommand SelectUserAccCmd
		{
			get => new(o =>
			{
				StartupPage currentStartupPage = o as StartupPage ?? new StartupPage();
				currentStartupPage.SelectUserAccBtn.IsEnabled = false;
				currentStartupPage.SelectCreatorAccBtn.IsEnabled = true;

				StartupWindowVM.ChoiceAccount = "1";
			});
		}
		public RelayCommand SelectCreatorAccCmd
		{
			get => new(o =>
			{
				StartupPage currentStartupPage = o as StartupPage ?? new StartupPage();
				currentStartupPage.SelectCreatorAccBtn.IsEnabled = false;
				currentStartupPage.SelectUserAccBtn.IsEnabled = true;

				StartupWindowVM.ChoiceAccount = "2";
			});
		}

		#endregion
	}
}
