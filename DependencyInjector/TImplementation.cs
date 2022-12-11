namespace DependencyInjector
{
    public class TImplementation
    {
        public Type Type { get; set; }
        public bool IsSingleton { get; set; }
        public object? Singleton { get; set; }
        public TImplementation(Type tImplementation, bool isSingleton)
        {
            Type = tImplementation;
            IsSingleton = isSingleton;
            Singleton = null;
        }
    }
}
