namespace DependencyInjector
{
    public class Implementation
    {
        public Type Type { get; set; }
        public bool IsSingleton { get; set; }
        public object? SingletonObject { get; set; }
        public Implementation(Type implementation, bool _IsSingleton)
        {
            Type = implementation;
            IsSingleton = _IsSingleton;
            SingletonObject = null;
        }
    }
}
