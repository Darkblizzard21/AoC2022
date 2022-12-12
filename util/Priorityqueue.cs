using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC2022.util
{
    /// <summary>
    /// Source https://www.dotnetlovers.com/article/231/priority-queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimplePriorityQueue<T> 
    {
        class Node
        {
            public int Priority { get; set; }
            public T Object { get; set; }
        }

        //object array
        List<Node> queue = new List<Node>();
        int heapSize = -1;
        bool _isMinPriorityQueue;
        public int Count { get { return queue.Count; } }

        /// <summary>
        /// If min queue or max queue
        /// </summary>
        /// <param name="isMinPriorityQueue"></param>
        public SimplePriorityQueue(bool isMinPriorityQueue = false)
        {
            _isMinPriorityQueue = isMinPriorityQueue;
        }

        /// <summary>
        /// Enqueue the object with priority
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="obj"></param>
        public void Enqueue(int priority, T obj)
        {
            Node node = new Node() { Priority = priority, Object = obj };
            queue.Add(node);
            heapSize++;
            //Maintaining heap
            if (_isMinPriorityQueue)
                BuildHeapMin(heapSize);
            else
                BuildHeapMax(heapSize);
        }
        /// <summary>
        /// Dequeue the object
        /// </summary>
        /// <returns></returns>
        public T Dequeue(out int priority)
        {
            if (heapSize > -1)
            {
                priority = queue[0].Priority;
                var returnVal = queue[0].Object;
                queue[0] = queue[heapSize];
                queue.RemoveAt(heapSize);
                heapSize--;
                //Maintaining lowest or highest at root based on min or max queue
                if (_isMinPriorityQueue)
                    MinHeapify(0);
                else
                    MaxHeapify(0);
                return returnVal;
            }
            else
                throw new Exception("Queue is empty");
        }

        public bool TryDequeue([MaybeNullWhen(false)] out int priority, [MaybeNullWhen(false)] out T obj)
        {
            if (heapSize < 0)
            {
                priority = 0;
                obj = default;
                return false;
            }

            obj = Dequeue(out priority);
            return true;
        }

        /// <summary>
        /// Updating the priority of specific object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="priority"></param>
        public void UpdatePriority(T obj, int priority)
        {
            int i = 0;
            for (; i <= heapSize; i++)
            {
                Node node = queue[i];
                if (object.ReferenceEquals(node.Object, obj))
                {
                    node.Priority = priority;
                    if (_isMinPriorityQueue)
                    {
                        BuildHeapMin(i);
                        MinHeapify(i);
                    }
                    else
                    {
                        BuildHeapMax(i);
                        MaxHeapify(i);
                    }
                    return;
                }
            }

            Enqueue(priority, obj);

        }
        /// <summary>
        /// Searching an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsInQueue(T obj)
        {
            foreach (Node node in queue)
                if (object.ReferenceEquals(node.Object, obj))
                    return true;
            return false;
        }

        /// <summary>
        /// Maintain max heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMax(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority < queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }
        /// <summary>
        /// Maintain min heap
        /// </summary>
        /// <param name="i"></param>
        private void BuildHeapMin(int i)
        {
            while (i >= 0 && queue[(i - 1) / 2].Priority > queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void MaxHeapify(int i)
        {
            int left = ChildL(i);
            int right = ChildR(i);

            int heighst = i;

            if (left <= heapSize && queue[heighst].Priority < queue[left].Priority)
                heighst = left;
            if (right <= heapSize && queue[heighst].Priority < queue[right].Priority)
                heighst = right;

            if (heighst != i)
            {
                Swap(heighst, i);
                MaxHeapify(heighst);
            }
        }
        private void MinHeapify(int i)
        {
            int left = ChildL(i);
            int right = ChildR(i);

            int lowest = i;

            if (left <= heapSize && queue[lowest].Priority > queue[left].Priority)
                lowest = left;
            if (right <= heapSize && queue[lowest].Priority > queue[right].Priority)
                lowest = right;

            if (lowest != i)
            {
                Swap(lowest, i);
                MinHeapify(lowest);
            }
        }
        private void Swap(int i, int j)
        {
            var temp = queue[i];
            queue[i] = queue[j];
            queue[j] = temp;
        }
        private int ChildL(int i)
        {
            return i * 2 + 1;
        }
        private int ChildR(int i)
        {
            return i * 2 + 2;
        }
    }
}
