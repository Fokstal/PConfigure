using PConfigure.Model;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PConfigure.Addition
{
	internal class WindowFromData
	{
		private static AddNewValueWindow? CurrentWindow { get; set; }
		private static List<Type> TypesProp { get; set; } = new();
		private static List<string> NamesProp { get; set; } = new();
		private static List<string> ValuesProp { get; set; } = new();

		public static void OpenAddNewValue(object item)
		{

			Type type = SetTypesAndNamesPropFromItem(item);

			CurrentWindow = new();

			SetControlAddNewValueToWindow(type, CurrentWindow);

			CurrentWindow.ShowDialog();
		}
		public static void OpenEditValue(object item)
		{

			Type type = SetTypesAndNamesPropFromItem(item);

			CurrentWindow = new();

			SetControlEditValueToWindow(type, CurrentWindow);

			CurrentWindow.ShowDialog();
		}

		#region Methods and Actions

		private static void CurrentWindowButton_Click(object sender, RoutedEventArgs e)
		{
			List<TextBox> listTextBox = new();
			TextBlock textBlock = new();
			int positionTextBlock = 0;

			foreach (var child in LogicalTreeHelper.GetChildren(CurrentWindow))
			{
				if (child is Grid)
				{
					foreach (var childGrid in LogicalTreeHelper.GetChildren(child as Grid))
					{
						if (childGrid is StackPanel)
						{
							foreach (var childStackPanel in LogicalTreeHelper.GetChildren(childGrid as StackPanel))
							{
								if (childStackPanel is TextBox)
								{
									listTextBox.Add(childStackPanel as TextBox ?? new());
								}

								if (childStackPanel is TextBlock && positionTextBlock == 0)
								{
									textBlock = childStackPanel as TextBlock ?? new();
									positionTextBlock = 1;
								}
							}
						}
					}
				}
			}

			int l = 0;

			List<object> listParam = new();

			foreach (TextBox textBox in listTextBox)
			{
				textBox.BorderBrush = Brushes.Black;
				textBox.BorderThickness = new(0.5);

				try
				{
					listParam.Add(Convert.ChangeType(textBox.Text, TypesProp[l]));
				}
				catch
				{
					textBox.BorderBrush = Brushes.DarkRed;
					textBox.BorderThickness = new(1.5);

					listParam = new();
				}

				l++;
			}

			if (listParam.Count != 0)
			{
				if (textBlock.Text.Contains("Add"))
				{
					string nameMethod = "AddNewValue_" + textBlock.Text.Replace("Add ", "");

					InvokeAddMethodByName(nameMethod, out string resultStr, listParam);

					//MessageBox.Show(resultStr);
				}

				if (textBlock.Text.Contains("Edit"))
				{
					string nameMethod = "EditValue_" + textBlock.Text.Replace("Edit ", "");

					InvokeEditMethodByName(nameMethod, out string resultStr, listParam);

					//MessageBox.Show(resultStr);
				}
			}

		}

		private static void InvokeAddMethodByName(string nameMethod, out string resultStr, List<object> parameters)
		{
			resultStr = "";

			switch (nameMethod)
			{
				case "AddNewValue_Blockpower":
					{
						//DataWorker.AddNewValue_Blockpower(out resultStr, parameters);
						MessageBox.Show("Add BLOCKPOWER");
						break;
					}

				case "AddNewValue_CPU":
					{
						MessageBox.Show("Add CPU");
						break;
					}

				case "AddNewValue_GPU":
					{
						MessageBox.Show("Add GPU");
						break;
					}

				case "AddNewValue_Memory":
					{
						MessageBox.Show("Add MEMORY");
						break;
					}

				case "AddNewValue_Motherboard":
					{
						MessageBox.Show("Add MOTHERBOARD");
						break;
					}

				case "AddNewValue_RAM":
					{
						MessageBox.Show("Add RAM");
						break;
					}

			}
		}

		private static void InvokeEditMethodByName(string nameMethod, out string resultStr, List<object> parameters)
		{
			resultStr = "";

			switch (nameMethod)
			{
				case "EditValue_Blockpower":
					{
						MessageBox.Show("Edit BLOCKPOWER"); 
						break;
					}

				case "EditValue_CPU":
					{
						MessageBox.Show("Edit CPU"); 
						break;
					}

				case "EditValue_GPU":
					{
						MessageBox.Show("Edit GPU"); 
						break;
					}

				case "EditValue_Memory":
					{
						MessageBox.Show("Edit MEMORY"); 
						break;
					}

				case "EditValue_Motherboard":
					{
						MessageBox.Show("Edit MOTHERBOARD"); 
						break;
					}

				case "EditValue_RAM":
					{
						MessageBox.Show("Edit RAM"); 
						break;
					}

			}
		}



		private static Type SetTypesAndNamesPropFromItem(object item)
		{
			Type type = item.GetType() ?? typeof(object);
			var props = type.GetProperties();


			TypesProp = new();
			NamesProp = new();
			ValuesProp = new();

			int numberProp = 0;
			foreach (var prop in props)
			{
				if (numberProp > 0)
				{
					TypesProp.Add(prop.PropertyType);
					NamesProp.Add(prop.Name);
					ValuesProp.Add(item.GetType().GetProperty(prop.Name.ToString())?.GetValue(item)?.ToString() ?? "");
				}

				numberProp++;
			}

			return type;
		}

		#endregion

		#region Controls WORKER

		private static void SetControlAddNewValueToWindow(Type type, AddNewValueWindow window)
		{
			Grid grid = new();
			grid.Background = Brushes.LightCyan;

			StackPanel stackPanel = new();

			TextBlock titleTextBlock = new();
			titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
			titleTextBlock.Margin = new(0, 0, 0, 5);
			titleTextBlock.FontSize = 26;
			titleTextBlock.Text = "Add " + type.Name.Replace("Data_", "");

			stackPanel.Children.Add(titleTextBlock);

			for (int i = 0; i < TypesProp.Count; i++)
			{
				CreateControlsToAddNewValue(out TextBlock tBlock, out TextBox tBox, NamesProp[i]);

				stackPanel.Children.Add(tBlock);
				stackPanel.Children.Add(tBox);
			}

			// Create Button and his ACTION
			Button btn = new();

			btn.Width = 150;
			btn.Content = "Add VALUE";
			btn.Click += CurrentWindowButton_Click;
			stackPanel.Children.Add(btn);

			// Param-paparam
			window.Height = (NamesProp.Count + 4) * 70;
			window.Width = 400;
			grid.Children.Add(stackPanel);

			window.Content = grid;
			window.Title = "Add new VALUE";
		}
		private static void SetControlEditValueToWindow(Type type, AddNewValueWindow window)
		{
			Grid grid = new();
			grid.Background = Brushes.LightCyan;

			StackPanel stackPanel = new();
			stackPanel.Orientation = Orientation.Horizontal;

			TextBlock titleTextBlock = new();
			titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
			titleTextBlock.Margin = new(0, 0, 0, 5);
			titleTextBlock.FontSize = 26;
			titleTextBlock.Text = "Edit " + type.Name.Replace("Data_", "");

			stackPanel.Children.Add(titleTextBlock);

			for (int i = 0; i < TypesProp.Count; i++)
			{
				CreateControlsToEditValue(out TextBlock tBlock, out TextBox tBox, NamesProp[i], ValuesProp[i]);

				stackPanel.Children.Add(tBlock);
				stackPanel.Children.Add(tBox);
			}

			// Create Button and his ACTION
			Button btn = new();

			btn.Width = 150;
			btn.Content = "Edit VALUE";
			btn.Click += CurrentWindowButton_Click;
			stackPanel.Children.Add(btn);

			// Param-paparam
			window.Width = (NamesProp.Count + 4) * 90;
			window.Height = 100;
			grid.Children.Add(stackPanel);

			window.Content = grid;
			window.Title = "Edit VALUE";
		}

		private static void CreateControlsToAddNewValue(out TextBlock tBlock, out TextBox tBox, string name)
		{
			tBlock = new();
			tBox = new();

			tBlock.Text = name;
			tBlock.FontSize = 16;
			tBlock.FontWeight = FontWeights.Bold;
			tBlock.HorizontalAlignment = HorizontalAlignment.Center;

			tBox.Name = name;
			tBox.TextAlignment = TextAlignment.Center;
			tBox.BorderBrush = Brushes.DimGray;
			tBox.Margin = new(0, 10, 0, 10);
			tBox.Height = 40;
			tBox.Width = 300;
			tBox.HorizontalAlignment = HorizontalAlignment.Center;
		}
		private static void CreateControlsToEditValue(out TextBlock tBlock, out TextBox tBox, string name, string valueTextBox)
		{
			tBlock = new();
			tBox = new();

			tBlock.Text = name;
			tBlock.FontSize = 16;
			tBlock.FontWeight = FontWeights.Bold;
			tBlock.VerticalAlignment = VerticalAlignment.Center;

			tBox.Name = name;
			tBox.Text = valueTextBox;
			tBox.TextAlignment = TextAlignment.Center;
			tBox.BorderBrush = Brushes.DimGray;
			tBox.Margin = new(0, 10, 0, 10);
			tBox.Height = 40;
			tBox.VerticalAlignment = VerticalAlignment.Center;
		}

		#endregion
	}
}
