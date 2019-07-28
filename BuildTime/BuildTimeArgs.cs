using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildTime
{
    class BuildTimeArgs
    {
        [Value(0)]
        public string OutputFile { get; set; } = null;

        [Option('n')]
        public string Namespace { get; set; } = null;

        [Option('c')]
        public string Class { get; set; } = "_BuildTime";

        [Option('p')]
        public bool Public { get; set; }
    }
}
