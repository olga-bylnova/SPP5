using System.Collections;

namespace Tests.TestClasses
{
    public interface ITestInterfaceWithGeneric<out T> where T : ITestInterface
    {
        void BaseMethod2();
    }
}
