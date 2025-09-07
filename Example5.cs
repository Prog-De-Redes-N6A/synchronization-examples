using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    class MutexDatabase
    {
        static Mutex mutex = new Mutex(false);

        public void SaveData(string text, uint num)
        {
            mutex.WaitOne();

            Console.WriteLine($"[MutexDatabase.SaveData] Running (thread {num})");
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(25);
                Console.Write(text);
            }
            Console.WriteLine($"\n[MutexDatabase.SaveData] Finished (thread {num})");

            mutex.ReleaseMutex();
        }
    }

    internal class Example5
    {
        public static MutexDatabase db = new MutexDatabase();

        static void WorkerThreadMethod(uint num)
        {
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} started");
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} calling Database.SaveData");
            string text = (num % 2 == 0) ? "o" : "x";
            db.SaveData(text, num);
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} finished");
        }

        public static void Example()
        {
            Console.WriteLine("[Example] Creating secondary worker threads");

            Thread t1 = new Thread(() => WorkerThreadMethod(1));
            Thread t2 = new Thread(() => WorkerThreadMethod(2));

            t1.Start();
            t2.Start();
        }
    }
}
