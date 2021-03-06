using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assign_1
{
    public class SafeRing
    {

        private int _size;
        private int _head;
        private int _tail;
        private readonly int[] _buffer;
        private readonly int _capacity;
        private readonly Mutex _mutex = new Mutex();
        private readonly ManualResetEvent _hasItems = new ManualResetEvent(false);
        private readonly ManualResetEvent _hasCapacity = new ManualResetEvent(true);

        public SafeRing(int size)
        {
            _capacity = size;
            _buffer = new int[size];
        }

        public int Remove(int timeout)
        {
            WaitHandle.WaitAll(new WaitHandle[] {_mutex, _hasItems});

            var i = _buffer[_head];
            _head = (_head + 1) % _capacity;
            _size--;
            _hasCapacity.Set();
            
            if (_size == 0) 
                _hasItems.Reset();

            _mutex.ReleaseMutex();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: removed {i}");
            return i;
        }

        public void Insert(int numberToInsert, int timeout)
        {
            WaitHandle.WaitAll(new WaitHandle[] {_mutex, _hasCapacity});
            _buffer[_tail] = numberToInsert;
            _tail = (_tail + 1) % _capacity;
            _size++;
            _hasItems.Set();
            
            if (_size == _capacity) 
                _hasCapacity.Reset();

            _mutex.ReleaseMutex();
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: inserted {numberToInsert}");
        }

        public int Count()
        {
            _mutex.WaitOne();
            var result = _size;
            _mutex.ReleaseMutex();
            return result;
        }
    }
}