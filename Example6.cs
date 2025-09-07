using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    internal class Example6
    {
        static Thread[] threads = new Thread[10];
        static Semaphore sem = new Semaphore(3, 3); // parameters: (initialCount,maximumCount)

        static void Func()
        {
            Console.WriteLine("[Func] {0} is waiting in line", Thread.CurrentThread.Name);
            sem.WaitOne();
            Console.WriteLine("[Func] {0} enters the zone!", Thread.CurrentThread.Name);
            Thread.Sleep(300);
            Console.WriteLine("[Func] {0} is leaving the zone", Thread.CurrentThread.Name);
            sem.Release();
        }

        public static void Example()
        {
            for (int i = 0; i < 10; i++)
            {
                // We create 10 threads that execute the same function
                threads[i] = new Thread(Func);
                threads[i].Name = "thread_" + i;
                threads[i].Start();
            }
            Console.ReadLine();
        }
    }
}
