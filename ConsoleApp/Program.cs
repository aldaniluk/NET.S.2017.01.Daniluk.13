using System;
using Logic;
using System.Collections;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> iqueue = new Queue<int>();
            iqueue.Enqueue(1);
            iqueue.Enqueue(2);
            iqueue.Enqueue(3);
            iqueue.Enqueue(4);
            iqueue.Enqueue(5);
            
            foreach (var i in iqueue)
            {
                Console.WriteLine(i);
            }

            //Console.WriteLine(iqueue);

            //iqueue.Dequeue(2);
            //Console.WriteLine(iqueue);

            ////iqueue.Dequeue(-2); //exception
            ////iqueue.Dequeue(100); //exception

            //Console.WriteLine(iqueue);
            //Console.WriteLine(iqueue.Peek()); //3
            //Console.WriteLine(iqueue.Contains(3)); //true
            //Console.WriteLine(iqueue.Contains(-3)); //false

            //iqueue.Clear();
            //Console.WriteLine(iqueue);

            //iqueue.Enqueue(1);
            //iqueue.Enqueue(2);
            //iqueue.Enqueue(3);

            //Queue<int> iqueue2 = new Queue<int>();

            //iqueue2.Enqueue(1);
            //iqueue2.Enqueue(2);
            //iqueue2.Enqueue(3);
            //Console.WriteLine(iqueue.Equals(iqueue2)); //true
        }
    }
}
