
namespace AoC2022.util
{
    public static class PriorityQueueExtiensions
    {
        public static void EnqueueOrUpdate<TElement, TPriority>(this PriorityQueue<TElement, TPriority> priorityQueue, TElement element, TPriority priority) where TPriority: notnull where TElement : notnull
        {
            if(priorityQueue.UnorderedItems.Select(t => t.Element).Contains(element))
            {
                LinkedList<(TElement,TPriority)> stash =  new LinkedList<(TElement, TPriority)>();
                do
                {
                    priorityQueue.TryDequeue(out TElement ele, out TPriority prio);
                    stash.AddFirst((ele, prio));

                } while (!stash.First().Item1.Equals(element));
                priorityQueue.Enqueue(element,priority);

                stash.Skip(1).DoForEach(t => priorityQueue.Enqueue(t.Item1, t.Item2));
            }
            else
            {
                priorityQueue.Enqueue(element, priority);
            }
        }
    }
}
