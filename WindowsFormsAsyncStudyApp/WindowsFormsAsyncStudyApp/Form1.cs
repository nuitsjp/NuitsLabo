using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAsyncStudyApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private async void Initialize()
        {
            await InitializeAsync();
        }

        private Task InitializeAsync()
        {
            return Task.Run(() => throw new Exception());
        }
    }
}
