using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SW2ExplorerWV
{
    public partial class Splash : Form
    {
        int counter = 0;
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            if (File.Exists("hadsplash"))
            {
                this.Close();
                return;
            }
            timer1.Enabled = true;
            counter = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (counter++ == 10)
            {
                this.Close();
                File.WriteAllText("hadsplash", "");
            }
        }
    }
}
