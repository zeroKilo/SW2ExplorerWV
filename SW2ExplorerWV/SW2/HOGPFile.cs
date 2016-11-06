using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace SW2ExplorerWV
{
    public class HOGPFile
    {
        public class FileEntry
        {
            public string name;
            public UInt32 sizeUncomp;
            public UInt32 sizeComp;
            public UInt32 blockMapStart;
            public UInt64 offset;
            public UInt64 _address;
            public List<ushort> blockSizes;
            public FileEntry(UInt64 pos, string s, UInt32 u1, UInt32 u2, UInt32 u3, UInt64 u4)
            {
                _address = pos;
                name = s;
                sizeUncomp = u1;
                sizeComp = u2;
                blockMapStart = u3;
                offset = u4;
                blockSizes = new List<ushort>();
            }

            public static FileEntry ReadEntry(Stream s)
            {
                return new FileEntry((ulong)s.Position, Helper.ReadU16String(s), Helper.ReadU32(s), Helper.ReadU32(s), Helper.ReadU32(s), Helper.ReadU64(s));
            }
        }


        public List<FileEntry> filelist;
        public List<ushort> blockSizeMap;
        public string filename;

        public HOGPFile(string path)
        {
            filelist = new List<FileEntry>();
            filename = path;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            UInt64 magic = Helper.ReadU64(fs);
            if (magic != 0x350474F48)
                throw new Exception("not a HOGP3 file!");
            Helper.Log("Magic ok");
            UInt64 datasize = Helper.ReadU64(fs);
            Helper.Log("Datasize: " + datasize.ToString("X") + "h bytes");
            fs.Seek((long)datasize, SeekOrigin.Current);
            UInt32 tocsize = Helper.ReadU32(fs);
            Helper.Log("TOC size " + tocsize.ToString("X") + "h bytes");
            UInt32 filecount = Helper.ReadU32(fs);
            Helper.Log("Found " + filecount + " files");
            filelist = new List<FileEntry>();
            for (int i = 0; i < filecount; i++)
            {
                FileEntry e = FileEntry.ReadEntry(fs);
                filelist.Add(e);
            }
            UInt32 mapsize = Helper.ReadU32(fs);
            blockSizeMap = new List<ushort>();
            for (int i = 0; i < mapsize; i++)
                blockSizeMap.Add(Helper.ReadU16(fs));
            for (int i = 0; i < filecount; i++)
            {
                FileEntry e = filelist[i];
                uint blocks = e.sizeUncomp / 0x10000;
                if ((e.sizeUncomp % 0x10000) != 0)
                    blocks++;
                for (int j = 0; j < blocks; j++)
                    e.blockSizes.Add(blockSizeMap[(int)e.blockMapStart + j]);
                filelist[i] = e;
            }
            fs.Close();
        }

        public byte[] ReadFile(FileEntry e)
        {
            try
            {
                byte[] buff = new byte[e.sizeComp];
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                if (e.offset == 0xffffffffffffffff)
                    return new byte[0];
                if (e.sizeUncomp == e.sizeComp)
                {
                    fs.Seek((long)e.offset, SeekOrigin.Begin);
                    fs.Read(buff, 0, (int)e.sizeComp);
                    fs.Close();
                    return buff;
                }
                else
                {
                    int blocks = (int)(e.sizeUncomp / 0x10000);
                    if ((e.sizeUncomp % 0x10000) != 0)
                        blocks++;
                    buff = DeflateData(fs, (long)e.offset, (int)e.blockMapStart, blocks);
                    fs.Close();
                    return buff;
                }
            }
            catch (Exception ex)
            {
                Helper.Log("Error extracting file " + e.name + " : " + ex.Message);
            }
            return new byte[0];
        }

        public void ExtractFile(FileEntry e, string path)
        {
            try
            {
                if (e.sizeComp != e.sizeUncomp)
                {
                    byte[] buff = new byte[e.sizeComp];
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    if (e.offset == 0xffffffffffffffff)
                        return;
                    int blocks = (int)(e.sizeUncomp / 0x10000);
                    if ((e.sizeUncomp % 0x10000) != 0)
                        blocks++;
                    buff = DeflateData(fs, (long)e.offset, (int)e.blockMapStart, blocks);
                    fs.Close();
                    File.WriteAllBytes(path, buff);
                }
                else
                {
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    FileStream fs2 = new FileStream(path, FileMode.Create, FileAccess.Write);
                    if (e.offset == 0xffffffffffffffff)
                        return;
                    Helper.StreamCopy(fs, fs2, (long)e.offset, (int)e.sizeUncomp);
                    fs2.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Helper.Log("Error extracting file " + e.name + " : " + ex.Message);
            }
        }

        public void ImportFile(int index, byte[] data)
        {
            FileEntry e = filelist[index];
            int blocks = (int)(e.sizeUncomp / 0x10000);
            if ((e.sizeUncomp % 0x10000) != 0)
                blocks++;
            e.blockSizes = new List<ushort>();
            for (int i = 0; i < blocks; i++)
                e.blockSizes.Add(0);
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            fs.Seek(8, 0);
            long datasize = (long)Helper.ReadU64(fs);
            fs.Seek(datasize + 0x10, 0);
            e.offset = (ulong)fs.Position;
            fs.Write(data, 0, data.Length);
            datasize += data.Length;
            fs.Seek(8, 0);
            fs.Write(BitConverter.GetBytes(datasize), 0, 8);
            fs.Seek(datasize + 0x10, 0);
            e.sizeComp = e.sizeUncomp = (uint)data.Length;
            filelist[index] = e;
            WriteTOC(fs);
            fs.Close();
        }

        public void WriteTOC(Stream s)
        {
            MemoryStream m = new MemoryStream();
            m.Write(BitConverter.GetBytes((int)filelist.Count), 0, 4);
            int mapIndex = 0;
            foreach (FileEntry e in filelist)
            {
                m.Write(BitConverter.GetBytes((ushort)e.name.Length), 0, 2);
                foreach (char c in e.name)
                    m.WriteByte((byte)c);
                m.Write(BitConverter.GetBytes(e.sizeUncomp), 0, 4);
                m.Write(BitConverter.GetBytes(e.sizeComp), 0, 4);
                if ((long)e.offset != -1)
                    m.Write(BitConverter.GetBytes(mapIndex), 0, 4);
                else
                    m.Write(BitConverter.GetBytes((int)-1), 0, 4);
                m.Write(BitConverter.GetBytes(e.offset), 0, 8);
                mapIndex += e.blockSizes.Count;
            }
            MemoryStream m2 = new MemoryStream();
            foreach (FileEntry e in filelist)
                foreach (ushort blockSize in e.blockSizes)
                    m2.Write(BitConverter.GetBytes(blockSize), 0, 2);
            m.Write(BitConverter.GetBytes((int)m2.Length / 2), 0, 4);
            m.Write(m2.ToArray(), 0, (int)m2.Length);
            s.Write(BitConverter.GetBytes((int)m.Length), 0, 4);
            s.Write(m.ToArray(), 0, (int)m.Length);
        }

        public byte[] DeflateData(Stream input, long offset, int blockMapStart, int blockCount)
        {
            MemoryStream result = new MemoryStream();
            long pos = offset;
            for (int i = 0; i < blockCount; i++)
            {
                input.Seek(pos, 0);
                MemoryStream output = new MemoryStream();
                using (DeflateStream ds = new DeflateStream(input, CompressionMode.Decompress, true))
                {
                    ds.CopyTo(output);
                    ds.Flush();
                }
                result.Write(output.ToArray(), 0, (int)output.Length);
                pos += blockSizeMap[blockMapStart + i];
            }
            return result.ToArray();
        }
                
    }
}
