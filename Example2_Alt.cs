using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    class MonitorDatabase_Alt
    {
        public void SaveData(string text, uint num)
        {
            try
            {
                Monitor.Enter(this);
                Console.WriteLine($"[MonitorDatabase_Alt.SaveData] Running (thread {num})");

                // We return before leaving the Monitor
                //Thread.Abort() throws a special asynchronous exception(ThreadAbortException) into the target thread at any arbitrary point.
                Example2_Alt.threads[num].Abort();

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(25);
                    Console.Write(text);
                }
                Console.WriteLine($"\n[MonitorDatabase_Alt.SaveData] Finished (thread {num})");
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }

    internal class Example2_Alt
    {
        public static MonitorDatabase_Alt db = new MonitorDatabase_Alt();
        public static Thread[] threads = new Thread[2];

        static void WorkerThreadMethod(uint num)
        {
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} started");
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} calling Database.SaveData");
            string text = (num % 2 == 0) ? "o" : "x";
            try
            {
                db.SaveData(text, num);
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} finished");
        }

        public static void Example()
        {
            Console.WriteLine("[Example] Creating secondary worker threads");

            threads[0] = new Thread(() => WorkerThreadMethod(1));
            threads[1] = new Thread(() => WorkerThreadMethod(2));

            threads[0].Start();
            threads[1].Start();
        }
    }
}
