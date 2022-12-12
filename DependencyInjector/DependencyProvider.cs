namespace DependencyInjector
{
    public class DependencyProvider
    {
        public DependenciesConfiguration DConfig { get; set; }
        public Stack<Type> RecursionStack { get; set; }
        public DependencyProvider(DependenciesConfiguration _DConfig)
        {
            DConfig = _DConfig;
            RecursionStack = new Stack<Type>();
        }
        public TDependency Resolve<TDependency>()
        {
            return (TDependency)Resolve(typeof(TDependency));
        }
        private object? Resolve(Type tDependency)
        {
            if (RecursionStack.Contains(tDependency))
            {
                throw new StackOverflowException("Recursion");
            }
            RecursionStack.Push(tDependency);

            if (!DConfig.DepImplDictionary.Any())
            {
                return null;
            }

            object? result;
            if (tDependency.IsGenericType && 
                tDependency.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                Type argumentType = tDependency.GetGenericArguments()[0];
                var implementations = new List<Implementation>(DConfig.GetImplementationsForDependency(argumentType));
                var createdArguments = (object[])Activator.CreateInstance(argumentType.MakeArrayType(), new object[] { implementations.Count });
                for (var i = 0; i < implementations.Count; i++)
                {
                    createdArguments[i] = CreateObjectOrGetObjectIfSingleton(implementations[i]);
                }
                result = createdArguments;
            }
            else
            {
                var implementations = new List<Implementation>();
                if (tDependency.IsGenericType && 
                    DConfig.DepImplDictionary
                        .ContainsKey(tDependency.GetGenericTypeDefinition()))
                {
                    implementations = new List<Implementation>(DConfig.GetImplementationsForDependency(tDependency.GetGenericTypeDefinition()));
                }
                else
                {
                    if (DConfig.DepImplDictionary.ContainsKey(tDependency))
                    {
                        implementations = new List<Implementation>(DConfig.GetImplementationsForDependency(tDependency));
                    }
                }
                if (implementations.Any())
                {
                    result = CreateObjectOrGetObjectIfSingleton(implementations[0]);
                }
                else
                {
                    result = CreateUsingConstructor(tDependency);
                }
            }
            RecursionStack.Pop();
            return result;
        }

        private object CreateObjectOrGetObjectIfSingleton(Implementation implementation)
        {
            if (implementation.IsSingleton)
            {
                if (implementation.SingletonObject == null)
                {
                    lock (implementation)
                    {
                        if (implementation.SingletonObject == null)
                        {
                            implementation.SingletonObject = Resolve(implementation.Type);
                            return implementation.SingletonObject;
                        }
                    }
                }
                return implementation.SingletonObject;
            }
            else
            {
                return Resolve(implementation.Type);
            }
        }
        private object CreateUsingConstructor(Type type)
        {
            object? result = null;

            if (type.IsGenericType)
            {
                var genericArguments = type.GetGenericArguments();
                var genericParams = genericArguments.Select(dependency =>
                {
                    var implementations = DConfig
                        .GetImplementationsForDependency(dependency.BaseType)?.ToArray();
                    if (implementations == null)
                    {
                        return dependency.BaseType;
                    }
                    else
                    {
                        return implementations.First().Type;
                    }
                }).ToArray();

                type = type.MakeGenericType(genericParams);
            }

            var constructorsInfo = type.GetConstructors();
            foreach (var constructorInfo in constructorsInfo)
            {
                var parameters = new List<object>();

                try
                {
                    var paramsInfo = constructorInfo.GetParameters();
                    foreach (var paramInfo in paramsInfo)
                    {
                        parameters.Add(Resolve(paramInfo.ParameterType));
                    }

                    result = Activator.CreateInstance(type, parameters.ToArray());
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (StackOverflowException e) 
                {
                    throw e;
                }
                catch { }
            }
            return result;
        }
    }
}
