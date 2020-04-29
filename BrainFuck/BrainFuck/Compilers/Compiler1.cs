using System;
using System.Collections.Generic;
using System.Text;
using static BrainFuck.Instructions;

namespace BrainFuck.Compilers
{
    public sealed class Compiler1 :
        CompilerBase1
    {
        private Compiler1(string source) : base(source)
        {
        }

        private Compiler1(string source, string name) : base(source, name)
        {
        }

        protected override IEnumerable<Instructions> Preprocess(string source)
        {
            foreach(var c in source)
            {
                switch(c)
                {
                    case '>':
                        yield return RShift;
                        break;
                    case '<':
                        yield return LShift;
                        break;
                    case '+':
                        yield return Inc;
                        break;
                    case '-':
                        yield return Dec;
                        break;
                    case '.':
                        yield return Write;
                        break;
                    case ',':
                        yield return Read;
                        break;
                    case '[':
                        yield return LBracket;
                        break;
                    case ']':
                        yield return RBracket;
                        break;
                    default:
                        break;
                }
            }
        }

        public static RunnerFactory Compile(string source) =>
            new Compiler1(source).Result;

        public static RunnerFactory Compile(string source, string typeName) =>
            new Compiler1(source, typeName).Result;
    }
}
