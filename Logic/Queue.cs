using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    /// <summary>
    /// Generic collection queue.
    /// </summary>
    /// <typeparam name="T">Type for substitution.</typeparam>
    public class Queue<T> : IEnumerable<T> 
    {
        #region fields
        private T[] array;
        private int head;
        private int tail;
        private int count;
        private int capacity = 10;
        private bool isFull => count == capacity;
        #endregion

        #region properties
        /// <summary>
        /// Quantity of elements.
        /// </summary>
        public int Count => count;

        /// <summary>
        /// Returns true, if queue is empty, and false otherwise.
        /// </summary>
        public bool IsEmpty => count == 0;
        #endregion

        #region Ctors
        /// <summary>
        /// Ctor without parameters.
        /// </summary>
        public Queue()
        {
            array = new T[capacity];
        }

        /// <summary>
        /// Ctor with parameter.
        /// </summary>
        /// <param name="capacity">Initial size of queue.</param>
        public Queue(int capacity)
        {
            if (capacity <= 0) throw new ArgumentException($"{nameof(capacity)} is unsuitable.");

            this.capacity = capacity;
            array = new T[capacity];
        }

        /// <summary>
        /// Ctor with parameter.
        /// </summary>
        /// <param name="collection">Collection to copy elements into queue.</param>
        public Queue(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException($"{nameof(collection)} is null.");

            array = new T[collection.Count()];
            foreach (T element in collection)
            {
                Enqueue(element);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Add an element into queue.
        /// </summary>
        /// <param name="element">Element ro insert.</param>
        public void Enqueue(T element)
        {
            if (ReferenceEquals(element, null)) throw new ArgumentNullException($"{nameof(element)} is null.");
            
            if (isFull) Expansion();

            array[tail] = element;
            tail = (tail + 1) % capacity;
            count++;
        }

        /// <summary>
        ///  Removes an element from queue.
        /// </summary>
        /// <returns>Removed element.</returns>
        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");

            T removed = array[head];
            array[head] = default(T);
            head = (head + 1) % capacity;
            count--;

            return removed;
        }

        /// <summary>
        /// Removes some elements from queue.
        /// </summary>
        /// <param name="quantity">Quantity of elements to remove.</param>
        public void Dequeue(int quantity)
        {
            if (quantity < 0 || quantity > array.Length) throw new ArgumentException($"{nameof(quantity)} is unsuitable.");
            
            while(quantity > 0)
            {
                Dequeue();
                quantity--;
            }
        }

        /// <summary>
        /// Returns first element in the queue.
        /// </summary>
        /// <returns>First element in the queue.</returns>
        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");
            
            return array[head];
        }

        /// <summary>
        /// Checks the presence of the element in the queue.
        /// </summary>
        /// <param name="element">Element to check.</param>
        /// <returns>True, if element presents, and false otherwise.</returns>
        public bool Contains(T element)
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");
            if (ReferenceEquals(element, null)) throw new ArgumentNullException($"{nameof(element)} is null.");
                
            for (int i = 0; i < count; i++)
            {
                if (element.Equals(array[i]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue is empty.");

            count = 0;
            head = 0;
            tail = 0;
            array = new T[capacity];
        }

        /// <summary>
        /// Enumerator for queue.
        /// </summary>
        /// <returns>Object IEnumerator to iterate.</returns>
        public IteratorQueue GetEnumerator() => new IteratorQueue(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #region struct IteratorQueue
        public struct IteratorQueue : IEnumerator<T>
        {
            private Queue<T> queue;
            private int index;

            public T Current
            {
                get
                {
                    if (index == -1 || index == queue.capacity) throw new ArgumentException($"{nameof(index)} is unsuitable.");
                    return queue.array[index];
                }
            }

            object IEnumerator.Current => Current;

            public IteratorQueue(Queue<T> queue)
            {
                this.queue = queue;
                this.index = queue.head - 1;
            }

            public bool MoveNext()
            {
                if (queue.head <= queue.tail)
                {
                    return ++index < queue.Count + queue.head;
                }
                else
                {
                    index = (index + 1) % queue.capacity;
                    return index != queue.tail;
                }
            }

            public void Reset() => index = -1;

            public void Dispose() { }
        }
        #endregion

        #endregion

        #region private methods
        private void Expansion()
        {
            capacity *= 2;
            T[] newArray = new T[capacity];

            if (count > 0)
            {
                if (head < tail)
                {
                    Array.Copy(array, head, newArray, 0, count);
                }
                else
                {
                    Array.Copy(array, head, newArray, 0, array.Length - head);
                    Array.Copy(array, 0, newArray, array.Length - head, tail);
                }
            }

            array = newArray;
            head = 0;
            tail = count;
        }
        #endregion
    }
}
