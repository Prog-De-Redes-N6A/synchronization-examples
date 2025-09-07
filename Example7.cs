using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    public abstract class AbstractTask
    {
        private Thread? m_Thread;

        public void Execute()
        {
            m_Thread = new Thread(this.Process);
            m_Thread.Start();
        }

        public void WaitForFinish()
        {
            if (m_Thread != null)
                m_Thread.Join();
        }

        protected abstract void Process();
    }

    public class ConcreteTask : AbstractTask
    {
        protected override void Process()
        {
            Console.WriteLine("[Process] Starting concrete task!");
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Process] Running task iteration {i}");
            }
            Console.WriteLine("[Process] Finishing concrete task!");
        }
    }

    internal class Example7
    {
        public static void Example()
        {
            Console.WriteLine("[Example] Starting test!");

            ConcreteTask concreteTask = new ConcreteTask();

            concreteTask.Execute();

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Example] Running code in main {i}");
            }

            concreteTask.WaitForFinish();
            Console.WriteLine("[Example] End");
        }
    }
}
