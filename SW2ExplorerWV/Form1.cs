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
using Be.Windows.Forms;
using System.IO.Compression;

namespace SW2ExplorerWV
{
    public partial class Form1 : Form
    {
        public HOGPFile file;

        public Form1()
        {
            InitializeComponent();
            Helper.box = rtb1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}h {1}h {2}h {3}h", en.sizeUncomp.ToString("X8"), en.sizeComp.ToString("X8"), en.blockMapStart.ToString("X8"), en.offset.ToString("X16"));
            rtb2.Text = sb.ToString();
            try
            {
                if (!en.name.ToLower().EndsWith(".dds"))
                {
                    rtb3.Visible = hb1.Visible = true;
                }
                if (en.name.ToLower().EndsWith(".nut"))
                {
                    try
                    {
                        NutFile nut = new NutFile(new MemoryStream(file.ReadFile(en)));
                        rtb3.Text = nut.Print();
                        rtb3.BringToFront();
                    }
                    catch (Exception)
                    {
                        hb1.ByteProvider = new DynamicByteProvider(file.ReadFile(en));
                        hb1.BringToFront();
                    }
                }
                else if (en.name.ToLower().EndsWith(".dds"))
                {
                    if (File.Exists("preview.dds"))
                        File.Delete("preview.dds");
                    if (File.Exists("preview.bmp"))
                        File.Delete("preview.bmp");
                    File.WriteAllBytes("preview.dds", file.ReadFile(en));
                    Helper.Run("texconv.exe", " preview.dds -ft bmp");
                    pic1.Image = Bitmap.FromStream(new MemoryStream(File.ReadAllBytes("preview.bmp")));
                    pic1.BringToFront();
                    rtb3.Visible = hb1.Visible = false;
                    if (File.Exists("preview.dds"))
                        File.Delete("preview.dds");
                    if (File.Exists("preview.bmp"))
                        File.Delete("preview.bmp");
                }
                else
                {
                    hb1.ByteProvider = new DynamicByteProvider(file.ReadFile(en));
                    hb1.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Helper.Log("Error displaying content:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void exportSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1 || file == null)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            SaveFileDialog d = new SaveFileDialog();
            d.FileName = Path.GetFileName(en.name);
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                file.ExtractFile(en, d.FileName);
                MessageBox.Show("Done");
            }
        }

        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (file == null)
                return;
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = d.SelectedPath + "\\";
                pb1.Maximum = file.filelist.Count;
                pb1.Value = 0;
                foreach (HOGPFile.FileEntry en in file.filelist)
                {
                    pb1.Value++;
                    status.Text = en.name;
                    Application.DoEvents();
                    if (!Directory.Exists(path + Path.GetDirectoryName(en.name)))
                        Directory.CreateDirectory(path + Path.GetDirectoryName(en.name));
                    file.ExtractFile(en, path + en.name);
                    GC.Collect();
                }
                pb1.Value = 0;
                status.Text = "";
                MessageBox.Show("Done");
            }
        }

