using System.IO;

namespace BrainFuck
{
    public interface IBFRunner
    {
        void Execute(Stream input, Stream output);
    }
}