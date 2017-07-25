using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class Queue<T> : IEnumerable<T>, IEquatable<Queue<T>> where T : IEquatable<T> 
    {
        #region fields
        private T[] array;
        private int count;
        private int capacity = 10;

        private bool isFull => count == capacity;
        #endregion

        #region properties
        public int Count => count;
        public bool IsEmpty => count == 0;
        #endregion

        #region Ctors
        public Queue()
        {
            array = new T[capacity];
        }

        public Queue(int capacity)
        {
            if (capacity <= 0) throw new ArgumentException($"{nameof(capacity)} is unsuitable.");

            this.capacity = capacity;
            array = new T[capacity];
        }

        public Queue(IEnumerable<T> collection)
        {
            if (ReferenceEquals(collection, null)) throw new ArgumentNullException($"{nameof(collection)} is null.");

            array = new T[collection.Count() * 2];
            foreach (T element in collection)
            {
                Enqueue(element);
            }
        }
        #endregion

        #region public methods
        public void Enqueue(T element)
        {
            if (ReferenceEquals(element, null)) throw new ArgumentNullException($"{nameof(element)} is null.");
            
            if (isFull) Expansion();

            array[count] = element;
            count++;
        }

        public void Dequeue()
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");
            
            for (int i = 0; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array[--count] = default(T);
        }

        public void Dequeue(int quantity)
        {
            if (quantity < 0 || quantity > array.Length) throw new ArgumentNullException($"{nameof(quantity)} is unsuitable.");
            
            while(quantity > 0)
            {
                Dequeue();
                quantity--;
            }
        }

        public T Peek()
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");
            
            return array[0];
        }

        public bool Contains(T element)
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");
            if (ReferenceEquals(element, null)) throw new ArgumentNullException($"{nameof(element)} is null.");
                
            for (int i = 0; i < count; i++)
            {
                if (element.Equals(array[i]))
                    return true;
            }
            return false;
        }

        public void Clear()
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");

            count = 0;
            capacity = 10;
            array = new T[capacity];
        }

        public override string ToString()
        {
            if (IsEmpty) return "Queue is empty.";
            
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                str.Append(array[i].ToString() + " ");
            }
            return str.ToString();
        }

        public bool Equals(Queue<T> other)
        {
            if (ReferenceEquals(other, null)) throw new ArgumentNullException($"{nameof(other)} is null.");

            if (other.Count != this.Count) return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this.array[i].Equals(other.array[i]))
                    return false;
            }
            return true;
        }

        public IEnumerator<T> GetEnumerator() => new IteratorQueue(this);
        IEnumerator IEnumerable.GetEnumerator() => new IteratorQueue(this);

        #region struct IteratorQueue
        private struct IteratorQueue : IEnumerator<T>
        {
            private Queue<T> queue;
            private int index;

            public T Current
            {
                get
                {
                    if (index == -1 || index == queue.Count) throw new ArgumentException($"{nameof(index)} is unsuitable.");
                    return queue.array[index];
                }
            }

            object IEnumerator.Current => queue.array[index];

            public IteratorQueue(Queue<T> queue)
            {
                this.queue = queue;
                this.index = -1;
            }

            public bool MoveNext() => ++index < queue.Count;

            public void Reset() => index = -1;

            public void Dispose() { }
        }
        #endregion

        #endregion

        #region private methods
        private void Expansion()
        {
            capacity *= 2;
            Array.Resize(ref array, capacity);
        }
        #endregion
    }
}
