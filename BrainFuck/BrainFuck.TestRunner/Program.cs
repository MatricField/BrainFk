using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BrainFuck;

namespace BrainFuck.TestRunner
{
    class Program
    {
        static void BubbleSort()
        {
            const string sortting =
                ">>>>>,+[>>>,+]<<<[<<<[>>>[-<<< -< +>[>] >>] <<<[<] >>[>>> +<<< -] <[> +>>> +<<<< -]<<] >>>[-.[-]] >>>[>>>] <<<]";
            var program = new BFInterpreter(sortting);

            var rand = new Random();
            var input = Enumerable.Repeat(0, 3).Select(_ => rand.Next(0, 50));
            var instream = new MemoryStream();
            foreach (var x in input)
            {
                Console.Write($"{x}; ");
                instream.WriteByte(Convert.ToByte(x));
            }
            instream.Position = 0;
            var outstream = new MemoryStream();
            program.Execute(instream, outstream);
            outstream.Position = 0;
            var output = new ReadOnlySpan<byte>(outstream.GetBuffer()).Slice(Convert.ToInt32(outstream.Length));
            Console.WriteLine();
            foreach(var x in output)
            {
                Console.Write($"{x}; ");
            }
        }

        static void HelloWorld()
        {
            const string HELLO_WORLD_PROGRAM =
                @"++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
            var program = new BFInterpreter(HELLO_WORLD_PROGRAM);
            program.Execute(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        static void Main(string[] args)
        {
            //HelloWorld();
            //BubbleSort();

            const string programSource =
                @"++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
            var factory = BFCompiler1.Compile(programSource);
            var program = factory.Create();
            var gen = new Lokad.ILPack.AssemblyGenerator();
            gen.GenerateAssembly(program.GetType().Assembly, @".\out.dll");
            program.Execute(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }
    }
}
