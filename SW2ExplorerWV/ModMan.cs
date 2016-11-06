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
    public partial class ModMan : Form
    {
        public ModMan()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            string name = listBox1.Items[n].ToString();
            listBox2.Items.Clear();
            foreach (Mod mod in ModManager.modlist)
                if (mod._filename == name)
                    listBox2.Items.Add(mod._infilename);
        }

        private void exportJobDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            int m = listBox2.SelectedIndex;
            if (n == -1 || m == -1)
                return;
            string name = listBox1.Items[n].ToString();
            string inname = listBox2.Items[m].ToString();
            foreach (Mod mod in ModManager.modlist)
                if (mod._filename == name && mod._infilename == inname)
                {
                    SaveFileDialog d = new SaveFileDialog();
                    d.FileName = Path.GetFileName(inname);
                    d.Filter = "*.*|*.*";
                    if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        File.WriteAllBytes(d.FileName, mod._data);
                        MessageBox.Show("Done.");
                        return;
                    }
                }
            
        }

        private void saveModJobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = "*.mod|*.mod";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mod.SaveModFile(d.FileName, ModManager.modlist.ToArray());
                MessageBox.Show("Done.");
            }
        }

        private void ModMan_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void RefreshList()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            List<string> names = new List<string>();
            foreach (Mod mod in ModManager.modlist)
            {
                bool found = false;
                foreach(string name in names)
                    if (name == mod._filename)
                    {
                        found = true;
                        break;
                    }
                if (!found)
                    names.Add(mod._filename);
            }
            foreach (string name in names)
                listBox1.Items.Add(name);
        }

        private void loadModJobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.mod|*.mod";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModManager.modlist = new List<Mod>(Mod.LoadModFile(d.FileName));
                RefreshList();
            }
        }

        private void addModJobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "*.mod|*.mod";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ModManager.modlist.AddRange(new List<Mod>(Mod.LoadModFile(d.FileName)));
                RefreshList();
            }

        }

        private void executeSelectedJobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int n = listBox1.SelectedIndex;
            int m = listBox2.SelectedIndex;
            if (n == -1 || m == -1)
                return;
            string name = listBox1.Items[n].ToString();
            string inname = listBox2.Items[m].ToString();
            foreach (Mod mod in ModManager.modlist)
                if (mod._filename == name && mod._infilename == inname)
                {
                    if (!Helper.FindExeLocation())
                        return;
                    if (!File.Exists(Helper.pathgame + name))
                    {
                        Helper.Log("File \"" + name + "\" not found!");
                        return;
                    }
                    HOGPFile hogp = new HOGPFile(Helper.pathgame + name);
                    for (int i = 0; i < hogp.filelist.Count; i++)
                    {
                        HOGPFile.FileEntry en = hogp.filelist[i];
                        if (en.name == inname)
                        {
                            hogp.ImportFile(i, mod._data);
                            MessageBox.Show("Done.");
                            return;
                        }
                    }

                }
        }

        private void executeAllJobsForThisFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = listBox1.SelectedIndex;
            if (n == -1)
                return;
            string name = listBox1.Items[n].ToString();
            foreach (Mod mod in ModManager.modlist)
                if (mod._filename == name)
                {
                    string inname = mod._infilename;
                    if (!Helper.FindExeLocation())
                        return;
                    if (!File.Exists(Helper.pathgame + name))
                    {
                        Helper.Log("File \"" + name + "\" not found!");
                        return;
                    }
                    HOGPFile hogp = new HOGPFile(Helper.pathgame + name);
                    for (int i = 0; i < hogp.filelist.Count; i++)
                    {
                        HOGPFile.FileEntry en = hogp.filelist[i];
                        if (en.name == inname)
                            hogp.ImportFile(i, mod._data);
                    }
                }
            MessageBox.Show("Done.");
            return;
        }

        private void executeAllJobsForAllFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Mod mod in ModManager.modlist)
                {
                    string name = mod._filename;
                    string inname = mod._infilename;
                    if (!Helper.FindExeLocation())
                        return;

                    if (!File.Exists(Helper.pathgame + name))
                    {
                        Helper.Log("File \"" + name + "\" not found!");
                        return;
                    }
                    HOGPFile hogp = new HOGPFile(Helper.pathgame + name);
                    for (int i = 0; i < hogp.filelist.Count; i++)
                    {
                        HOGPFile.FileEntry en = hogp.filelist[i];
                        if (en.name == inname)
                            hogp.ImportFile(i, mod._data);
                    }
                }
            MessageBox.Show("Done.");
            return;
        }
    }
}
