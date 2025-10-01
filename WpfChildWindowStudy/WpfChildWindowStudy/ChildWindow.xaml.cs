using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfChildWindowStudy
{
    /// <summary>
    /// ChildWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ChildWindow : Window
    {
        private bool isModal = false;

        public ChildWindow()
        {
            InitializeComponent();
        }

        public bool IsModal 
        { 
            get => isModal; 
            private set => isModal = value; 
        }

        // モーダルで表示
        public new bool? ShowDialog()
        {
            IsModal = true;
            return base.ShowDialog();
        }

        // 非モーダルで表示
        public new void Show()
        {
            IsModal = false;
            base.Show();
        }

        // モーダルから非モーダルに切り替え
        public void SwitchToNonModal()
        {
            if (IsModal && IsVisible)
            {
                // 現在の状態を保存
                var left = Left;
                var top = Top;
                var width = Width;
                var height = Height;
                var windowState = WindowState;
                var owner = Owner;

                // ウィンドウを閉じてから再度非モーダルで開く
                IsModal = false;
                Hide();
                
                // 状態を復元
                Left = left;
                Top = top;
                Width = width;
                Height = height;
                WindowState = windowState;
                Owner = owner;
                
                Show();
            }
        }

        // 非モーダルからモーダルに切り替え
        public bool? SwitchToModal()
        {
            if (!IsModal && IsVisible)
            {
                // 現在の状態を保存
                var left = Left;
                var top = Top;
                var width = Width;
                var height = Height;
                var windowState = WindowState;
                var owner = Owner;

                // ウィンドウを閉じる
                Hide();
                
                // 状態を復元
                Left = left;
                Top = top;
                Width = width;
                Height = height;
                WindowState = windowState;
                Owner = owner;
                
                // モーダルで再表示
                return ShowDialog();
            }
            return null;
        }
    }
}
