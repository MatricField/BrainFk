using System;
using System.Collections.Generic;
using System.Linq;

namespace BrainFuck
{
  public sealed class InfiniArray<T>
  {
    private const int SEG_LEN = 2048;
    private List<List<T>> data = new List<List<T>>();
    
    private void EnsureIndex(long index)
    {
      var seg = Convert.ToInt32(index/SEG_LEN);
      var diff = Convert.ToInt32(index%SEG_LEN);
      if(seg >= data.Count)
      {
        data.AddRange(Enumerable.Repeat<List<T>>(null, seg - data.Count + 1));
      }
      if(null == data[seg])
      {
        data[seg] = new List<T>();
      }
      if(diff >= data[seg].Count)
      {
        data[seg].AddRange(Enumerable.Repeat(default(T), diff - data[seg].Count + 1));
      }
    }
    
    public T this[long i]
    {
      get
      {
        EnsureIndex(i);
        var seg = Convert.ToInt32(i/SEG_LEN);
        var diff = Convert.ToInt32(i%SEG_LEN);
        return data[seg][diff];
      }
      set
      {
        EnsureIndex(i);
        var seg = Convert.ToInt32(i/SEG_LEN);
        var diff = Convert.ToInt32(i%SEG_LEN);
        data[seg][diff]=value;
      }
    }
  }
}
