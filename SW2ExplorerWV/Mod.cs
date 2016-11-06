using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW2ExplorerWV
{
    public class Mod
    {
        public const uint magic = 0x1C2D4520;
        public string _filename;
        public byte[] _data;
        public string _infilename;
        public Mod(string filename, HOGPFile.FileEntry en, byte[] data)
        {
            _filename = filename;
            _infilename = en.name;
            _data = data;
        }

        public Mod(Stream s)
        {
            _filename = Helper.ReadString(s);
            _infilename = Helper.ReadString(s);
            uint size = Helper.ReadU32(s);
            _data = new byte[size];
            s.Read(_data, 0, (int)size);
        }

        public void WriteToStream(Stream s)
        {
            Helper.WriteString(s, _filename);
            Helper.WriteString(s, _infilename);
            s.Write(BitConverter.GetBytes(_data.Length), 0, 4);
            s.Write(_data,0,_data.Length);
        }

        public static void SaveModFile(string filename, Mod[] list)
        {
            MemoryStream m = new MemoryStream();
            m.Write(BitConverter.GetBytes(magic), 0, 4);
            m.Write(BitConverter.GetBytes((uint)list.Length), 0, 4);
            foreach (Mod mod in list)
                mod.WriteToStream(m);
            File.WriteAllBytes(filename, m.ToArray());
        }

        public static Mod[] LoadModFile(string filename)
        {
            List<Mod> result = new List<Mod>();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            uint test = Helper.ReadU32(fs);
            if (test != magic)
            {
                fs.Close();
                return result.ToArray(); ;
            }
            uint count = Helper.ReadU32(fs);
            for (int i = 0; i < count; i++)
                result.Add(new Mod(fs));
            fs.Close();
            return result.ToArray();
        }
    }
}
