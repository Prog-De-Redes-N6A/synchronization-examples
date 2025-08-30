using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    class MonitorDatabase
    {
        public void SaveData(string text, uint num)
        {
            Monitor.Enter(this);
            Console.WriteLine($"[MonitorDatabase.SaveData] Running (thread {num})");
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(25);
                Console.Write(text);
            }
            Console.WriteLine($"\n[MonitorDatabase.SaveData] Finished (thread {num})");
            Monitor.Exit(this);
        }

        public void SaveDataException(string text, uint num)
        {
            Monitor.Enter(this);
            Console.WriteLine($"[MonitorDatabase.SaveData] Running (thread {num})");

            // We return before leaving the Monitor
            throw new Exception("ERROR!");
            
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(25);
                Console.Write(text);
            }
            Console.WriteLine($"\n[MonitorDatabase.SaveData] Finished (thread {num})");
            Monitor.Exit(this);
        }

        public void SaveDataExceptionSolved(string text, uint num)
        {
            try
            {
                Monitor.Enter(this);
                Console.WriteLine($"[MonitorDatabase.SaveData] Running (thread {num})");

                // We return before leaving the Monitor
                throw new Exception("ERROR!");

                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(25);
                    Console.Write(text);
                }
                Console.WriteLine($"\n[MonitorDatabase.SaveData] Finished (thread {num})");
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }

    internal class Example2
    {
        public static MonitorDatabase db = new MonitorDatabase();

        static void WorkerThreadMethod(uint num)
        {
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} started");
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} calling Database.SaveData");
            string text = (num % 2 == 0) ? "o" : "x";
            db.SaveData(text, num);
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} finished");
        }

        static void WorkerThreadMethodException(uint num)
        {
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} started");
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} calling Database.SaveData");
            string text = (num % 2 == 0) ? "o" : "x";
            try
            {
                db.SaveDataException(text, num);
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} finished");
        }

        static void WorkerThreadMethodExceptionSolved(uint num)
        {
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} started");
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} calling Database.SaveData");
            string text = (num % 2 == 0) ? "o" : "x";
            try
            {
                db.SaveDataExceptionSolved(text, num);
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"[WorkerThreadMethod] Secondary worker thread {num} finished");
        }

        public static void Example()
        {
            Console.WriteLine("[Example] Creating secondary worker threads");

            Thread t1 = new Thread(() => WorkerThreadMethod(1)); // Change to WorkerThreadMethodException or WorkerThreadMethodExceptionSolved to see different behavior
            Thread t2 = new Thread(() => WorkerThreadMethod(2)); // Change to WorkerThreadMethodException or WorkerThreadMethodExceptionSolved to see different behavior

            t1.Start();
            t2.Start();
        }
    }
}
