using Microsoft.IdentityModel.Tokens;
using PConfigure.Addition;
using PConfigure.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class MessageAlarmVM : INotifyPropertyChanged
	{
		private string _messageText = "Alarm";
		private Brush _colorBell = Brushes.Black;

		public string MessageText
		{
			get => _messageText;
			set
			{
				_messageText = value;

				OnPropertyChanged(nameof(MessageText));
			}
		}
		public Brush ColorBell
		{
			get => _colorBell;
			set
			{
				_colorBell = value;
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (propertyName is not null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void Open(Window currentWindow, string? message = null, Brush? colorBell = null)
		{
			MessageText = message ?? "Alarm";
			ColorBell = colorBell ?? Brushes.ForestGreen;


			MessageAlarm messageAlarm = new();

			messageAlarm.Owner = currentWindow;
			messageAlarm.Left = currentWindow.Left + currentWindow.Width - messageAlarm.Width - 40;
			messageAlarm.Top = currentWindow.Top + currentWindow.Height - messageAlarm.Height - 40;

			messageAlarm.Show();

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(5.5);
			timer.Tick += (sender, args) =>
			{
				timer.Stop();
				messageAlarm.Close();
			};
			timer.Start();

		}
	}
}
