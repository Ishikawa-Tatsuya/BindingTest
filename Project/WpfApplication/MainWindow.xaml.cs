using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new
            {
                Hoge = new
                {
                    Fuga = "abc"
                },
                Piyo = 2,
                Color = Colors.Red,
                Collection = new[]
                {
                    new {Text = "AAA", IsSelected = true},
                    new {Text = "BBB", IsSelected = false},
                    new {Text = "CCC", IsSelected = false},
                },
            };
        }
    }
}
