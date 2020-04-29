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
        static void BubbleSort()
        {
            var rand = new Random();
            var input = Enumerable.Repeat(0, 1000).Select(_ => rand.Next(0, byte.MaxValue - 1));
            var instream = new MemoryStream();
            foreach (var x in input)
            {
                instream.WriteByte((byte)x);
            }
            foreach (var x in input.Take(50))
            {
                Console.Write($"{x}; ");
            }

            instream.Position = 0;

            Console.WriteLine();

            var outstream = new MemoryStream();

            var program = Compiler1.Compile(AppResource.BSort).Create();
            program.Execute(instream, outstream);

            outstream.Position = 0;
            var output = new ReadOnlySpan<byte>(outstream.GetBuffer()).Slice(0, Convert.ToInt32(50));
            Console.WriteLine();
            foreach(var x in output)
            {
                Console.Write($"{x}; ");
            }
        }

        static void HelloWorld()
        {
            var program = Compiler1.Compile(AppResource.HelloWorld).Create();
            program.Execute(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

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

        static void Main(string[] args)
        {
            HelloWorld();
            Console.WriteLine();
            BubbleSort();
            Console.WriteLine();
            Translate();
            var gen = new AssemblyGenerator();
            gen.GenerateAssembly(Compiler1.Compile("").Create().GetType().Assembly, @".\out.dll");
        }
    }
}
