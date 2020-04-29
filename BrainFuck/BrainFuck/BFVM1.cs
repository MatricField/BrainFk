using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BrainFuck
{
    public abstract class BFVM1 : IBFRunner
    {
        private readonly InfiniArray<byte> _data = new InfiniArray<byte>();

        protected long DataPointer { get; set; }

        protected byte Data
        {
            get => _data[DataPointer];
            set => _data[DataPointer] = value;
        }

        public abstract void Execute(Stream input, Stream output);
    }
}