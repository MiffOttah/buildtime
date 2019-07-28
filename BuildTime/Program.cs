using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace BuildTime
{
    class Program
    {
        static int Main(string[] args)
        {
            int rc = 2;

            Parser.Default.ParseArguments<BuildTimeArgs>(args)
                .WithParsed(a => rc = Main2(a))
                .WithNotParsed(e => Errors(e));

            return rc;
        }

        private static void Errors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                Console.Error.WriteLine(error.ToString());
            }
        }

        private static int Main2(BuildTimeArgs args)
        {
            try
            {
                string outFile = args.OutputFile;
                if (string.IsNullOrEmpty(outFile))
                {
                    outFile = Path.Combine(Environment.CurrentDirectory, (args.Class ?? "_BuildTime") + ".cs");
                }

                string code = CodeGen.GenerateCode(args, DateTimeOffset.Now);
                File.WriteAllText(outFile, code);

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }
    }
}
