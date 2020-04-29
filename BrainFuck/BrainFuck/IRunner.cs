using System.IO;

namespace BrainFuck
{
    public interface IRunner
    {
        void Execute(Stream input, Stream output);
    }
}