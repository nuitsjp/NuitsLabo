using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace CompassStudy
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Compass _compass;
        public MainPage()
        {
            this.InitializeComponent();
            InitCompass();
        }

        private void InitCompass()
        {
            _compass = Compass.GetDefault();
            if (_compass == null)
            {
                // コンパスがサポートされていない場合
                return;
            }

            // イベントハンドラを登録
            _compass.ReadingChanged += Compass_ReadingChanged;
        }

        private void Compass_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            // 方位角を取得
            var heading = args.Reading.HeadingTrueNorth;
            // UIスレッドでUIを更新
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // UIを更新
                txtCompass.Text = heading.ToString();
            });
        }
    }
}
