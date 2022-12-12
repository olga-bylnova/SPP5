namespace Tests.TestClasses
{
    public class ImplWithRecursionDependency : ITestInterface
    {
        public ITestInterface RecursiveDependency { get; private set; }
       
        public ImplWithRecursionDependency(ITestInterface testInterface)
        {
            RecursiveDependency = testInterface;
        }

        public void BaseMethod()
        {
            throw new NotImplementedException();
        }
    }
}
