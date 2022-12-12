using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestClasses
{
    public interface ITestInterface1
    {
        public IEnumerable<ITestInterface> getIt();
    }
}
