using System.Diagnostics.CodeAnalysis;

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
            SimplePriorityQueue<TNode> fScoreQueue = new SimplePriorityQueue<TNode>(isMinPriorityQueue: true);
            fScoreQueue.Enqueue(heuristic(startNode), startNode);

            Dictionary<TNode, int> gScore = new Dictionary<TNode, int>
            {
                { startNode, 0 }
            };

            Dictionary<TNode, TNode> cameFrom = new Dictionary<TNode, TNode>();


            while (fScoreQueue.TryDequeue(out int currentFscore, out TNode? current))
            {
                if(isDestination(current))
                    return (currentFscore,
                            EnumerableGeneration.InfiniteByValueTransformation(current, c => cameFrom.GetOrThrow(c)).TakeWhile(node => !node.Equals(startNode)).Reverse());

                foreach (var neigbour in neigbours(current))
                {
                    int tentativeGscore = gScore.GetOrThrow(current) + costFromTo(current, neigbour);
                    if ((!gScore.ContainsKey(neigbour)) || tentativeGscore < gScore.GetOrThrow(neigbour))
                    {
                        cameFrom[neigbour] = current;
                        gScore[neigbour] = tentativeGscore;
                        fScoreQueue.UpdatePriority(neigbour, tentativeGscore + heuristic(neigbour));
                    }
                }
            }

            return (-1, Enumerable.Empty<TNode>());
        }
    }

    public class BreathFirstSearch
    {
        public static bool TryFind<TNode>(
            TNode start,
            Func<TNode, bool> target,
            Func<TNode, IEnumerable<TNode>> neigbours,
            [MaybeNullWhen(false)] out TNode result)
        {
            result = start;
            if (target(start)) return true;
            // Find
            HashSet<TNode> visited = new HashSet<TNode>();
            Queue<TNode> frontier = new Queue<TNode>();
            visited.Add(start);
            frontier.Enqueue(start);


            while (frontier.TryDequeue(out TNode? current))
            {
                foreach (var n in neigbours(current))
                {
                    if (target(n))
                    {
                        result= n;
                        return true;
                    }
                    if (visited.Contains(n)) continue;
                    visited.Add(n);
                    frontier.Enqueue(n);
                }
            }
            return false;
        }


        public static IEnumerable<TNode> Reachable<TNode>(
            TNode start,
            Func<TNode, IEnumerable<TNode>> neigbours) where TNode : notnull
        {
            HashSet<TNode> visited = new HashSet<TNode>();
            Queue<TNode> frontier = new Queue<TNode>();
            visited.Add(start);
            frontier.Enqueue(start);

            while(frontier.TryDequeue(out TNode? result))
            {
                foreach (var n in neigbours(result))
                {
                    if (visited.Contains(n)) continue;
                    visited.Add(n);
                    frontier.Enqueue(n);
                }
            }

            return visited;
        }
    }

    public class DeapthFirstSearch
    {
        public static bool TryFind<TNode>(
            TNode start,
            Func<TNode, bool> target,
            Func<TNode, IEnumerable<TNode>> neigbours,
            [MaybeNullWhen(false)] out TNode result)
        {
            result = start;
            if (target(start)) return true;
            // Find
            HashSet<TNode> visited = new HashSet<TNode>();
            Stack<TNode> frontier = new Stack<TNode>();
            visited.Add(start);
            frontier.Push(start);


            while (frontier.TryPop(out TNode? current))
            {
                foreach (var n in neigbours(current))
                {
                    if (target(n))
                    {
                        result = n;
                        return true;
                    }
                    if (visited.Contains(n)) continue;
                    visited.Add(n);
                    frontier.Push(n);
                }
            }
            return false;
        }

        public static IEnumerable<TNode> Reachable<TNode>(
            TNode start,
            Func<TNode, IEnumerable<TNode>> neigbours) where TNode : notnull
        {
            HashSet<TNode> visited = new HashSet<TNode>();
            Stack<TNode> frontier = new Stack<TNode>();
            visited.Add(start);
            frontier.Push(start);

            while (frontier.TryPop(out TNode? result))
            {
                foreach (var n in neigbours(result))
                {
                    if (visited.Contains(n)) continue;
                    visited.Add(n);
                    frontier.Push(n);
                }
            }

            return visited;
        }
    }
}
