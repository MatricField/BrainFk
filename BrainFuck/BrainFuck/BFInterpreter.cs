using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BrainFuck
{
    public class BFInterpreter :
      BFVM1
    {
        private string code;

        private Dictionary<int, int> matchingRBracket = new Dictionary<int, int>();

        private Dictionary<int, int> matchingLBracket = new Dictionary<int, int>();

        protected HashSet<char> InstructionSet { get; } =
            new HashSet<char>()
            { '>', '<', '+','-','.',',','[',']'};

        public BFInterpreter(string code)
        {
            var cleanCode =
                new string (code.Where(x => InstructionSet.Contains(x)).ToArray());

            var stack = new Stack<int>();
            var pc = 0;
            foreach(var c in cleanCode)
            {
                switch(c)
                {
                    case '[':
                        stack.Push(pc);
                        break;
                    case ']':
                        var l = stack.Pop();
                        matchingLBracket[pc] = l;
                        matchingRBracket[l] = pc;
                        break;
                }
                pc++;
            }
            this.code = cleanCode;
        }

        public override void Execute(Stream input, Stream output)
        {
            for (int pc = 0; pc < code.Length; pc++)
            {
                Debug.WriteLine($"dp:{DataPointer} d:{Data} pc:{pc} code:{code[pc]}");
                switch (code[pc])
                {
                    case '>':
                        DataPointer++;
                        break;
                    case '<':
                        DataPointer--;
                        break;
                    case '+':
                        Data++;
                        break;
                    case '-':
                        Data--;
                        break;
                    case '.':
                        output.WriteByte(Data);
                        break;
                    case ',':
                        var tmp = input.ReadByte();
                        Data = -1 == tmp ? (byte)0 : (byte)tmp;
                        break;
                    case '[':
                        if (Data == 0)
                        {
                            pc = matchingRBracket[pc];
                        }
                        break;
                    case ']':
                        if (Data != 0)
                        {
                            pc = matchingLBracket[pc];
                        }
                        break;
                    default:
                        break;
                }
            }
            Debug.WriteLine("End of Program");
        }
    }
}