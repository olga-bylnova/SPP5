namespace DependencyInjector
{
    public class DependenciesConfiguration
    {
        public Dictionary<Type, List<Implementation>> DepImplDictionary { get; set; }

        public DependenciesConfiguration()
        {
            DepImplDictionary = new Dictionary<Type, List<Implementation>>();
        }

        public void Register<TDependency, TImplementation>(bool isSingleton = false)
        {
            Register(typeof(TDependency), typeof(TImplementation), isSingleton);
        }

        public void Register(Type tDependency, Type tImplementation, bool isSingleton = false)
        {
            if (tImplementation.IsAbstract)
            {
                throw new ArgumentException("Registration fail. Implementation is abstract");
            }

            if (!tDependency.IsAssignableFrom(tImplementation)
                && !tDependency.IsGenericTypeDefinition
                && !tImplementation.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Registration fail. Dependency is not assignable from implementation");
            }

            List<Implementation> dependencyImplementations;
            if (!DepImplDictionary.TryGetValue(tDependency, out dependencyImplementations))
            {
                dependencyImplementations = new List<Implementation>();
                DepImplDictionary[tDependency] = dependencyImplementations;
            }
            dependencyImplementations.Add(new Implementation(tImplementation, isSingleton));
        }
        public List<Implementation>? GetImplementationsForDependency(Type tDependency)
        {
            if (DepImplDictionary.ContainsKey(tDependency))
            {
                return DepImplDictionary[tDependency];
            }
            return null;
        }
    }
}