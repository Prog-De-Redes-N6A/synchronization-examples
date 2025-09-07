using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    public delegate void FinishTask(AbstractTaskCallback abstractTask);
    
    public abstract class AbstractTaskCallback
    {
        private Thread? m_Thread;
        private FinishTask? m_Callback;

        public void Execute(FinishTask callback)
        {
            m_Callback = callback;
            m_Thread = new Thread(this.FinishTask);
            m_Thread.Start();
        }

        public void WaitForFinish()
        {
            if (m_Thread != null)
                m_Thread.Join();
        }

        private void FinishTask()
        {
            this.Process();
            if (m_Callback != null)
                m_Callback(this);
        }

        protected abstract void Process();
    }

    public class ConcreteTaskCallback : AbstractTaskCallback
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

    internal class Example8
    {
        public static void Example()
        {
            Console.WriteLine("[Example] Starting test!");

            ConcreteTaskCallback concreteTask = new ConcreteTaskCallback();

            concreteTask.Execute(PrintFinishedTask);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Example] Running code in main {i}");
            }
        }

        static void PrintFinishedTask(AbstractTaskCallback task)
        {
            Console.WriteLine("[PrintFinishedTask] End");
        }
    }
}
