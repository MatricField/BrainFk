using System;
using System.IO;
using System.Linq;
using System.Text;
using BrainFuck.Compilers;
using BrainFuck.Interpreters;
using Lokad.ILPack;

namespace BrainFuck.TestRunner
{
    class Program
    {
        static void Translate()
        {
            var program = Compiler1.Compile(AppResource.dbf2c).Create();
            var memStream = new MemoryStream();
            var writer = new StreamWriter(memStream);
            writer.Write(AppResource.dbf2c);
            writer.Flush();
            memStream.Position = 0;
            program.Execute(memStream, Console.OpenStandardOutput());
        }

        static void Test()
        {
            var program = Compiler1.Compile(AppResource.tests).Create();
            var istream = new MemoryStream();
            var writer = new StreamWriter(istream, Encoding.ASCII);
            writer.Write("\n");
            program.Execute(istream, Console.OpenStandardOutput());
        }

        static void Main(string[] args)
        {
            Translate();
            Test();
            var gen = new AssemblyGenerator();
            gen.GenerateAssembly(CompilerBase1.GeneratedAssembly, @".\out.dll");
        }
    }
}
