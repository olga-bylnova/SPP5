using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestClasses
{
    public class TestInterface1Impl : ITestInterface1
    {
        public IEnumerable<ITestInterface> it;
        public TestInterface1Impl(IEnumerable<ITestInterface> _it)
        {
            it = _it;
        }
        public IEnumerable<ITestInterface> getIt()
        {
            return it;
        }
    }
}
