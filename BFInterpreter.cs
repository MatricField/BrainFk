using System.IO;
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
      for(int pc = 0; pc < code.Length; pc++)
      {
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
            output.WriteByte(Data);
            break;
          case ',':
            Data=(byte)input.ReadByte();
            break;
          case '[':
            if(Data==0)
            {
              while(code[pc]!=']')
              {
                pc++;
              }
            }
            break;
          case ']':
            if(Data!=0)
            {
              while(code[pc]!='[')
              {
                pc--;
              }
            }
            break;
        }
      }
    }
  }
}