using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assign_1
{
    public class Consumer
    {
        private readonly SafeRing _ring;
        private readonly ManualResetEvent _completeEvent = new ManualResetEvent(false);
        private Thread _thread;
        private readonly Random _rand;
        private bool _shouldStop = false;

        public Consumer(SafeRing ring, Random random)
        {
            _ring = ring;
            _rand = random;
        }

        public void Stop()
        {
            _shouldStop = true;
            _thread.Interrupt();
        } 

        private static void ThreadFunc(object param)
        {
            var c = (param as Consumer);
            c.Consume();
        }

        public void Start()
        {
            _thread = new Thread(ThreadFunc);
            _thread.Start(this);
        }

        private void Consume()
        {
            while (!_shouldStop)
            {
                try
                {
                    _ring.Remove(10);
                    Thread.Sleep(_rand.Next(1, 1001));
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {e.Message}");
                }
            }
        }
    }
}
