using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtIO_FS_Win
{
    public class FSProcess
    {
        public readonly string mountPoint;
        public readonly string debugLogFile;
        public readonly string debugFlags;
        public readonly string tag;
        public readonly bool caseInsensitive;

        public readonly string args;

        private readonly Process process = new Process();
        private static string MakeStrArg(string arg, string key)
        {
            return !string.IsNullOrEmpty(arg) ? $"{key} {arg}" : "";
        }

        private static string MakeBoolArg(bool arg, string key)
        {
            return arg ? key : "";
        }

        public FSProcess(string binaryPath, string mountPoint, string debugLogFile, string debugFlags, string tag, bool caseInsensitive)
        {
            this.mountPoint = mountPoint;
            this.debugLogFile = debugLogFile;
            this.debugFlags = debugFlags;
            this.tag = tag;
            this.caseInsensitive = caseInsensitive;

            string mountPointArg = MakeStrArg(mountPoint, "-m");
            string debugLogFileArg = MakeStrArg(debugLogFile, "-D");
            string debugFlagsArg = MakeStrArg(debugFlags, "-d");
            string tagArg = MakeStrArg(tag, "-t");
            string caseInsensitiveArg = MakeBoolArg(caseInsensitive, "-i");

            args = $"{mountPointArg} {debugLogFileArg} {debugFlagsArg} {tagArg} {caseInsensitiveArg}";

            process.StartInfo.FileName = binaryPath;
            process.StartInfo.Arguments = args;
        }

        public bool Start()
        {
            return process.Start();
        }

        public void Kill()
        {
            process.Kill();
        }

        override public string ToString()
        {
            return $"virtiofs {args}";
        }
    }
}
