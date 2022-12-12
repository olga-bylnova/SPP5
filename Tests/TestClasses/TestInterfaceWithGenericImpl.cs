using System.Collections;

namespace Tests.TestClasses
{
    public class TestInterfaceWithGenericImpl<T> : ITestInterfaceWithGeneric<T> where T : TestInterfaceImpl
    {
        public void BaseMethod2()
        {
            throw new NotImplementedException();
        }
        public TestInterfaceWithGenericImpl(T t)
        {

        }
    }
}
