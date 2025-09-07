using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronization
{
    public delegate void FinishTaskEvent(AbstractTaskEvent abstractTask);

    public abstract class AbstractTaskEvent
    {
        private Thread? m_Thread;
        public event FinishTaskEvent? OnFinishTask;

        public void Execute()
        {
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
            if (OnFinishTask != null)
                OnFinishTask(this);
        }

        protected abstract void Process();
    }

    public class ConcreteTaskEvent : AbstractTaskEvent
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

    internal class Example9
    {
        public static void Example()
        {
            Console.WriteLine("[Example] Starting test!");

            ConcreteTaskEvent concreteTask = new ConcreteTaskEvent();

            concreteTask.Execute();

            concreteTask.OnFinishTask += new FinishTaskEvent(PrintFinishedTask);
            concreteTask.OnFinishTask += new FinishTaskEvent(PrintFinishedTask);
            concreteTask.OnFinishTask += new FinishTaskEvent(PrintFinishedTask);
            concreteTask.OnFinishTask += new FinishTaskEvent(PrintFinishedTask);

            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine($"[Example] Running code in main {i}");
            }
        }

        static void PrintFinishedTask(AbstractTaskEvent task)
        {
            Console.WriteLine("[PrintFinishedTask] End");
        }
    }
}
