using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Example adapted from https://www.c-sharpcorner.com/UploadFile/1d42da/wait-and-pulse-method-in-threading-C-Sharp/

namespace Synchronization
{
    internal class PingPong
    {
        public void Ping(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    // Ball halts
                    Monitor.Pulse(this); // Notify any waiting threads
                    return;
                }
                Console.WriteLine("Ping ");
                Monitor.Pulse(this); // Let Pong run
                Monitor.Wait(this); // Wait for Pong to complete
            }
        }

        public void Pong(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    // Ball halts
                    Monitor.Pulse(this); // Notify any waiting threads
                    return;
                }
                Console.WriteLine("Pong ");
                Monitor.Pulse(this); // Let Ping run
                Monitor.Wait(this); // Wait for Ping to complete
            }
        }
    }

    internal class MyThread
    {
        public Thread thread;
        PingPong pingPongObject;

        // Construct a new thread
        public MyThread(string name, PingPong pp)
        {
            thread = new Thread(this.Run);
            pingPongObject = pp;
            thread.Name = name;
            thread.Start();
        }

        // Begin execution of new thread
        void Run()
        {
            if (thread.Name == "Ping")
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    pingPongObject.Ping(true);
                }
                pingPongObject.Ping(false);
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(100);
                    pingPongObject.Pong(true);
                }
                pingPongObject.Pong(false);
            }
        }
    }
 
    internal class Example4
    {
        public static void Example()
        {
            Console.WriteLine("[Example] The ball is dropped...");

            PingPong pp = new PingPong();

            MyThread t1 = new MyThread("Ping", pp);
            MyThread t2 = new MyThread("Pong", pp);

            t1.thread.Join();
            t2.thread.Join();

            Console.WriteLine("[Example] The ball stops bouncing");
            Console.Read();
        }
    }
}
