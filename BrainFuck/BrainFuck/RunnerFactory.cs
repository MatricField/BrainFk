using System;

namespace BrainFuck
{
    public sealed class RunnerFactory
    {
        private readonly Type type;

        public RunnerFactory(Type type)
        {
            if(!typeof(IRunner).IsAssignableFrom(type))
            {
                throw new ArgumentException();
            }
            this.type = type;
        }

        public IRunner Create()
        {
            return (IRunner)Activator.CreateInstance(type);
        }
    }
}
