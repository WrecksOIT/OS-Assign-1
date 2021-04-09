using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assign_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ring = new SafeRing(5);
            ring.Insert(1);
            ring.Insert(2);
            ring.Insert(3);
            ring.Remove();
        }
    }
}
