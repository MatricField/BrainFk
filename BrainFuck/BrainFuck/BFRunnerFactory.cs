using System;

namespace BrainFuck
{
    public sealed class BFRunnerFactory
    {
        private readonly Type type;

        public BFRunnerFactory(Type type)
        {
            if(!typeof(IBFRunner).IsAssignableFrom(type))
            {
                throw new ArgumentException();
            }
            this.type = type;
        }

        public IBFRunner Create()
        {
            return (IBFRunner)Activator.CreateInstance(type);
        }
    }
}
