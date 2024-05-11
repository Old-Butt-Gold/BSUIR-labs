using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MIAPR_4
{
    internal static class ExtensionMethods
    {
        public static void MakeInputBoxes(this StackPanel inputPanel, int number, TextCompositionEventHandler textCheck)
        {
            inputPanel.Children.Clear();
            for (var i = 0; i < number; i++)
            {
                var textBox = new TextBox
                {
                    Background = Brushes.Wheat,
                    BorderBrush = Brushes.Wheat,
                    Name = $"Input{i}",
                    FontSize = 25,
                    FontFamily = new FontFamily("Open Sans"),
                    TextAlignment = TextAlignment.Center,
                };
                textBox.PreviewTextInput += textCheck;
                inputPanel.Children.Add(textBox);
            }
        }


        public static void ClearListView(this ListView listView) => listView.Items.Clear();

        public static void CreateTextBlockOnListView(this ListView listView, string value)
        {
            listView.Items.Add(new TextBlock()
            {
                Width = listView.ActualWidth - 15,
                Background = Brushes.Wheat,
                TextAlignment = value[0] == 'К' ? TextAlignment.Center : TextAlignment.Left,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontFamily = new FontFamily("Open Sans"),
                FontSize = 18,
                Text = value
            });
        }
    }
}