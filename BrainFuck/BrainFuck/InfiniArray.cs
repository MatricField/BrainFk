using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BrainFuck
{
    sealed class InfiniArray<T>
    {
        private const int DEFAULT_SEG_LEN = 2048;
        private readonly int segLength;

        private List<List<T>> data = new List<List<T>>();

        public InfiniArray(int segLen = DEFAULT_SEG_LEN)
        {
            segLength = segLen;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (int, int) EnsureIndex(long index)
        {
            var seg = Convert.ToInt32(index / segLength);
            var diff = Convert.ToInt32(index % segLength);
            if (seg >= data.Count)
            {
                data.AddRange(Enumerable.Repeat<List<T>>(null, seg - data.Count + 1));
            }
            if (null == data[seg])
            {
                data[seg] = new List<T>();
            }
            if (diff >= data[seg].Count)
            {
                data[seg].AddRange(Enumerable.Repeat(default(T), diff - data[seg].Count + 1));
            }
            return (seg, diff);
        }

        public T this[long i]
        {
            get
            {
                var (seg, diff) = EnsureIndex(i);
                return data[seg][diff];
            }
            set
            {
                var (seg, diff) = EnsureIndex(i);
                data[seg][diff] = value;
            }
        }
    }
}
