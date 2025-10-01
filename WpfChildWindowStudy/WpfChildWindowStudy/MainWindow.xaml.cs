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

namespace WpfChildWindowStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChildWindow currentChildWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var child = new ChildWindow();
            child.Owner = this;
            child.Show(); // 非モーダルで開く
            currentChildWindow = child;
        }

        // モーダルから非モーダルに切り替えるメソッド
        public void SwitchToNonModal()
        {
            if (currentChildWindow != null && currentChildWindow.IsLoaded)
            {
                // 現在のウィンドウの位置とサイズを保存
                var left = currentChildWindow.Left;
                var top = currentChildWindow.Top;
                var width = currentChildWindow.Width;
                var height = currentChildWindow.Height;
                var windowState = currentChildWindow.WindowState;

                // 現在のウィンドウを閉じる
                currentChildWindow.Close();

                // 新しいウィンドウを非モーダルで開く
                var newChild = new ChildWindow();
                newChild.Owner = this;
                newChild.Left = left;
                newChild.Top = top;
                newChild.Width = width;
                newChild.Height = height;
                newChild.WindowState = windowState;
                newChild.Show();
                
                currentChildWindow = newChild;
            }
        }
    }
}