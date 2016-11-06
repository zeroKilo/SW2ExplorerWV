using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SW2ExplorerWV
{
    public static class Helper
    {
        public static RichTextBox box;
        public static readonly object _sync = new object();

        public static void Log(string s, bool newline = true)
        {
            lock (_sync)
            {
                box.Invoke((MethodInvoker)delegate()
                {
                    box.AppendText(s + (newline ? "\n" : ""));
                });
            }
        }

        public static void Log(string s)
        {
            lock (_sync)
            {
                box.Invoke((MethodInvoker)delegate()
                {
                    box.AppendText(s + "\n");
                });
            }
        }

        public static UInt16 ReadU16(Stream s)
        {
            byte[] buff = new byte[2];
            s.Read(buff, 0, 2);
            return BitConverter.ToUInt16(buff, 0);
        }

        public static UInt32 ReadU32(Stream s)
        {
            byte[] buff = new byte[4];
            s.Read(buff, 0, 4);
            return BitConverter.ToUInt32(buff, 0);
        }
        public static UInt64 ReadU64(Stream s)
        {
            byte[] buff = new byte[8];
            s.Read(buff, 0, 8);
            return BitConverter.ToUInt64(buff, 0);
        }

        public static string ReadU16String(Stream s)
        {
            UInt16 size = ReadU16(s);
            byte[] buff = new byte[size];
            s.Read(buff, 0, size);
            return Encoding.UTF8.GetString(buff);
        }

        public static string ReadSQString(Stream s)
        {
            ushort u1 = ReadU16(s);
            ushort size = ReadU16(s);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
                sb.Append((char)s.ReadByte());
            return sb.ToString();
        }

        public static void WriteString(Stream s, string str)
        {
            s.Write(BitConverter.GetBytes((uint)str.Length), 0, 4);
            foreach (char c in str)
                s.WriteByte((byte)c);
        }

        public static string ReadString(Stream s)
        {
            StringBuilder sb = new StringBuilder();
            uint count = Helper.ReadU32(s);
            for (int i = 0; i < count; i++)
                sb.Append((char)s.ReadByte());
            return sb.ToString();
        }

        public static void Run(string path, string args)
        {
            ProcessStartInfo s = new ProcessStartInfo(path);
            s.Arguments = args;
            s.UseShellExecute = false;
            s.CreateNoWindow = true;
            Process p = new Process();
            p.StartInfo = s;
            p.Start();
            p.WaitForExit();
        }

        public static void StreamCopy(Stream input, Stream output, long offset, int size)
        {
            input.Seek(offset, 0);
            long bytesread = 0;
            int bytestoread = 0x10000000; //16MB
            if (bytestoread > size)
                bytestoread = size;
            int bread;
            byte[] buff = new byte[bytestoread];
            while (bytesread < size)
            {
                bread = input.Read(buff, 0, bytestoread);
                output.Write(buff, 0, bread);
                bytesread += bread;
                if (bytesread < size && size - bytesread < bytestoread)
                    bytestoread = (int)(size - bytesread);
            }
        }

        public static string pathgame = "";

        public static bool FindExeLocation()
        {
            if (pathgame != "")
                return true;
            string keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 324800";
            string valueName = "InstallLocation";
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(keyName))
                {
                    pathgame = (string)key.GetValue(valueName) + "\\";
                    return true;
                }
            }                
            catch (Exception)
            {
                return false;
            }
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "ShadowWarrior2.exe|ShadowWarrior2.exe";
            d.FileName = "ShadowWarrior2.exe";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Helper.pathgame = Path.GetDirectoryName(d.FileName) + "\\";
            else
                return false;
            return true;
        }
    }
}
