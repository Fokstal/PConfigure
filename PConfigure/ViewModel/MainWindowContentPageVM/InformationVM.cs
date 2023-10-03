using PConfigure.Addition;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;

namespace PConfigure.ViewModel.MainWindowContentPageVM
{
	class InformationVM
	{
		private readonly RelayCommand _setEngContentCmd = new(o =>
		{
			InformationPage informationPage = o as InformationPage ?? new();

			RichTextBox docBox = informationPage.ContentRichTextBox;

			TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);

			using (FileStream fs = new FileStream("InformationPage_Content/en.txt", FileMode.Open))
			{
				doc.Load(fs, DataFormats.Text);
			}

			informationPage.EngButton.IsEnabled = false;
			informationPage.RusButton.IsEnabled = true;
		});

		private readonly RelayCommand _setRusContentCmd = new(o =>
		{
			InformationPage informationPage = o as InformationPage ?? new();

			RichTextBox docBox = informationPage.ContentRichTextBox;

			TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);

			using (FileStream fs = new FileStream("InformationPage_Content/ru.txt", FileMode.Open))
			{
				doc.Load(fs, DataFormats.Text);
			}

			informationPage.EngButton.IsEnabled = true;
			informationPage.RusButton.IsEnabled = false;
		});

		public RelayCommand SetEngContentCmd { get => _setEngContentCmd; }
		public RelayCommand SetRusContentCmd { get => _setRusContentCmd; }
	}
}