        private void importOverSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            OpenFileDialog d = new OpenFileDialog();
            d.FileName = Path.GetFileName(en.name);
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] buff = File.ReadAllBytes(d.FileName);
                file.ImportFile(n, buff);
                MessageBox.Show("Done.");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            int n = listBox1.SelectedIndex + 1;
            string term = toolStripTextBox1.Text.ToLower().Trim();
            for (int i = n; i < listBox1.Items.Count; i++)
                if (listBox1.Items[i].ToString().ToLower().Contains(term))
                {
                    listBox1.SelectedIndex = i;
                    return;
                }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                Search();
        }

        private void editInUnicodeEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            byte[] data = file.ReadFile(en);
            bool header = false;
            if (data.Length > 2 && data[0] == 0xFF && data[1] == 0xFE)
            {
                MemoryStream m = new MemoryStream();
                m.Write(data, 2, data.Length - 2);
                data = m.ToArray();
                header = true;
            }
            TextEditor ted = new TextEditor();
            ted.rtb1.Text = Encoding.Unicode.GetString(data);
            ted.ShowDialog();
            byte[] newdata = Encoding.Unicode.GetBytes(ted.rtb1.Text.Replace("\n", "\r\n"));
            bool same = true;
            if (newdata.Length != data.Length)
                same = false;
            else
                for (int i = 0; i < newdata.Length; i++)
                    if (newdata[i] != data[i])
                    {
                        same = false;
                        break;
                    }
            if (!same && MessageBox.Show("Do you want to save the changes?", "Do you want to save the changes?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                MemoryStream m2 = new MemoryStream();
                if (header)
                    m2.Write(new byte[] { 0xFF, 0xFE}, 0, 2);
                m2.Write(newdata, 0, newdata.Length);
                file.ImportFile(n, m2.ToArray());
                MessageBox.Show("Done.");
            }
        }

        private void editInAsciiEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            byte[] data = file.ReadFile(en);
            TextEditor ted = new TextEditor();
            ted.rtb1.Text = Encoding.ASCII.GetString(data);
            ted.ShowDialog();
            byte[] newdata = Encoding.ASCII.GetBytes(ted.rtb1.Text.Replace("\n", "\r\n"));
            bool same = true;
            if (newdata.Length != data.Length)
                same = false;
            else
                for (int i = 0; i < newdata.Length; i++)
                    if (newdata[i] != data[i])
                    {
                        same = false;
                        break;
                    }
            if (!same && MessageBox.Show("Do you want to save the changes?", "Do you want to save the changes?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                file.ImportFile(n, newdata);
                MessageBox.Show("Done.");
            }
        }

        private void openBinToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.bin|*.bin";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rtb1.Text = "";
                try
                {
                    file = new HOGPFile(d.FileName);
                    listBox1.Items.Clear();
                    int count = 0;
                    foreach (HOGPFile.FileEntry en in file.filelist)
                        listBox1.Items.Add((count++).ToString("X8") + " : " + en.name + "(" + en.sizeComp.ToString("X") + "h bytes)");
                }
                catch (Exception ex)
                {
                    Helper.Log("Error: " + ex.Message);
                }
            }
        }

        private void rebuildBinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.bin|*.bin";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rtb1.Text = "";
                try
                {
                    file = new HOGPFile(d.FileName);
                    pb1.Value = 0;
                    pb1.Maximum = file.filelist.Count;
                    FileStream fs = new FileStream(d.FileName, FileMode.Open, FileAccess.Read);
                    FileStream fs2 = new FileStream(d.FileName + ".tmp", FileMode.Create, FileAccess.Write);
                    fs2.Write(new byte[] {0x48, 0x4F, 0x47, 0x50, 0x3, 0, 0, 0}, 0, 8);
                    fs2.Write(new byte[8], 0, 8);
                    long datacount = 0;
                    uint memBlockPos = 0;
                    for (int i = 0; i < file.filelist.Count; i++)
                    {
                        HOGPFile.FileEntry en = file.filelist[i];
                        pb1.Value = i;
                        status.Text = en.name;
                        Application.DoEvents();
                        if ((long)en.offset == -1)
                            continue;
                        Helper.StreamCopy(fs, fs2, (long)en.offset, (int)en.sizeComp);
                        en.offset = (ulong)datacount + 16;
                        en.blockMapStart = memBlockPos;
                        memBlockPos += (uint)en.blockSizes.Count;
                        datacount += en.sizeComp;
                        file.filelist[i] = en;
                    }
                    fs2.Seek(8, 0);
                    fs2.Write(BitConverter.GetBytes(datacount), 0, 8);
                    fs2.Seek(0, SeekOrigin.End);
                    file.WriteTOC(fs2);
                    fs2.Close();
                    fs.Close();
                    File.Delete(d.FileName);
                    File.Move(d.FileName + ".tmp", d.FileName);
                    status.Text = "";
                    pb1.Value = 0;
                    MessageBox.Show("Done.");
                }
                catch (Exception ex)
                {
                    Helper.Log("Error: " + ex.Message);
                }
            }
        }

        private void exportSelectedAsBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int n = listBox1.SelectedIndex;
            if (n == -1 || file == null)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            SaveFileDialog d = new SaveFileDialog();
            d.FileName = Path.GetFileNameWithoutExtension(en.name) + ".bmp";
            d.Filter = "*.bmp|*.bmp";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if (File.Exists("preview.dds"))
                    File.Delete("preview.dds");
                if (File.Exists("preview.bmp"))
                    File.Delete("preview.bmp");
                File.WriteAllBytes("preview.dds", file.ReadFile(en));
                Helper.Run("texconv.exe", " preview.dds -ft bmp");
                if (File.Exists("preview.dds"))
                    File.Delete("preview.dds");
                if (File.Exists("preview.bmp"))
                    File.Move("preview.bmp", d.FileName);
                MessageBox.Show("Done");
            }
        }

        private void createModForSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            HOGPFile.FileEntry en = file.filelist[n];
            OpenFileDialog d = new OpenFileDialog();
            d.FileName = Path.GetFileName(en.name);
            d.Filter = "*.*|*.*";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] buff = File.ReadAllBytes(d.FileName);
                ModManager.modlist.Add(new Mod(Path.GetFileName(file.filename), en, buff));
                MessageBox.Show("Done.");
            }
        }

        private void openModToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModMan mm = new ModMan();
            mm.ShowDialog();
        }
    }
}
