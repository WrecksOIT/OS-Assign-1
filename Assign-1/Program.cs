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
            const int ringSize = 4;
            const int itemsToProduce = 1;
            const int numOfProducers = 5;
            const int numOfConsumers = 5;
            var rand = new Random();
            var ring = new SafeRing(ringSize);
            var producerList = new List<Producer>();
            var consumerList = new List<Consumer>();
            var completeEventList = new List<WaitHandle>();

            for (int i = 0; i < numOfProducers; i++)  
            {
                var p = new Producer(ring, itemsToProduce, rand);
                producerList.Add(p);
                p.Start();
                completeEventList.Add(p.Complete);
            }

            for (int i = 0; i < numOfConsumers; i++)
            {
                var c = new Consumer(ring, rand);
                consumerList.Add(c);
                c.Start();
            }

            WaitHandle.WaitAll(completeEventList.ToArray());
            consumerList.ForEach(c => c.Stop());
        }
    }
}
