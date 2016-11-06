using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW2ExplorerWV
{
    public class NutFile
    {
        string[] OpcodeNames = 
        {
	        "OP_LINE",
	        "OP_LOAD",
	        "OP_LOADINT",
	        "OP_LOADFLOAT",
	        "OP_DLOAD",
	        "OP_TAILCALL",
	        "OP_CALL",
	        "OP_PREPCALL",
	        "OP_PREPCALLK",
	        "OP_GETK",
	        "OP_MOVE",
	        "OP_NEWSLOT",
	        "OP_DELETE",
	        "OP_SET",
	        "OP_GET",
	        "OP_EQ",
	        "OP_NE",
	        "OP_ARITH",
	        "OP_BITW",
	        "OP_RETURN",
	        "OP_LOADNULLS",
	        "OP_LOADROOTTABLE",
	        "OP_LOADBOOL",
	        "OP_DMOVE",
	        "OP_JMP",
	        "OP_JNZ",
	        "OP_JZ",
	        "OP_LOADFREEVAR",
	        "OP_VARGC",
	        "OP_GETVARGV",
	        "OP_NEWTABLE",
	        "OP_NEWARRAY",
	        "OP_APPENDARRAY",
	        "OP_GETPARENT",
	        "OP_COMPARITH",
	        "OP_COMPARITHL",
	        "OP_INC",
	        "OP_INCL",
	        "OP_PINC",
	        "OP_PINCL",
	        "OP_CMP",
	        "OP_EXISTS",
	        "OP_INSTANCEOF",
	        "OP_AND",
	        "OP_OR",
	        "OP_NEG",
	        "OP_NOT",
	        "OP_BWNOT",
	        "OP_CLOSURE",
	        "OP_YIELD",
	        "OP_RESUME",
	        "OP_FOREACH",
	        "OP_POSTFOREACH",
	        "OP_DELEGATE",
	        "OP_CLONE",
	        "OP_TYPEOF",
	        "OP_PUSHTRAP",
	        "OP_POPTRAP",
	        "OP_THROW",
	        "OP_CLASS",
	        "OP_NEWSLOTA"
        };
        public class locVarInfo
        {
            public string name;
            public ushort flags;
            public UInt64 u1;
            public UInt64 u2;
            public UInt64 u3;
            public locVarInfo(Stream s)
            {
                name = Helper.ReadSQString(s);
                flags = Helper.ReadU16(s);
                u1 = Helper.ReadU64(s);
                u2 = Helper.ReadU64(s);
                u3 = Helper.ReadU64(s);
            }
        }
        public class lineInfo
        {
            public UInt64 line;
            public UInt64 op;
            public lineInfo(Stream s)
            {
                line = Helper.ReadU64(s);
                op = Helper.ReadU64(s);
            }
        }
        public class instruction
        {
            public byte op;
            public byte arg1;
            public byte arg2;
            public byte arg3;
            public instruction(Stream s)
            {
                op = (byte)s.ReadByte();
                arg1 = (byte)s.ReadByte();
                arg2 = (byte)s.ReadByte();
                arg3 = (byte)s.ReadByte();
            }
        }
        public string scriptName;
        public string mainName;
        public ulong nLiterals;
        public ulong nParameters;
        public ulong nOuterValues;
        public ulong nLocalVarInfos;
        public ulong nLineInfos;
        public ulong nDefaultParams;
        public ulong nInstructions;
        public ulong nFunctions;
        public List<string> literals;
        public List<string> parameter;
        public List<locVarInfo> localVarInfos;
        public List<lineInfo> lineInfos;
        public List<instruction> instructions;

        public void ReadNutFile(Stream m)
        {
            try
            {
                Expect(Helper.ReadU16(m), 0xFAFA);
                Expect(Helper.ReadU64(m), 0x53514952);
                Expect(Helper.ReadU64(m), 1);
                Expect(Helper.ReadU64(m), 0x50415254);
                scriptName = Helper.ReadSQString(m);
                mainName = Helper.ReadSQString(m);
                Expect(Helper.ReadU64(m), 0x50415254);
                nLiterals = Helper.ReadU64(m);
                nParameters = Helper.ReadU64(m);
                nOuterValues = Helper.ReadU64(m);
                nLocalVarInfos = Helper.ReadU64(m);
                nLineInfos = Helper.ReadU64(m);
                nDefaultParams = Helper.ReadU64(m);
                nInstructions = Helper.ReadU64(m);
                nFunctions = Helper.ReadU64(m);
                Expect(Helper.ReadU64(m), 0x50415254);

                literals = new List<string>();
                for (int i = 0; i < (int)nLiterals; i++)
                    literals.Add(Helper.ReadSQString(m));
                Expect(Helper.ReadU64(m), 0x50415254);

                parameter = new List<string>();
                for (int i = 0; i < (int)nParameters; i++)
                    parameter.Add(Helper.ReadSQString(m));
                Expect(Helper.ReadU64(m), 0x50415254);

                if (nOuterValues != 0)
                    throw new Exception();
                Expect(Helper.ReadU64(m), 0x50415254);

                localVarInfos = new List<locVarInfo>();
                for (int i = 0; i < (int)nLocalVarInfos; i++)
                    localVarInfos.Add(new locVarInfo(m));
                Expect(Helper.ReadU64(m), 0x50415254);

                lineInfos = new List<lineInfo>();
                for (int i = 0; i < (int)nLineInfos; i++)
                    lineInfos.Add(new lineInfo(m));
                Expect(Helper.ReadU64(m), 0x50415254);

                if (nDefaultParams != 0)
                    throw new Exception();
                Expect(Helper.ReadU64(m), 0x50415254);

                instructions = new List<instruction>();
                for (int i = 0; i < (int)nInstructions; i++)
                    instructions.Add(new instruction(m));
                Expect(Helper.ReadU64(m), 0x50415254);
            }
            catch (Exception)
            {
            }
        }

        public NutFile(string path)
        {
            ReadNutFile(new MemoryStream(File.ReadAllBytes(path)));
        }

        public NutFile(Stream s)
        {
            ReadNutFile(s);
        }

        private void Expect(ulong a, ulong b)
        {
            if (a != b) throw new Exception();
        }
        public string Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Script Name\t:{0}\n", scriptName);
            sb.AppendFormat("Main Name\t:{0}\n", mainName);
            sb.AppendFormat("Literals\t:{0}\n", nLiterals);
            sb.AppendFormat("Parameters\t:{0}\n", nParameters);
            sb.AppendFormat("OuterValues\t:{0}\n", nOuterValues);
            sb.AppendFormat("LocalVarInfos\t:{0}\n", nLocalVarInfos);
            sb.AppendFormat("LineInfos\t:{0}\n", nLineInfos);
            sb.AppendFormat("DefaultParams\t:{0}\n", nDefaultParams);
            sb.AppendFormat("Instructions\t:{0}\n", nInstructions);
            sb.AppendFormat("Functions\t:{0}\n", nFunctions);
            sb.AppendLine("Literals");
            for (int i = 0; i < literals.Count; i++)
                sb.AppendFormat(" {0}\t:{1}\n", i.ToString("X8"), literals[i]);
            sb.AppendLine("Parameter");
            for (int i = 0; i < parameter.Count; i++)
                sb.AppendFormat(" {0}\t:{1}\n", i.ToString("X8"), parameter[i]);
            sb.AppendLine("Local Variable Info");
            for (int i = 0; i < localVarInfos.Count; i++)
                sb.AppendFormat(" {0}\t:{1} {2} {3} {4} {5}\n",
                                 i.ToString("X8"),
                                 localVarInfos[i].flags.ToString("X2"),
                                 localVarInfos[i].u1.ToString("X16"),
                                 localVarInfos[i].u2.ToString("X16"),
                                 localVarInfos[i].u3.ToString("X16"),
                                 localVarInfos[i].name);
            sb.AppendLine("Line Info");
            for (int i = 0; i < lineInfos.Count; i++)
                sb.AppendFormat(" {0}\t:Line = {1} Opcode = {2}\n",
                                 i.ToString("X8"),
                                 lineInfos[i].line.ToString("X16"),
                                 lineInfos[i].op.ToString("X16"));
            sb.AppendLine("Instructions");
            for (int i = 0; i < instructions.Count; i++)
                sb.AppendFormat(" {0}\t: {1} {2} {3} {4} {5}\n",
                                 i.ToString("X8"),
                                 instructions[i].op.ToString("X2"),
                                 instructions[i].arg1.ToString("X2"),
                                 instructions[i].arg2.ToString("X2"),
                                 instructions[i].arg3.ToString("X2"),
                                 instructions[i].op < OpcodeNames.Length ? OpcodeNames[instructions[i].op] : "");
            return sb.ToString();
        }
    }
}
