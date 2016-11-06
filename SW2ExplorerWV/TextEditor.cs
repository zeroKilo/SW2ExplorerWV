using System;
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
    public partial class TextEditor : Form
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        private void TextEditor_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            rtb1.Find(toolStripTextBox1.Text, rtb1.SelectionStart + 1, RichTextBoxFinds.None);
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
                rtb1.Find(toolStripTextBox1.Text, rtb1.SelectionStart + 1, RichTextBoxFinds.None);
        }
    }
}
