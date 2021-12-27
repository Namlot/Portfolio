using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
namespace SearchAlgo
{
    class Program
    {
        static Stopwatch globalWatch = new Stopwatch();
        public class Node
        {
            public Queue<int[,]> stateHistory = new Queue<int[,]>();
            public Queue<Direction> moveHistory = new Queue<Direction>();
            public int[,] currentState;
            public int depth = 0;

            public Node(int[,] initialState, Queue<int[,]> stateHistory , Queue<Direction> directionHistory, int depth = 0)
            {
                this.currentState = initialState;
                this.moveHistory = directionHistory;
                this.stateHistory = stateHistory;
                this.depth = depth;
            }

            
        }

        public class SearchDetails
        {
            public int[,] initialConfig;
            public int nodesCreated = 0;
            public int nodesExpanded = 0;
            public string time;

            public Node winningNode;
        }

        static Dictionary<long, bool> pastStates = new Dictionary<long, bool>();

        static long GetHash(int[,] puzzle)
        {
            int hash = 0;
            int multiply = 1;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    hash += puzzle[i, j] * multiply;
                    multiply *= 10;
                }
            return hash;
        }

        static long GetHashWithDepth(int[,] puzzle, int depth)
        {
            int hash = 0;
            int multiply = 1;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    hash += puzzle[i, j] * multiply;
                    multiply *= 10;
                }
            return hash + depth;
        }

        static SearchDetails BreadthFirstSearch(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            Queue<Node> nodeQ = new Queue<Node>();
            nodeQ.Enqueue(initialNode);
            newSearchDetails.nodesCreated++;
           

            while (true)
            {
                Node current = nodeQ.Dequeue();
                newSearchDetails.nodesExpanded++;
                List < Direction > possibleDirections = PossibleDirections(current.currentState);
                foreach(Direction d in possibleDirections)
                {
                    int[,] tempCurrentState = DeepCopy(current.currentState);
                    Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                    if (!pastStates.ContainsKey(GetHash(newNode.currentState)))
                    {
                       
                        newSearchDetails.nodesCreated++;
                        newNode = SetUpNewNode(newNode, d);
                        pastStates[GetHash(newNode.currentState)] = true;
                        nodeQ.Enqueue(newNode);
                    if (IsFinished(newNode.currentState))
                    {
                        newSearchDetails.winningNode = newNode;
                        return newSearchDetails;
                    }
                    }
                }
            }
        }

        static SearchDetails GreedySearch(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
          //  Queue<Node> nodeQ = new Queue<Node>();
          //  nodeQ.Enqueue(initialNode);
            newSearchDetails.nodesCreated++;
            int currentGreedyMin = ManhattanDistance(initialNode.currentState);
            Dictionary<int, Queue<Node>> greedyDictQ = new Dictionary<int, Queue<Node>>();
            greedyDictQ[currentGreedyMin] = new Queue<Node>();
            greedyDictQ[currentGreedyMin].Enqueue(initialNode);
            

            while (true)
            {
                if (!greedyDictQ.ContainsKey(currentGreedyMin) || greedyDictQ[currentGreedyMin].Count == 0)
                {
                    currentGreedyMin++;
                }
                else
                {

                    Node current = greedyDictQ[currentGreedyMin].Dequeue();
                    newSearchDetails.nodesExpanded++;
                    List<Direction> possibleDirections = PossibleDirections(current.currentState);
                    foreach (Direction d in possibleDirections)
                    {
                        int[,] tempCurrentState = DeepCopy(current.currentState);
                        Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                        if (!pastStates.ContainsKey(GetHash(newNode.currentState)))
                        {

                            newSearchDetails.nodesCreated++;
                            newNode = SetUpNewNode(newNode, d);
                            pastStates[GetHash(newNode.currentState)] = true;

                            int heuristic = ManhattanDistance(newNode.currentState);
                            if (heuristic < currentGreedyMin)
                                currentGreedyMin = heuristic;

                            if (!greedyDictQ.ContainsKey(heuristic))
                                greedyDictQ[heuristic] = new Queue<Node>();

                            greedyDictQ[heuristic].Enqueue(newNode);

                            if (IsFinished(newNode.currentState))
                            {
                                newSearchDetails.winningNode = newNode;
                                return newSearchDetails;
                            }
                        }
                    }
                }
            }
        }

        static SearchDetails AStarBook(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            //  Queue<Node> nodeQ = new Queue<Node>();
            //  nodeQ.Enqueue(initialNode);
            newSearchDetails.nodesCreated++;
            int AstarMin = ManhattanDistance(initialNode.currentState);
            Dictionary<int, Queue<Node>> AstarDictQ = new Dictionary<int, Queue<Node>>();
            AstarDictQ[AstarMin] = new Queue<Node>();
            AstarDictQ[AstarMin].Enqueue(initialNode);


            while (true)
            {
                if (!AstarDictQ.ContainsKey(AstarMin) || AstarDictQ[AstarMin].Count == 0)
                {
                    AstarMin++;
                }
                else
                {

                    Node current = AstarDictQ[AstarMin].Dequeue();
                    newSearchDetails.nodesExpanded++;
                    List<Direction> possibleDirections = PossibleDirections(current.currentState);
                    foreach (Direction d in possibleDirections)
                    {
                        int[,] tempCurrentState = DeepCopy(current.currentState);
                        Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                        if (!pastStates.ContainsKey(GetHash(newNode.currentState)))
                        {

                            newSearchDetails.nodesCreated++;
                            newNode = SetUpNewNode(newNode, d);
                            pastStates[GetHash(newNode.currentState)] = true;

                            int heuristic = ManhattanDistance(newNode.currentState) + newNode.depth;
                            if (heuristic < AstarMin)
                                AstarMin = heuristic;

                            if (!AstarDictQ.ContainsKey(heuristic))
                                AstarDictQ[heuristic] = new Queue<Node>();

                            AstarDictQ[heuristic].Enqueue(newNode);

                            if (IsFinished(newNode.currentState))
                            {
                                newSearchDetails.winningNode = newNode;
                                return newSearchDetails;
                            }
                        }
                    }
                }
            }
        }

        static SearchDetails CustomAStar(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            //  Queue<Node> nodeQ = new Queue<Node>();
            //  nodeQ.Enqueue(initialNode);
            newSearchDetails.nodesCreated++;
            int customAstarMax = CustomHeuristic(initialNode.currentState);
            Dictionary<int, Queue<Node>> AstarDictQ = new Dictionary<int, Queue<Node>>();
            AstarDictQ[customAstarMax] = new Queue<Node>();
            AstarDictQ[customAstarMax].Enqueue(initialNode);


            while (true)
            {
                if (!AstarDictQ.ContainsKey(customAstarMax) || AstarDictQ[customAstarMax].Count == 0)
                {
                    customAstarMax--;
                }
                else
                {

                    Node current = AstarDictQ[customAstarMax].Dequeue();
                    newSearchDetails.nodesExpanded++;
                    List<Direction> possibleDirections = PossibleDirections(current.currentState);
                    foreach (Direction d in possibleDirections)
                    {
                        int[,] tempCurrentState = DeepCopy(current.currentState);
                        Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                        if (!pastStates.ContainsKey(GetHash(newNode.currentState)))
                        {

                            newSearchDetails.nodesCreated++;
                            newNode = SetUpNewNode(newNode, d);
                            pastStates[GetHash(newNode.currentState)] = true;

                            int heuristic = CustomHeuristic(newNode.currentState) - newNode.depth;
                            if (heuristic > customAstarMax)
                                customAstarMax = heuristic;

                            if (!AstarDictQ.ContainsKey(heuristic))
                                AstarDictQ[heuristic] = new Queue<Node>();

                            AstarDictQ[heuristic].Enqueue(newNode);

                            if (IsFinished(newNode.currentState))
                            {
                                newSearchDetails.winningNode = newNode;
                                return newSearchDetails;
                            }
                        }
                    }
                }
            }
        }
        static int ManhattanDistance(int[,] puzzle)
        {
            int total = 0;
            for(int i=0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                {
                    Tuple<int, int> correctPoint = GetCorrectPoint(puzzle[i, j]);
                    total += Math.Abs(i - correctPoint.Item1) + Math.Abs(j - correctPoint.Item2);
                   
                }


            return total;
        }
        static Tuple<int,int> GetCorrectPoint(int num)
        {
            switch (num)
            {
                case 1:
                    return Tuple.Create(0,0);
                case 2:
                    return Tuple.Create(0, 1);
                case 3:
                    return Tuple.Create(0, 2);
                case 4:
                    return Tuple.Create(1, 0);
                case 5:
                    return Tuple.Create(1, 1);
                case 6:
                    return Tuple.Create(1, 2);
                case 7:
                    return Tuple.Create(2, 0);
                case 8:
                    return Tuple.Create(2, 1);
                case -1:
                    return Tuple.Create(2, 2);
            }
            return null;
        }

        static int CustomHeuristic(int[,] puzzle)
        {
            int total = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    
                    total += CorrectSides(puzzle[i,j], puzzle, Tuple.Create(i,j));

                }
            return total;
        }

        static int CorrectSides(int num, int[,] puzzle, Tuple<int,int> location)
        {
            int total = 0;
            switch (num)
            {
                case 1:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == 4)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == 2)
                            total++;
                    break;
                case 2:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == 5)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == 3)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 1)
                            total++;
                    break;
                case 3:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == 6)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 2)
                            total++;
                    break;
                case 4:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == 7)
                            total++;
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 1)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == 5)
                            total++;
                    break;
                case 5:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == 8)
                            total++;
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 2)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == 6)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 4)
                            total++;
                    break;
                case 6:
                    if (location.Item1 < 2)
                        if (puzzle[location.Item1 + 1, location.Item2] == -1)
                            total++;
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 3)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 5)
                            total++;
                    break;



                case 7:
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 4)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == 8)
                            total++;
                    break;
                case 8:
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 5)
                            total++;
                    if (location.Item2 < 2)
                        if (puzzle[location.Item1, location.Item2 + 1] == -1)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 7)
                            total++;
                    break;
                case -1:
                    if (location.Item1 > 0)
                        if (puzzle[location.Item1 - 1, location.Item2] == 6)
                            total++;
                    if (location.Item2 > 0)
                        if (puzzle[location.Item1, location.Item2 - 1] == 8)
                            total++;
                    break;


            }
            return total;
        }



        static Dictionary<long, Node> startDict = new Dictionary<long, Node>();
        static Dictionary<long, Node> finishDict = new Dictionary<long, Node>();
        static SearchDetails BiDirectionalSearch(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            Queue<Node> nodeQ = new Queue<Node>();
            nodeQ.Enqueue(initialNode);
            newSearchDetails.nodesCreated++;

            int[,] finishState = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, -1 } };
            Node finishNode = new Node(finishState, new Queue<int[,]>(), new Queue<Direction>());
            
            Queue<Node> finishNodeQ = new Queue<Node>();
            
            finishNodeQ.Enqueue(finishNode);
            newSearchDetails.nodesCreated++;


            while (true)
            {
                Node current = nodeQ.Dequeue();
                newSearchDetails.nodesExpanded++;
                List<Direction> possibleDirections = PossibleDirections(current.currentState);
                foreach (Direction d in possibleDirections)
                {
                    int[,] tempCurrentState = DeepCopy(current.currentState);
                    Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                    if (!startDict.ContainsKey(GetHash(newNode.currentState)))
                    {

                        newSearchDetails.nodesCreated++;
                        newNode = SetUpNewNode(newNode, d);
                        startDict[GetHash(newNode.currentState)] = newNode;
                        nodeQ.Enqueue(newNode);
                        if (finishDict.ContainsKey(GetHash(newNode.currentState)))
                        {
                            //finished
                            return BiDirectionalSolutionHelper(newNode, finishDict[GetHash(newNode.currentState)], newSearchDetails);

                        }

                    }
                    
                }
                Node currentFinished = finishNodeQ.Dequeue();
                newSearchDetails.nodesExpanded++;
                List<Direction> possibleFinishDirections = PossibleDirections(currentFinished.currentState);
                foreach (Direction d in possibleFinishDirections)
                {
                    int[,] tempCurrentState = DeepCopy(currentFinished.currentState);
                    Node newNode = new Node(SwapTiles(d, tempCurrentState), currentFinished.stateHistory, currentFinished.moveHistory, currentFinished.depth + 1);
                    if (!finishDict.ContainsKey(GetHash(newNode.currentState)))
                    {

                        newSearchDetails.nodesCreated++;
                        newNode = SetUpNewNode(newNode, d);
                       
                        finishDict[GetHash(newNode.currentState)] = newNode;
                        finishNodeQ.Enqueue(newNode);
                        if (startDict.ContainsKey(GetHash(newNode.currentState)))
                        {
                            //finished
                            return BiDirectionalSolutionHelper(startDict[GetHash(newNode.currentState)],newNode,newSearchDetails);
                        }

                    }
                    
                }
            }
        }

        static SearchDetails BiDirectionalSolutionHelper(Node start, Node finish, SearchDetails sd)
        {
            
            Stack<Direction> reverseMoveStack = new Stack<Direction>();
            Stack<int[,]> reverseStateStack = new Stack<int[,]>();
            List<Direction> moveTranslatorList = new List<Direction>();
            while (finish.moveHistory.Count > 0)
            {
                moveTranslatorList.Add(finish.moveHistory.Dequeue());
            }

            for (int i = 0; i < moveTranslatorList.Count; i++)
            {
                if (moveTranslatorList[i] == Direction.left)
                    reverseMoveStack.Push(Direction.right);
                if (moveTranslatorList[i] == Direction.right)
                    reverseMoveStack.Push(Direction.left);
                if (moveTranslatorList[i] == Direction.up)
                    reverseMoveStack.Push(Direction.down);
                if (moveTranslatorList[i] == Direction.down)
                    reverseMoveStack.Push(Direction.up);
            }
            if (finish.currentState[2, 1] == -1)
                reverseMoveStack.Push(Direction.left);
            else
                reverseMoveStack.Push(Direction.down);
           


            

            
            while (reverseMoveStack.Count > 0)
                start.moveHistory.Enqueue(reverseMoveStack.Pop());

            while (finish.stateHistory.Count > 0)
                reverseStateStack.Push(finish.stateHistory.Dequeue());
            
            int[,] finishState = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, -1 } };

            

            while (reverseStateStack.Count > 0)
                start.stateHistory.Enqueue(reverseStateStack.Pop());
            start.stateHistory.Enqueue(finishState);


            if(start.stateHistory.Count != start.moveHistory.Count)
            {
                Console.WriteLine("Houston we have a problem");
            }

            start.depth += finish.depth + 1;
            sd.winningNode = start;
            return sd;

        }

        static Direction ReverseDirection(Direction d)
        {
            switch (d)
            {
                case Direction.up:
                    return Direction.down;
                case Direction.down:
                    return Direction.up;
                case Direction.left:
                    return Direction.right;
                case Direction.right:
                    return Direction.left;
            }
            return Direction.up;
        }

        static SearchDetails DepthFirstSearch(int[,] puzzle)
        {

            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            Stack<Node> nodeStack = new Stack<Node>();
            nodeStack.Push(initialNode);
            newSearchDetails.nodesCreated++;
            pastStates[GetHash(initialNode.currentState)] = true;

            while (true)
            {
                Node current = nodeStack.Pop();

                
                  
                    newSearchDetails.nodesExpanded++;
       
                List<Direction> possibleDirections = PossibleDirections(current.currentState);
                foreach (Direction d in possibleDirections)
                {
                    if (true)
                    {

                        int[,] tempCurrentState = DeepCopy(current.currentState);
                        tempCurrentState = SwapTiles(d, tempCurrentState);
                        if (!pastStates.ContainsKey(GetHash(tempCurrentState)))
                        {
                            Node newNode = new Node(tempCurrentState, current.stateHistory, current.moveHistory, current.depth + 1);
                            newSearchDetails.nodesCreated++;
                            newNode = SetUpNewNode(newNode, d);
                            pastStates[GetHash(newNode.currentState)] = true;
                            nodeStack.Push(newNode);
                            if (IsFinished(newNode.currentState))
                            {
                                newSearchDetails.winningNode = newNode;
                                return newSearchDetails;
                            }


                        }
                    }
                }
            }
        }

        static SearchDetails DepthLimitedSearch(int[,] puzzle, int limit)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            Stack<Node> nodeStack = new Stack<Node>();
            nodeStack.Push(initialNode);
            newSearchDetails.nodesCreated++;
            pastStates[GetHashWithDepth(initialNode.currentState,0)] = true;

            while (true)
            {
                if (nodeStack.Count == 0)
                    return null;
                Node current = nodeStack.Pop();
                newSearchDetails.nodesExpanded++;
                List<Direction> possibleDirections = PossibleDirections(current.currentState);
                foreach (Direction d in possibleDirections)
                {
                    int[,] tempCurrentState = DeepCopy(current.currentState);
                    
                    Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                    if (!pastStates.ContainsKey(GetHashWithDepth(newNode.currentState,newNode.depth)) && newNode.depth < limit)
                    {
                        
                        pastStates[GetHashWithDepth(newNode.currentState,newNode.depth)] = true;
                        newSearchDetails.nodesCreated++;
                        newNode = SetUpNewNode(newNode, d);
                        nodeStack.Push(newNode);
                        if (IsFinished(newNode.currentState))
                        {
                            newSearchDetails.winningNode = newNode;
                            return newSearchDetails;
                        }
                    }
                    //else if(pastStates.ContainsKey(GetHash(newNode.currentState)) && newNode.depth < 3)
                    //{
                    //    PrintPuzzle(newNode.currentState);
                    //    Console.WriteLine();
                    //}
                }
            }
        }

        static SearchDetails IterativeDeepiningSearch(int[,] puzzle)
        {
            SearchDetails newSearchDetails = new SearchDetails();
            Node initialNode = new Node(puzzle, new Queue<int[,]>(), new Queue<Direction>());
            if (IsFinished(puzzle))
            {
                newSearchDetails.winningNode = initialNode;
                return newSearchDetails;
            }
            Stack<Node> nodeStack = new Stack<Node>();
            nodeStack.Push(initialNode);
            newSearchDetails.nodesCreated++;

            int currentLimit = 1;
            while (true)
            {
                if (nodeStack.Count == 0)
                {
                    currentLimit++;
                    nodeStack.Push(initialNode);
                    pastStates.Clear();
                }
                else
                {
                    Node current = nodeStack.Pop();
                    newSearchDetails.nodesExpanded++;
                    List<Direction> possibleDirections = PossibleDirections(current.currentState);
                    foreach (Direction d in possibleDirections)
                    {
                        int[,] tempCurrentState = DeepCopy(current.currentState);
                        Node newNode = new Node(SwapTiles(d, tempCurrentState), current.stateHistory, current.moveHistory, current.depth + 1);
                        if (!pastStates.ContainsKey(GetHashWithDepth(newNode.currentState,newNode.depth)) && newNode.depth < currentLimit)
                        {

                            pastStates[GetHashWithDepth(newNode.currentState,newNode.depth)] = true;
                            newSearchDetails.nodesCreated++;
                            newNode = SetUpNewNode(newNode, d);
                            nodeStack.Push(newNode);
                            if (IsFinished(newNode.currentState))
                            {
                                newSearchDetails.winningNode = newNode;
                                return newSearchDetails;
                            }
                        }
                        
                    }
                }
            }
        }

        static Node SetUpNewNode(Node newNode, Direction d)
        {
           
            Queue<Direction> newMoveHistory = new Queue<Direction>(newNode.moveHistory);
            Queue<int[,]> newStateHistory = new Queue<int[,]>(newNode.stateHistory);
            
            if(newStateHistory.Count == 20)
            {
                newStateHistory.Dequeue();
                newStateHistory.Enqueue(newNode.currentState);
            }
            else
            newStateHistory.Enqueue(newNode.currentState);
            
            if(newMoveHistory.Count == 20)
            {
                newMoveHistory.Dequeue();
                newMoveHistory.Enqueue(d);
            }
            else
            newMoveHistory.Enqueue(d);

            newNode.stateHistory = newStateHistory;
            newNode.moveHistory = newMoveHistory;
            
            return newNode;
        }

        static int[,] DeepCopy(int[,] toCopy)
        {
            int[,] copied = new int[3,3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    copied[i, j] = toCopy[i, j];
                }
            return copied;
        }
        public enum Direction
        {
            left,
            right,
            up,
            down,
        }

        static void PrintDirections(List<Direction> directions)
        {
            Console.WriteLine();
            foreach (Direction d in directions)
            {
                Console.Write(d + " ");
            }
        }


        static bool IsFinished(int[,] puzzle)
        {

            int[,] finishedPuzzle = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, -1 } };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    if (puzzle[i, j] != finishedPuzzle[i, j])
                        return false;
            }

            return true;

        }

        static List<Direction> PossibleDirections(int[,] puzzle)
        {
            int xPos = -1;
            int yPos = -1;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (puzzle[i, j] == -1)
                    {
                        xPos = i;
                        yPos = j;
                    }
            // Console.WriteLine("hmm" + xPos + " " + yPos);
            List<Direction> movesList = new List<Direction>();
            if (xPos > 0)
                movesList.Add(Direction.up);
            if (yPos < 2)
                movesList.Add(Direction.right);
            if (yPos > 0)
                movesList.Add(Direction.left);
            if (xPos < 2)
                movesList.Add(Direction.down);

            return movesList;

        }

        static void PrintPuzzle(int[,] puzzle)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < 3; j++)
                    if (puzzle[i, j] != -1)
                        Console.Write(puzzle[i, j] + " ");
                    else
                        Console.Write("_ ");
            }
            Console.WriteLine();
        }

        static int[,] ShufflePuzzle(int[,] puzzle)
        {
            Random rand = new Random();
            for(int i = 0; i < 50; i++)
            {
                List<Direction> possibleDirections = PossibleDirections(puzzle);
                puzzle = SwapTiles(possibleDirections[rand.Next(0, possibleDirections.Count)], puzzle);
            }
            return puzzle;
            
        }

        static int[,] SwapTiles(Direction dir, int[,] puzzle)
        {
            int xPos = -1;
            int yPos = -1;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (puzzle[i, j] == -1)
                    {
                        xPos = i;
                        yPos = j;
                        break;
                    }
            switch (dir)
            {
                case Direction.down:
                    puzzle[xPos, yPos] = puzzle[xPos + 1, yPos ];
                    puzzle[xPos + 1, yPos ] = -1;
                    break;
                case Direction.left:
                    puzzle[xPos, yPos] = puzzle[xPos , yPos -1];
                    puzzle[xPos, yPos-1] = -1;
                    break;
                case Direction.right:
                    puzzle[xPos, yPos] = puzzle[xPos, yPos + 1];
                    puzzle[xPos, yPos + 1] = -1;
                    break;
                case Direction.up:
                    puzzle[xPos, yPos] = puzzle[xPos - 1, yPos];
                    puzzle[xPos -1, yPos ] = -1;
                    break;
            }

            return puzzle;


        }
        static int[,] ParseInput(string input)
        {
            int[,] puzzle = new int[3, 3];
            int[] flatPuzzle = new int[9];
            int flatPuzzleIndex = 0;
            foreach(char c in input)
            {
                if (char.IsDigit(c))
                {
                    flatPuzzle[flatPuzzleIndex] = (int)char.GetNumericValue(c);
                    flatPuzzleIndex++;
                }
                else if(c == '_')
                {
                    flatPuzzle[flatPuzzleIndex] = -1;
                    flatPuzzleIndex++;
                }
            }
            flatPuzzleIndex = 0;
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                {
                    puzzle[i, j] = flatPuzzle[flatPuzzleIndex++];
                }
            return puzzle;
        }

        static void Main(string[] args)
        {
            int[,] initialState = { { 8, 3, 6 }, {5,2, 4 }, {1,7,-1 } };
           // int[,] initialState = { {1, 2, 3 }, { 4, 5, 6 }, { 7, 8, -1 } };

           
            
            Stopwatch stopWatch = new Stopwatch();
            SearchDetails searchDetails = new SearchDetails();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome To Tayler's Search Algorithms, Pick a number to proceed");
                Console.WriteLine("Current Puzzle is: ");
                PrintPuzzle(initialState);
                Console.WriteLine("1. Read input from puzzle.txt");
                Console.WriteLine("2. Shuffle current puzzle");
                Console.WriteLine("3. Breadth First Search Solution");
                Console.WriteLine("4. Depth First Search Solution");
                Console.WriteLine("5. Depth Limited Search Solution (Depth limit is 30)");
                Console.WriteLine("6. Iterative Deepining Search Solution");
                Console.WriteLine("7. Bi-directional Search Solution");
                Console.WriteLine("8. Greedy search solution");
                Console.WriteLine("9. AStar book solution");
                Console.WriteLine("10. CustomAstar solution");

                int parse;
                bool canParse = Int32.TryParse(Console.ReadLine(), out parse);

                if (!canParse || parse > 10)
                {
                    Console.WriteLine("Please try again");
                }
                else
                {
                    switch (parse)
                    {
                        case 1:
                            string stringState;
                            using (var sr = new StreamReader("puzzle.txt"))
                                stringState = sr.ReadToEnd();
                            initialState = ParseInput(stringState);
                           
                            break;
                        case 2:
                            initialState = ShufflePuzzle(initialState);
                          
                            break;
                        case 3:
                            stopWatch.Start();
                            searchDetails = BreadthFirstSearch(initialState);
                            stopWatch.Stop();
                            break;
                        case 4:
                            stopWatch.Start();
                            searchDetails = DepthFirstSearch(initialState);
                            stopWatch.Stop();
                            break;
                        case 5:
                            stopWatch.Start();
                            searchDetails = DepthLimitedSearch(initialState, 30);
                            stopWatch.Stop();
                            break;
                        case 6:
                            stopWatch.Start();
                            searchDetails = IterativeDeepiningSearch(initialState);
                            stopWatch.Stop();
                            break;
                        case 7:
                            stopWatch.Start();
                            searchDetails = BiDirectionalSearch(initialState);
                            stopWatch.Stop();
                            break;
                        case 8:
                            stopWatch.Start();
                            searchDetails = GreedySearch(initialState);
                            stopWatch.Stop();
                            break;
                        case 9:
                            stopWatch.Start();
                            searchDetails = AStarBook(initialState);
                            stopWatch.Stop();
                            break;
                        case 10:
                            stopWatch.Start();
                            searchDetails = CustomAStar(initialState);
                            stopWatch.Stop();
                            break;

                    }
                    if (parse > 2)
                    {
                        if (searchDetails != null)
                        {
                            searchDetails.time = stopWatch.ElapsedMilliseconds.ToString();
                            searchDetails.initialConfig = initialState;
                            PrintSolution(searchDetails);
                            stopWatch.Reset();
                            pastStates.Clear();
                            finishDict.Clear();
                            startDict.Clear();
                        }
                        else
                        {
                            stopWatch.Reset();
                            pastStates.Clear();
                            finishDict.Clear();
                            startDict.Clear();
                            Console.WriteLine("No solution found");
                        }
                    }
                }
            }
           
           
            
            

        }

        static void PrintSolution(SearchDetails solved)
        {
            Console.WriteLine("Time taken: " + solved.time + " milliseconds");
            Console.WriteLine("Nodes Created: " + solved.nodesCreated);
            Console.WriteLine("Nodes Expanded: " + solved.nodesExpanded);
            Console.WriteLine();
            Console.Write("Initial puzzle: ");
            PrintPuzzle(solved.initialConfig);
      
            Console.WriteLine();
            Console.WriteLine("total amount of steps is: " + solved.winningNode.depth);

            Console.WriteLine("Showing the last " + solved.winningNode.stateHistory.Count + " step(s)");
            while (solved.winningNode.moveHistory.Count > 0)
            {
                Console.Write(solved.winningNode.moveHistory.Dequeue());
                PrintPuzzle(solved.winningNode.stateHistory.Dequeue());
                Console.WriteLine();
            }
            
            
        }



    }
}
