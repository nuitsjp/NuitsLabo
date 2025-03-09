/* Этот файл является частью примеров использования библиотеки Saraff.Twain.NET
 * © Буякевич Александр, 2016.
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2011.
 * Saraff.Twain.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Twain.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of samples of Saraff.Twain.NET.
 * © Buyakevich Alexander, 2016.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2011.
 * Saraff.Twain.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Twain.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Twain.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Remoting.Messaging;
using Saraff.Twain.Aux;
using Saraff.Twain.Wpf.Sample3.Core;

namespace Saraff.Twain.Wpf.Sample3 {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TwainSample3:Window {
        private bool _isLoading=true;
        private Dictionary<TwCap, Twain32.Enumeration> _capValues=new Dictionary<TwCap, Twain32.Enumeration> {
            {TwCap.XResolution,null},
            {TwCap.IPixelType,null},
            {TwCap.IXferMech,null},
            {TwCap.ImageFileFormat,null},
        };

        public TwainSample3() {
            InitializeComponent();
        }

        #region Methods

        private void _Load() {
            this.acquireButton.IsEnabled=false;
            this._XferMech.Source=new string[0];
            this._Sources.Source=new Collection<string> { "Loading..." };
            new Func<Collection<Source>>(this._GetSourcesCore).BeginInvoke(result => {
                this.Dispatcher.BeginInvoke(
                    new Action(() => {
                        try {
                            var _sources=((Func<Collection<Source>>)((AsyncResult)result).AsyncDelegate).EndInvoke(result);
                            this._Sources.Source=_sources;
                            for(var i = 0; i<_sources.Count; i++) {
                                if(_sources[i].IsDefault) {
                                    this._Sources.View.MoveCurrentToPosition(i);
                                    break;
                                }
                            }
                            this._Sources.View.CurrentChanged+=this._DataSourceCurrentChanged;

                            this._isLoading=false;
                            if(this._Sources.View!=null&&!this._Sources.View.IsEmpty) {
                                this._DeviceChanged();
                            }
                        } catch(Exception ex) {
                            ex.ErrorMessageBox();
                            this._Sources.Source=new Collection<string> { "Error!" };
                        }
                    })
                );
            }, null);
        }

        private void _DeviceChanged() {
            this.acquireButton.IsEnabled=false;
            this._Resolutions.Source=this._PixelTypes.Source=this._XferMech.Source=this._ImageFileFormats.Source=new Collection<string> { "Loading..." };
            new Action<Source>(this._GetCapsCore).BeginInvoke(this._CurrentDataSource, result => {
                this.Dispatcher.BeginInvoke(
                    new Action(() => {
                        try {
                            ((Action<Source>)((AsyncResult)result).AsyncDelegate).EndInvoke(result);

                            this._Resolutions.Set(this._capValues[TwCap.XResolution]);
                            this._PixelTypes.Set(this._capValues[TwCap.IPixelType]);
                            this._XferMech.Set(this._capValues[TwCap.IXferMech]);
                            this._ImageFileFormats.Set(this._capValues[TwCap.ImageFileFormat]);

                            this._XferMech.View.CurrentChanged+=this._XferMechCurrentChanged;
                            this._SetVisibilityImageFileFormExpander();
                            this.acquireButton.IsEnabled=true;

                        } catch(Exception ex) {
                            this._Resolutions.Source=this._PixelTypes.Source=this._XferMech.Source=this._ImageFileFormats.Source=new List<string> { "Error!" };
                            ex.ErrorMessageBox();
                        }
                    })
                );
            }, null);
        }

        private void _Acquire() {
            this.dataSourcesExpander.IsEnabled=
                this.resolutionExpander.IsEnabled=
                this.pixelTypeExpander.IsEnabled=
                this.xferMechExpander.IsEnabled=
                this.imageFileFormExpander.IsEnabled=
                this.acquireButton.IsEnabled=false;

            new Action<Source, float, TwPixelType, TwSX, TwFF>(this._AcquireCore).BeginInvoke(
                this._CurrentDataSource,
                (float)(this.resolutionExpander.Content as ListBox).SelectedValue,
                (TwPixelType)(this.pixelTypeExpander.Content as ListBox).SelectedValue,
                (TwSX)(this.xferMechExpander.Content as ListBox).SelectedValue,
                (TwFF)(this.imageFileFormExpander.Content as ListBox).SelectedValue,
                result => {
                    this.Dispatcher.BeginInvoke(
                        new Action(() => {
                            try {
                                ((Action<Source, float, TwPixelType, TwSX, TwFF>)((AsyncResult)result).AsyncDelegate).EndInvoke(result);
                            } catch(Exception ex) {
                                ex.ErrorMessageBox();
                            } finally {
                                this.dataSourcesExpander.IsEnabled=
                                    this.resolutionExpander.IsEnabled=
                                    this.pixelTypeExpander.IsEnabled=
                                    this.xferMechExpander.IsEnabled=
                                    this.acquireButton.IsEnabled=true;

                                this.imageFileFormExpander.IsEnabled=((TwSX)this._XferMech.View.CurrentItem)==TwSX.File;
                            }
                        })
                    );
                }, null);
        }

        #region Core

        private Collection<Source> _GetSourcesCore() {
            var _result=new Collection<Source>();
            foreach(var _host in new string[] { Source.x86Aux,Source.msilAux }) {
                TwainExternalProcess.Execute(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),_host),
                    twain => {
                        try {
                            if(_host==Source.msilAux&&!twain.IsTwain2Supported) {
                                return;
                            }
                            for(var i=0; i<twain.SourcesCount; i++) {
                                _result.Add(new Source {
                                    Id=i,
                                    Name=twain.GetSourceProductName(i),
                                    IsX64Platform=_host==Source.x86Aux?false:Environment.Is64BitOperatingSystem,
                                    IsTwain2=twain.IsTwain2Supported,
                                    IsDefault=twain.SourceIndex==i
                                });
                            }
                        } catch {
                        }
                    });
            }
            return _result;
        }

        private void _GetCapsCore(Source source) {
            TwainExternalProcess.Execute(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),source.ExecFileName),
                twain => {
                    try {
                        twain.SourceIndex=source.Id;
                        twain.OpenDataSource();

                        this._capValues[TwCap.XResolution]=twain.Capabilities.XResolution.Get();
                        this._capValues[TwCap.IPixelType]=twain.Capabilities.PixelType.Get();
                        this._capValues[TwCap.IXferMech]=twain.Capabilities.XferMech.Get();
                        this._capValues[TwCap.ImageFileFormat]=twain.Capabilities.ImageFileFormat.Get();
                    } catch {
                    }
                });
        }

        private void _AcquireCore(Source source,float resolution,TwPixelType pixelType,TwSX xferMech,TwFF imageFileFormat) {
            TwainExternalProcess.Execute(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location),source.ExecFileName),
                twain => {

                    #region Native

                    twain.EndXfer+=(sender,e) => {
                        try {
                            this.Dispatcher.BeginInvoke(
                                 new Action(() => {
                                     try {
                                         this.scanImage.Source=e.ImageSource;
                                     } catch(Exception ex) {
                                         ex.ErrorMessageBox();
                                     }
                                 })
                             );
                        } catch {
                        }
                    };

                    #endregion

                    #region File

                    twain.SetupFileXferEvent+=(sender,e) => {
                        try {
                            var _dlg=new Microsoft.Win32.SaveFileDialog {
                                Filter=String.Format("{0}-files|*.{0}", imageFileFormat.ToString().ToLower()),
                                OverwritePrompt=true
                            };
                            if((bool)_dlg.ShowDialog()) {
                                e.FileName=_dlg.FileName;
                            } else {
                                e.Cancel=true;
                            }
                        } catch {
                        }
                    };

                    twain.FileXferEvent+=(sender,e) => {
                        try {
                            if(System.IO.Path.GetExtension(e.ImageFileXfer.FileName)==".tmp") {
                                Win32.MoveFileEx(e.ImageFileXfer.FileName,null,Win32.MoveFileFlags.DelayUntilReboot);
                            }
                            var _img=new BitmapImage(new Uri(e.ImageFileXfer.FileName));
                            _img.Freeze();
                            this.Dispatcher.BeginInvoke(
                                new Action(() => {
                                    try {
                                        this.scanImage.Source=_img;
                                    } catch(Exception ex) {
                                        ex.ErrorMessageBox();
                                    }
                                })
                            );
                        } catch {
                        }
                    };

                    #endregion

                    #region Memory

                    #region SetupMemXferEvent

                    twain.SetupMemXferEvent+=(sender,e) => {
                        try {
                            System.Windows.Media.PixelFormat _format=PixelFormats.Default;
                            BitmapPalette _pallete=null;
                            switch(e.ImageInfo.PixelType) {
                                case TwPixelType.BW:
                                    _format=PixelFormats.BlackWhite;
                                    break;
                                case TwPixelType.Gray:
                                    _format=new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {2,PixelFormats.Gray2},
                                        {4,PixelFormats.Gray4},
                                        {8,PixelFormats.Gray8},
                                        {16,PixelFormats.Gray16}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                case TwPixelType.Palette:
                                    _pallete=new BitmapPalette(new Func<IList<Color>>(() => {
                                        var _res=new Collection<Color>();
                                        var _colors=twain.Palette.Get().Colors;
                                        for(int i=0; i<_colors.Length; i++) {
                                            _res.Add(Color.FromArgb(_colors[i].A,_colors[i].R,_colors[i].G,_colors[i].B));
                                        }
                                        return _res;
                                    })());
                                    _format=new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {2,PixelFormats.Indexed1},
                                        {4,PixelFormats.Indexed2},
                                        {8,PixelFormats.Indexed4},
                                        {16,PixelFormats.Indexed8}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                case TwPixelType.RGB:
                                    _format=new Dictionary<short, System.Windows.Media.PixelFormat> {
                                        {8,PixelFormats.Rgb24},
                                        {24,PixelFormats.Rgb24},
                                        {16,PixelFormats.Rgb48},
                                        {48,PixelFormats.Rgb48}
                                    }[e.ImageInfo.BitsPerPixel];
                                    break;
                                default:
                                    throw new InvalidOperationException("Данный формат пикселей не поддерживается.");
                            }

                            this.Dispatcher.BeginInvoke(
                                new Action(() => {
                                    try {
                                        this.scanImage.Source=new WriteableBitmap(
                                            e.ImageInfo.ImageWidth,
                                            e.ImageInfo.ImageLength,
                                            e.ImageInfo.XResolution,
                                            e.ImageInfo.YResolution,
                                            _format,
                                            _pallete);
                                    } catch(Exception ex) {
                                        ex.ErrorMessageBox();
                                    }
                                })
                            );

                        } catch {
                        }
                    };

                    #endregion

                    twain.MemXferEvent+=(sender,e) => {
                        try {
                            this.Dispatcher.BeginInvoke(
                                new Action(() => {
                                    try {
                                        (this.scanImage.Source as WriteableBitmap).WritePixels(
                                            new Int32Rect(0,0,(int)e.ImageMemXfer.Columns,(int)e.ImageMemXfer.Rows),
                                            e.ImageMemXfer.ImageData,
                                            (int)e.ImageMemXfer.BytesPerRow,
                                            (int)e.ImageMemXfer.XOffset,
                                            (int)e.ImageMemXfer.YOffset);
                                    } catch(Exception ex) {
                                        ex.ErrorMessageBox();
                                    }
                                })
                            );
                        } catch {
                        }
                    };

                    #endregion

                    #region Set Capabilities

                    twain.SourceIndex=source.Id;
                    twain.OpenDataSource();

                    try {
                        twain.SetCap(TwCap.XResolution,resolution);
                    } catch {
                    }

                    try {
                        twain.SetCap(TwCap.YResolution,resolution);
                    } catch {
                    }

                    try {
                        twain.SetCap(TwCap.IPixelType,pixelType);
                    } catch {
                    }

                    try {
                        twain.SetCap(TwCap.IXferMech,xferMech);
                    } catch {
                    }

                    try {
                        twain.SetCap(TwCap.ImageFileFormat,imageFileFormat);
                    } catch {
                    }

                    try {
                        twain.Capabilities.Indicators.Set(false);
                    } catch {
                    }

                    twain.Capabilities.XferCount.Set(1);


                    #endregion

                    twain.Acquire();
                });
        }

        #endregion

        private void _SetVisibilityImageFileFormExpander() {
            this.imageFileFormExpander.Visibility=(this.imageFileFormExpander.IsEnabled=((TwSX)this._XferMech.View.CurrentItem)==TwSX.File)?Visibility.Visible:Visibility.Hidden;
        }

        #endregion

        #region Properties

        private CollectionViewSource _Sources {
            get {
                return this.Resources["TwainSources"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _Resolutions {
            get {
                return this.Resources["Resolutions"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _PixelTypes {
            get {
                return this.Resources["PixelTypes"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _XferMech {
            get {
                return this.Resources["XferMech"] as CollectionViewSource;
            }
        }

        private CollectionViewSource _ImageFileFormats {
            get {
                return this.Resources["ImageFileFormats"] as CollectionViewSource;
            }
        }

        private Source _CurrentDataSource {
            get {
                return (this.dataSourcesExpander.Content as ListBox).SelectedValue as Source;
            }
        }

        #endregion

        #region EventHandlers

        private void _DataSourceCurrentChanged(object sender,EventArgs e) {
            try {
                if(!this._isLoading) {
                    this._DeviceChanged();
                }
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _XferMechCurrentChanged(object sender,EventArgs e) {
            try {
                this._SetVisibilityImageFileFormExpander();
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _AcquireButtonClick(object sender,RoutedEventArgs e) {
            try {
                this._Acquire();
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _ByWidthButtonChecked(object sender,RoutedEventArgs e) {
            try {
                this.scanImage.Stretch=Stretch.UniformToFill;
                this.scrol.HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled;
                this.scrol.VerticalScrollBarVisibility=ScrollBarVisibility.Auto;
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _ByHeightButtonChecked(object sender,RoutedEventArgs e) {
            try {
                this.scanImage.Stretch=Stretch.Uniform;
                this.scrol.HorizontalScrollBarVisibility=ScrollBarVisibility.Auto;
                this.scrol.VerticalScrollBarVisibility=ScrollBarVisibility.Disabled;
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _DefaultButtonChecked(object sender,RoutedEventArgs e) {
            try {
                this.scanImage.Stretch=Stretch.None;
                this.scrol.HorizontalScrollBarVisibility=ScrollBarVisibility.Auto;
                this.scrol.VerticalScrollBarVisibility=ScrollBarVisibility.Auto;
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _BySizeButtonChecked(object sender,RoutedEventArgs e) {
            try {
                this.scanImage.Stretch=Stretch.Fill;
                this.scrol.HorizontalScrollBarVisibility=ScrollBarVisibility.Disabled;
                this.scrol.VerticalScrollBarVisibility=ScrollBarVisibility.Disabled;
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        private void _WindowLoaded(object sender,RoutedEventArgs e) {
            try {
                this._Load();
            } catch(Exception ex) {
                ex.ErrorMessageBox();
            }
        }

        #endregion

        private class Source {
            public const string x86Aux="Saraff.Twain.Aux_x86.exe";
            public const string msilAux="Saraff.Twain.Aux_MSIL.exe";

            public static readonly DependencyProperty CurrentProperty;

            static Source() {
                Source.CurrentProperty=DependencyProperty.RegisterAttached("Current",typeof(Source),typeof(Source),new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsRender|FrameworkPropertyMetadataOptions.AffectsMeasure));
            }

            public string Visual {
                get {
                    return this.ToString();
                }
            }

            public int Id {
                get;
                set;
            }

            public string ExecFileName {
                get {
                    return !this.IsX64Platform&&!this.IsTwain2?Source.x86Aux:Source.msilAux;
                }
            }

            public string Name {
                get;
                set;
            }

            public bool IsX64Platform {
                get;
                set;
            }

            public bool IsTwain2 {
                get;
                set;
            }

            public bool IsDefault {
                get;
                set;
            }

            public override bool Equals(object obj) {
                for(var _val = obj as Source; _val!=null;) {
                    return _val.IsX64Platform==this.IsX64Platform&&_val.IsTwain2==this.IsTwain2&&_val.Id==this.Id;
                }
                return false;
            }

            public override int GetHashCode() {
                return this.Id.GetHashCode();
            }

            public override string ToString() {
                return String.Format("[{0}.x; {1}]: {2}",this.IsTwain2?"2":"1",this.IsX64Platform?"x64":"x86",this.Name);
            }
        }
    }
}
