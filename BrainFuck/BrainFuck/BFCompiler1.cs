using System;
using System.Collections.Generic;
using System.Text;
using static BrainFuck.BFInstructions;

namespace BrainFuck
{
    public sealed class BFCompiler1 :
        BFCompilerBase
    {
        private BFCompiler1(string source) : base(source)
        {
        }

        private BFCompiler1(string source, string name) : base(source, name)
        {
        }

        protected override IEnumerable<BFInstructions> Preprocess(string source)
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

        public static BFRunnerFactory Compile(string source) =>
            new BFCompiler1(source).Result;

        public static BFRunnerFactory Compile(string source, string typeName) =>
            new BFCompiler1(source, typeName).Result;
    }
}
