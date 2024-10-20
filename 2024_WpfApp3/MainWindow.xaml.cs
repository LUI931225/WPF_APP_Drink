using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

namespace _2024_WpfApp3
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>
        {
            {"紅茶　", 10},
            {"奶茶　", 20},
            {"綠茶　", 15},
            {"冬瓜茶", 25},
            {"咖啡　", 30},
            {"拿鐵　", 40},
            {"水果茶", 50}
        };
        string takeout = "";
        public MainWindow()
        {
            InitializeComponent();

            DisplayDrinkMenu(drinks);
        }

        private void DisplayDrinkMenu(Dictionary<string,int> drinks)
        {
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
                    Margin = new Thickness(0,0,10,0),
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
            }
        }
    }
}
