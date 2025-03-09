using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AdvanceSample
{
    class PathUtil
    {
        static string environmentPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string fullPathToFolder = environmentPath + @"\twainImg";
        static string fullPathFileTransfer = environmentPath + @"\twainImg\imgFileTransfer.bmp";
        static string fullPathTiff = environmentPath + @"\twainImg\imgTiff.tiff";
        static string fullPathPDF = environmentPath + @"\twainImg\imgpdf.pdf";
        public static string SavePath()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Portable Network Graphics|*.png|PDF|*.pdf|TIFF|*.tiff|BMP|*.bmp";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog1.FileName;
            }
            return null;
        }
        /// <summary>
        /// Get tiff path.
        /// </summary>
        public static string TiffPath
        {
            get { return fullPathTiff; }
        }
        /// <summary>
        /// Get PDF path.
        /// </summary>
        public static string PDFPath
        {
            get { return fullPathPDF; }
        }
        /// <summary>
        /// Get file transfer path.
        /// </summary>
        public static string FileTransferPath
        {
            get { return fullPathFileTransfer; }
        }
        /// <summary>
        /// Get full path.
        /// </summary>
        public static string FullPath
        {
            get { return fullPathToFolder; }
        }
    }

}
