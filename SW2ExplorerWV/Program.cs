using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SW2ExplorerWV
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (CheckCommandline())
                return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            
            Application.Run(new Splash());
            Application.Run(new Form1());
        }

        static bool CheckCommandline()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
                RunModJobs(args[1]);
            return args.Length > 1;
        }

        static void RunModJobs(string filename)
        {
            ModManager.modlist = new List<Mod>(Mod.LoadModFile(filename));
            foreach (Mod mod in ModManager.modlist)
            {
                string name = mod._filename;
                string inname = mod._infilename;
                if (!Helper.FindExeLocation())
                    return;
                if (!File.Exists(Helper.pathgame + name))
                    return;
                HOGPFile hogp = new HOGPFile(Helper.pathgame + name, true);
                for (int i = 0; i < hogp.filelist.Count; i++)
                    if (hogp.filelist[i].name == inname)
                        hogp.ImportFile(i, mod._data);
            }
        }
    }
}
