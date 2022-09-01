using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xfinium.Pdf.View;

namespace PDFViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand FitWidthCommand = new RoutedCommand();

        public static RoutedCommand FitWidthOnDoubleClickCommand = new RoutedCommand();

        private int[] zoomLevels = new int[] { 25, 50, 75, 100, 150, 200, 250, 300 };

        public MainWindow()
        {
            InitializeComponent();
            pdfView.GraphicRendererFactory = new Xfinium.Graphics.Gdi.GdiRendererFactory();
            pdfView.Document = new Xfinium.Pdf.View.PdfVisualDocument();
            cbxZoom.ItemsSource = zoomLevels;
        }

        private void LoadPDFFile(string filePath)
        {
            FileStream pdfStream = File.OpenRead(filePath);
            pdfView.Document.Load(pdfStream);
            pdfStream.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPDFFile("xfinium.pdf");
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                LoadPDFFile(ofd.FileName);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            sfd.DefaultExt = "pdf";
            if (sfd.ShowDialog() == true)
            {
                pdfView.Document.Document.Save(sfd.FileName);
            }
        }

        private void FitWidth_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool isChecked = (bool)e.Parameter;
            pdfView.ZoomMode = isChecked ? PdfZoomMode.FitWidth : PdfZoomMode.Custom;
        }

        private void FitWidthOnDoubleClick_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            pdfView.FitWidthOnDoubleClick = (bool)e.Parameter;
        }

        private void cbxZoom_DropDownClosed(object sender, EventArgs e)
        {
            pdfView.ZoomMode = PdfZoomMode.Custom;
            pdfView.Zoom = int.Parse(cbxZoom.SelectedValue.ToString());
        }

        private void pdfView_ZoomModeChanged(object sender, EventArgs e)
        {
            tgbtnZoomMode.IsChecked = pdfView.ZoomMode == PdfZoomMode.FitWidth;
        }
    }
}
