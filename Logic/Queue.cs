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
    public class Queue<T> : IEnumerable<T>, IEquatable<Queue<T>> where T : IEquatable<T> 
    {
        #region fields
        private T[] array;
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
            if (ReferenceEquals(collection, null)) throw new ArgumentNullException($"{nameof(collection)} is null.");

            array = new T[collection.Count() * 2];
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

            array[count] = element;
            count++;
        }

        /// <summary>
        /// Removes an element from queue.
        /// </summary>
        public void Dequeue()
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");
            
            for (int i = 0; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array[--count] = default(T);
        }

        /// <summary>
        /// Removes some elements from queue.
        /// </summary>
        /// <param name="quantity">Quantity of elements to remove.</param>
        public void Dequeue(int quantity)
        {
            if (quantity < 0 || quantity > array.Length) throw new ArgumentNullException($"{nameof(quantity)} is unsuitable.");
            
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
            if (IsEmpty) throw new ArgumentException("Queue is empty.");
            
            return array[0];
        }

        /// <summary>
        /// Checks the presence of the element in the queue.
        /// </summary>
        /// <param name="element">Element to check.</param>
        /// <returns>True, if element presents, and false otherwise.</returns>
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

        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            if (IsEmpty) throw new ArgumentException("Queue is empty.");

            count = 0;
            capacity = 10;
            array = new T[capacity];
        }

        /// <summary>
        /// Returns string representation of queue.
        /// </summary>
        /// <returns>String representation of queue.</returns>
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

        /// <summary>
        /// Compares two queues for equality.
        /// </summary>
        /// <param name="other">Second queue to compare.</param>
        /// <returns>True, if queues are equal, and false otherwise.</returns>
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

        /// <summary>
        /// Enumerator for queue.
        /// </summary>
        /// <returns>Object IEnumerator<T> to iterate.</returns>
        public IEnumerator<T> GetEnumerator() => new IteratorQueue(this);

        /// <summary>
        /// Enumerator for queue.
        /// </summary>
        /// <returns>Object IEnumerator to iterate.</returns>
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

            object IEnumerator.Current => Current;

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
