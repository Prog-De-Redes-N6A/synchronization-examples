using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    internal class Example10
    {
        public static void Example()
        {
            Console.WriteLine("[Example] Starting test!");

            BackgroundWorker bw = new BackgroundWorker();
            //Fixes not having to do ReadLine AutoResetEvent done = new AutoResetEvent(false);

            bw.DoWork += WorkerFunction;

            bw.RunWorkerCompleted += FinishFunction;
            //Fixes not having to do ReadLine bw.RunWorkerCompleted += (s, e) => done.Set();

            bw.RunWorkerAsync();

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Example] Running code in main {i}");
            }

            //Fixes not having to do ReadLine done.WaitOne();

            Console.ReadLine();
        }

        static void WorkerFunction(object? sender, DoWorkEventArgs e)
        {
            Console.WriteLine("[WorkerFunction] Started working!");
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Process] Running iteration {i}");
            }
        }

        static void FinishFunction(object? sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("[FinishFunction] Finished working!");
        }
    }
}
