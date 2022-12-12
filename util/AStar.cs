namespace AoC2022.util
{

    public class AStar
    {
        public static (int cost, IEnumerable<TNode> trace) FindPath<TNode>(
            TNode startNode,
            Func<TNode, IEnumerable<TNode>> neigbours,
            Func<TNode, bool> isDestination,
            Func<TNode, int> heuristic,
            Func<TNode, TNode, int> costFromTo) where TNode : notnull
        {
            PriorityQueue<TNode, int> fScoreQueue = new PriorityQueue<TNode, int>();
            fScoreQueue.Enqueue(startNode, heuristic(startNode));

            Dictionary<TNode, int> gScore = new Dictionary<TNode, int>
            {
                { startNode, 0 }
            };

            Dictionary<TNode, TNode> cameFrom = new Dictionary<TNode, TNode>();


            while (fScoreQueue.TryDequeue(out TNode? current, out int currentFscore))
            {
                if(isDestination(current))
                    return (currentFscore,
                            IEnumerableExtentions.InfiniteByValueTransformation(current, c => cameFrom.GetOrThrow(c)).TakeWhile(node => !node.Equals(startNode)).Reverse());

                foreach (var neigbour in neigbours(current))
                {
                    int tentativeGscore = gScore.GetOrThrow(current) + costFromTo(current, neigbour);
                    if ((!gScore.ContainsKey(neigbour)) || tentativeGscore < gScore.GetOrThrow(neigbour))
                    {
                        cameFrom[neigbour] = current;
                        gScore[neigbour] = tentativeGscore;
                        fScoreQueue.EnqueueOrUpdate(neigbour, tentativeGscore + heuristic(neigbour));
                    }
                }
            }

            return (-1, Enumerable.Empty<TNode>());
        }
    }
}
