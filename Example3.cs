using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    class LockDatabase
    {
        public void SaveData(string text, uint num)
        {
            lock (this)
            {
                Console.WriteLine($"[LockDatabase.SaveData] Running (thread {num})");
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(25);
                    Console.Write(text);
                }
                Console.WriteLine($"\n[LockDatabase.SaveData] Finished (thread {num})");
            }
        }
    }

    internal class Example3
    {
        public static LockDatabase db = new LockDatabase();

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
