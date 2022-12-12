using DependencyInjector;
using System.Collections;
using Tests.TestClasses;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_ArgumentException_IfImplementationClassIsAbstract()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, AbstractClassTestInterfaceImpl>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_ArgumentException_IfDependencyIsNotAssignableFromImplementation()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, Class1>();
        }

        [TestMethod]
        public void Base_Interface_And_Implementation()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>();

            var di = new DependencyProvider(diConfig);
            var actual = di.Resolve<ITestInterface>();

            Assert.AreEqual(typeof(TestInterfaceImpl), actual.GetType());
        }

        [TestMethod]
        public void Should_Resolve_Inner_Dependencies()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>();

            var di = new DependencyProvider(diConfig);
            var actual = di.Resolve<ITestInterface>();

            Assert.AreEqual(typeof(TestInterfaceImpl), actual.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(StackOverflowException))]
        public void Should_Throw_StackOverflowException_IfRecursion()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, ImplWithRecursionDependency>();

            var di = new DependencyProvider(diConfig);
            di.Resolve<ITestInterface>();
        }

        [TestMethod]
        public void Should_Return_New_Instance_If_Not_Singleton()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>(isSingleton: false);

            var di = new DependencyProvider(diConfig);
            var actual1 = di.Resolve<ITestInterface>();
            var actual2 = di.Resolve<ITestInterface>();

            Assert.AreNotSame(actual1, actual2);
        }

        [TestMethod]
        public void Should_Return_Same_Instance_If_Singleton()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>(isSingleton: true);

            var di = new DependencyProvider(diConfig);
            var actual1 = di.Resolve<ITestInterface>();
            var actual2 = di.Resolve<ITestInterface>();

            Assert.AreSame(actual1, actual2);
        }

        [TestMethod]
        public void Should_Return_All_Registered_Implementations_For_One_Dependency()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>();
            diConfig.Register<ITestInterface, TestInterfaceImpl2>();

            var di = new DependencyProvider(diConfig);
            var actual = di.Resolve<IEnumerable<ITestInterface>>();

            Assert.AreEqual(typeof(TestInterfaceImpl), actual.First().GetType());
            Assert.AreEqual(typeof(TestInterfaceImpl2), actual.Last().GetType());
        }

        [TestMethod]
        public void Should_Resolve_IEnumerable_Parameter_In_Constructor()
        {
            var diConfig = new DependenciesConfiguration();
            diConfig.Register<ITestInterface, TestInterfaceImpl>();
            diConfig.Register<ITestInterface, TestInterfaceImpl2>();
            diConfig.Register<ITestInterface1, TestInterface1Impl>();

            var di = new DependencyProvider(diConfig);
            var actual = di.Resolve<ITestInterface1>();

            Assert.AreEqual(typeof(TestInterfaceImpl), actual.getIt().First().GetType());
            Assert.AreEqual(typeof(TestInterfaceImpl2), actual.getIt().Last().GetType());
        }

        [TestMethod]
        public void Should_Resolve_Open_Generic_Dependency()
        {
            var diConfig = new DependenciesConfiguration();

            diConfig.Register<ITestInterface, TestInterfaceImpl>();
            diConfig.Register(typeof(ITestInterfaceWithGeneric<>), typeof(TestInterfaceWithGenericImpl<>));
           // diConfig.Register<ITestInterfaceWithGeneric<ITestInterface>, TestInterfaceWithGenericImpl<ITestInterface>>();

            var di = new DependencyProvider(diConfig);
           // var actual = di.Resolve<ITestInterfaceWithGeneric<IEnumerable>>();
            var actual = di.Resolve<ITestInterfaceWithGeneric<ITestInterface>>();

           // Assert.AreEqual(typeof(TestInterfaceWithGenericImpl<ArrayList>), actual.GetType());
            Assert.AreEqual(typeof(TestInterfaceWithGenericImpl<TestInterfaceImpl>), actual.GetType());
        }

        /*[TestMethod]
        public void Should_Resolve_Open_Generic_Dependency_With_Constructor_Dependencies()
        {
            var diConfig = new DependenciesConfiguration();

            diConfig.Register<ITestInterface, TestInterfaceImpl>();
            diConfig.Register(typeof(ITestInterfaceWithGeneric<>), typeof(TestInterfaceWithGenericImpl<>));

            var di = new DependencyProvider(diConfig);
            var actual = di.Resolve<ITestInterfaceWithGeneric<IEnumerable>>();

            Assert.AreEqual(typeof(TestInterfaceWithGenericImpl<ArrayList>), actual.GetType());
        }*/
    }
}