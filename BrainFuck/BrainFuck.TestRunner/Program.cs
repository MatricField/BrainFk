using System;
using System.Diagnostics;
using BrainFuck;

namespace BrainFuck.TestRunner
{
    class Program
    {
        const string HELLO_WORLD_PROGRAM =
            @"++++++++[>++++[>++>+++>+++>+<<<<-]>+>+>->>+[<]<-]>>.>---.+++++++..+++.>>.<-.<.+++.------.--------.>>+.>++.";
        static void Main(string[] args)
        {
            var program = new BFInterpreter(HELLO_WORLD_PROGRAM);
            program.Execute(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }
    }
}
