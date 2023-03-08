using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClickOnceStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static string Query { get; private set; }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // ClickOnceアプリの場合のときのみ以下のコードを実行
            if (Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed")?.ToLower() == "true")
            {
                return;
            }

            // 起動URLを取得
            string? value = Environment.GetEnvironmentVariable("ClickOnce_ActivationUri");
            if (string.IsNullOrEmpty(value)) return;

            var activationUri = new Uri(value);
            Query = activationUri.Query;
        }
    }
}
