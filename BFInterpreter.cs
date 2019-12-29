using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace BrainFuck
{
  class BFInterpreter :
    BFVM1
  {
    private string code;
    
    public BFInterpreter(string code)
    {
      this.code = code;
    }
    
    public override void Execute(Stream input, Stream output)
    {
      var read = new StreamReader(input);
      var write = new StreamWriter(output);
      write.AutoFlush = true;
      var stack = new Stack<int>();
      for(int pc = 0; pc < code.Length; pc++)
      {
        write.WriteLine($"dp:{DataPointer} d:{Data} pc:{pc} code:{code[pc]}");
        switch(code[pc])
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
            write.Write((char)Data);
            break;
          case ',':
            Data=(byte)read.Read();
            break;
          case '[':
            if(Data==0)
            {
              while(code[pc]!=']')
              {
                pc++;
              }
            }
            else
            {
              stack.Push(pc+1);
              write.WriteLine($"pushed {pc+1}");
            }
            break;
          case ']':
            if(Data!=0)
            {
              pc = stack.Peek();
            }
            else
            {
              var x = stack.Pop();
              write.WriteLine($"poped {x}");
            }
            break;
          default:
            break;
        }
      }
      write.WriteLine("End of Program");
    }
  }
}