using System.IO;

namespace BrainFuck
{
  abstract class BFVM1
  {
    //private readonly InfiniArray<byte> _data = new InfiniArray<byte>();
    private readonly byte[] _data = new byte[3000000];
    
    protected long DataPointer {get; set;}
    protected byte Data 
    {
      get => _data[DataPointer];
      set => _data[DataPointer]= value;
    }
    
    public abstract void Execute(Stream input, Stream output);
  }
}