using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;

namespace _2024_WpfApp3
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>();

        string takeout = "";
        Dictionary<string, int> orders = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();

            AddNewDrink(drinks);

            DisplayDrinkMenu(drinks);
        }

        private void AddNewDrink(Dictionary<string, int> drinks)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "選擇飲料品項檔案";
            openFileDialog.Filter = "CSV文件|*.csv|文字檔案|*.txt|所有文件|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(fileName);

                foreach (var line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = Convert.ToInt32(tokens[1]);
                    drinks.Add(drinkName, price);
                }
            }
        }
        private void DisplayDrinkMenu(Dictionary<string, int> drinks)
        {
            StackPanel_DrinkMenu.Children.Clear();
            StackPanel_DrinkMenu.Height = 42*drinks.Count;
            foreach (var drink in drinks)
            {
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(3),
                    Height = 35,
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = Brushes.LightBlue
                };

                var cb = new CheckBox
                {
                    Content = $"{drink.Key} ${drink.Value}元",
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 18,
                    Foreground = Brushes.Blue,
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                var sl = new Slider
                {
                    Width = 150,
                    Value = 0,
                    Minimum = 0,
                    Maximum = 10,
                    IsSnapToTickEnabled = true,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                var lb = new Label
                {
                    Width = 40,
                    Content = "0",
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 18,
                };

                Binding myBinding = new Binding("Value")
                {
                    Source = sl,
                    Mode = BindingMode.OneWay
                };
                lb.SetBinding(ContentProperty, myBinding);

                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                StackPanel_DrinkMenu.Children.Add(sp);
                StackPanel_DrinkMenu.Height = drinks.Count * 40 + 8;
                ResultTextBlock.Margin = new Thickness(0, drinks.Count * 40 + 145, 0, 0);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if ((rb.IsChecked == true))
            {
                takeout = rb.Content.ToString();
                //MessageBox.Show($"取餐方式：{takeout}");
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "";
            string discountMessage = "";
            //確認所有訂單品項
            orders.Clear();
            for (int i = 0; i < StackPanel_DrinkMenu.Children.Count; i++)
            {
                var sp = StackPanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
                var lb = sp.Children[2] as Label;

                if (cb.IsChecked == true && sl.Value > 0)
                {
                    orders.Add(cb.Content.ToString(), int.Parse(lb.Content.ToString()));
                }
            }
            //顯示訂單細項，計算總金額

            double total = 0.0;
            double sellPrice = 0.0;

            ResultTextBlock.Text += $"取餐方式：{takeout}\n";

            int Num = 1;

            foreach (var item in orders)
            {
                string drinkName = item.Key.Split(' ')[0];
                int quanity = item.Value;
                int price = drinks[drinkName];

                int subTotal = price * quanity;
                total += subTotal;
                ResultTextBlock.Text += $"{Num}. {drinkName} x {quanity}杯，總共{subTotal}元\n";
                Num++;
            }
            if (total >= 500)
            {
                discountMessage = "滿500元，打8折";
                sellPrice = total;
                sellPrice *= 0.8;
            }
            else if (total >= 300)
            {
                discountMessage = "滿300元，打9折";
                sellPrice = total;
                sellPrice *= 0.9;
            }
            ResultTextBlock.Text += $"總金額：{total}元\n";
            ResultTextBlock.Text += $"{discountMessage}，實付金額：{sellPrice}元\n";
        }
    }
}
