using PConfigure.Model;
using PConfigure.View.MainWindowContentPage;
using System;
using System.Collections.Generic;
using System.Reflection;
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
		private static object InstanceItem { get; set; } = new object();

		private static Window? InstanceWindow { get; set; }

		public static void OpenAddNewValue(object item)
		{
			InstanceItem = item;

			Type type = SetTypesAndNamesPropFromItem(item);

			CurrentWindow = new();

			SetControlAddNewValueToWindow(type, CurrentWindow);

			CurrentWindow.ShowDialog();
		}
		public static void OpenEditValue(object item)
		{
			InstanceItem = item;

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
					listParam.Add(Convert.ChangeType(textBox.Text.Replace('.', ','), TypesProp[l]));
				}
				catch
				{
					textBox.BorderBrush = Brushes.DarkRed;
					textBox.BorderThickness = new(1.5);

					listParam = new();
					return;
				}

				l++;
			}

			if (listParam.Count != 0)
			{
				if (textBlock.Text.Contains("Add"))
				{
					string nameType = "AddNewValue_" + textBlock.Text.Replace("Add ", "");

					DataWorker.AddNewValue(SetPropsByType(listParam, nameType), out string resultStr);
				}

				if (textBlock.Text.Contains("Edit"))
				{
					string nameType = "AddNewValue_" + textBlock.Text.Replace("Edit ", "");

					DataWorker.EditValue(InstanceItem, SetPropsByType(listParam, nameType), out string resultStr);
				}
			}

			InstanceWindow.Close();

		}
		private static void CurrentWindowButton_ClickClose(object sender, RoutedEventArgs e) => InstanceWindow.Close();

		private static object SetPropsByType(List<object> parameters, string nameType)
		{
			if (nameType == "AddNewValue_Blockpower") return new Data_Blockpower()
				{
					Name = parameters[0].ToString(),
					CapacityPower = Convert.ToInt32(parameters[1]),
					CUA = Convert.ToInt32(parameters[2]),
					TypeGPUPower = Convert.ToInt32(parameters[3]),
					Price = Convert.ToDouble(parameters[4]),
				};
			if (nameType == "AddNewValue_CPU") return new Data_CPU() 
			{ 
				Model = parameters[0].ToString(), 
				Name = parameters[1].ToString(), 
				Socket = parameters[2].ToString(), 
				Frequency = Convert.ToDouble(parameters[3]), 
				Core = Convert.ToInt32(parameters[4]),
				Cash = Convert.ToInt32(parameters[5]), 
				TDP = Convert.ToInt32(parameters[6]), 
				Price = Convert.ToDouble(parameters[7]) };
			if (nameType == "AddNewValue_GPU") return new Data_GPU() 
			{ Name = parameters[0].ToString(), Frequency = Convert.ToInt32(parameters[1]), CapacityMemory = Convert.ToInt32(parameters[2]), TypeGDDR = Convert.ToInt32(parameters[3]), TypePower = Convert.ToInt32(parameters[4]), TDP = Convert.ToInt32(parameters[5]), Price = Convert.ToDouble(parameters[6]) };
			if (nameType == "AddNewValue_Memory") return new Data_Memory() 
			{ Name = parameters[0].ToString(), Type = parameters[1].ToString(), CapacityMemory = Convert.ToInt32(parameters[2]), TypeConnect = parameters[3].ToString(), Speed = Convert.ToInt32(parameters[4]), Price = Convert.ToDouble(parameters[5]) };
			if (nameType == "AddNewValue_Motherboard") return new Data_Motherboard() 
			{ Name = parameters[0].ToString(), TypeATX = parameters[1].ToString(), Socket = parameters[2].ToString(), Chipset = parameters[3].ToString(), TypeDDR = Convert.ToInt32(parameters[4]), TypeGDDR = Convert.ToInt32(parameters[5]), CountSATA3 = Convert.ToInt32(parameters[6]), CountM2 = Convert.ToInt32(parameters[7]), Price = Convert.ToDouble(parameters[8]) };
			if (nameType == "AddNewValue_RAM") return new Data_RAM() 
			{ Name = parameters[0].ToString(), Frequency = Convert.ToInt32(parameters[1]), TypeDDR = Convert.ToInt32(parameters[2]), CapacityMemory = Convert.ToInt32(parameters[3]), TDP = Convert.ToDouble(parameters[4]), Price = Convert.ToDouble(parameters[5]) };

			return new object();

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
			grid.Background = Brushes.DimGray;
			grid.Opacity = 0.9;

			StackPanel stackPanel = new();

			TextBlock titleTextBlock = new();
			titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
			titleTextBlock.Margin = new(0, 0, 0, 5);
			titleTextBlock.FontSize = 26;
			titleTextBlock.Foreground = Brushes.GhostWhite;
			titleTextBlock.Text = $"Add " + type.Name.Replace("Data_", "");

			stackPanel.Children.Add(titleTextBlock);

			for (int i = 0; i < TypesProp.Count; i++)
			{
				CreateControlsToAddNewValue(out TextBlock tBlock, out TextBox tBox, NamesProp[i]);

				stackPanel.Children.Add(tBlock);
				stackPanel.Children.Add(tBox);
			}

			// Create Button and his ACTION
			Button btnAdd = new();

			btnAdd.Width = 150;
			btnAdd.Margin = new(0, 30, 0, 0);
			btnAdd.FontWeight = FontWeights.Bold;
			btnAdd.Content = $"Add VALUE";
			btnAdd.Click += CurrentWindowButton_Click;
			stackPanel.Children.Add(btnAdd);

			Button btnClose = new();

			btnClose.Width = 150;
			btnClose.Margin = new(0, 30, 0, 0);
			btnClose.Background = Brushes.LightCoral;
			btnClose.BorderBrush = Brushes.LightCoral;
			btnClose.FontWeight = FontWeights.Bold;
			btnClose.Content = "Close";
			btnClose.Click += CurrentWindowButton_ClickClose;
			stackPanel.Children.Add(btnClose);

			// Param-paparam
			window.Height = (NamesProp.Count + 4) * 70;
			window.Width = 400;
			grid.Children.Add(stackPanel);

			window.Content = grid;

			InstanceWindow = window;
		}
		private static void SetControlEditValueToWindow(Type type, AddNewValueWindow window)
		{
			Grid grid = new();
			grid.Background = Brushes.DimGray;
			grid.Opacity = 0.9;

			StackPanel stackPanel = new();

			TextBlock titleTextBlock = new();
			titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
			titleTextBlock.Margin = new(0, 0, 0, 5);
			titleTextBlock.FontSize = 26;
			titleTextBlock.Foreground = Brushes.GhostWhite;
			titleTextBlock.Text = $"Edit " + type.Name.Replace("Data_", "");

			stackPanel.Children.Add(titleTextBlock);

			for (int i = 0; i < TypesProp.Count; i++)
			{
				CreateControlsToEditValue(out TextBlock tBlock, out TextBox tBox, NamesProp[i], ValuesProp[i]);

				stackPanel.Children.Add(tBlock);
				stackPanel.Children.Add(tBox);
			}

			// Create Button and his ACTION
			Button btnAdd = new();

			btnAdd.Width = 150;
			btnAdd.Margin = new(0, 30, 0, 0);
			btnAdd.FontWeight = FontWeights.Bold;
			btnAdd.Content = $"Edit VALUE";
			btnAdd.Click += CurrentWindowButton_Click;
			stackPanel.Children.Add(btnAdd);

			Button btnClose = new();

			btnClose.Width = 150;
			btnClose.Margin = new(0, 30, 0, 0);
			btnClose.Background = Brushes.LightCoral;
			btnClose.BorderBrush = Brushes.LightCoral;
			btnClose.FontWeight = FontWeights.Bold;
			btnClose.Content = "Close";
			btnClose.Click += CurrentWindowButton_ClickClose;
			stackPanel.Children.Add(btnClose);

			// Param-paparam
			window.Height = (NamesProp.Count + 4) * 70;
			window.Width = 400;
			grid.Children.Add(stackPanel);

			window.Content = grid;

			InstanceWindow = window;
		}

		private static void CreateControlsToAddNewValue(out TextBlock tBlock, out TextBox tBox, string name)
		{
			tBlock = new();
			tBox = new();

			tBlock.Text = name;
			tBlock.FontSize = 18;
			tBlock.Foreground = Brushes.Black;
			tBlock.FontWeight = FontWeights.DemiBold;
			tBlock.HorizontalAlignment = HorizontalAlignment.Center;

			tBox.Name = name;
			tBox.TextAlignment = TextAlignment.Center;
			tBox.BorderBrush = Brushes.DarkSeaGreen;
			tBox.Margin = new(0, 10, 0, 10);
			tBox.FontWeight = FontWeights.Light;
			tBox.FontSize = 18;
			tBox.Width = 300;
			tBox.HorizontalAlignment = HorizontalAlignment.Center;
		}
		private static void CreateControlsToEditValue(out TextBlock tBlock, out TextBox tBox, string name, string valueTextBox)
		{
			tBlock = new();
			tBox = new();

			tBlock.Text = name;
			tBlock.FontSize = 18;
			tBlock.Foreground = Brushes.Black;
			tBlock.FontWeight = FontWeights.DemiBold;
			tBlock.HorizontalAlignment = HorizontalAlignment.Center;

			tBox.Name = name;
			tBox.Text = valueTextBox;
			tBox.TextAlignment = TextAlignment.Center;
			tBox.BorderBrush = Brushes.DarkSeaGreen;
			tBox.Margin = new(0, 10, 0, 10);
			tBox.FontWeight = FontWeights.Light;
			tBox.FontSize = 18;
			tBox.Width = 300;
			tBox.HorizontalAlignment = HorizontalAlignment.Center;
		}

		#endregion
	}
}
