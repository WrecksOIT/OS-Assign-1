using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assign_1
{
    public class Producer
    {
        private readonly SafeRing _ring;
        private readonly int _numToProduce;
        private readonly Random _rand ;
        private readonly ManualResetEvent _completeEvent = new ManualResetEvent(false);
        private Thread _thread; 

        public WaitHandle Complete => _completeEvent;

        public Producer(SafeRing ring, int numToProduce, Random random)
        {
            _numToProduce = numToProduce;
            _ring = ring;
            _rand = random;
        }

        public void Start()
        {
            _thread = new Thread(ThreadFunc);
            _thread.Start(this);
        }

        private static void ThreadFunc(object param)
        {
            var p = (param as Producer);
            p?.Produce();
        }

        private void Produce()
        {
            for (int i = 0; i < _numToProduce; i++)
            {
                var num = _rand.Next(1, 1001);
                _ring.Insert(num,10);
                Thread.Sleep(num);
            }

            _completeEvent.Set();
        }
    }
}
