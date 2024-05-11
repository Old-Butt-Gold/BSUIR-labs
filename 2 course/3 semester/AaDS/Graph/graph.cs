Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Введите количество вершин у вашего графа");
int amount = int.Parse(Console.ReadLine());
Graph graph = new(amount);
for (int i = 0; i < graph.VertexCount; i++)
    graph.VertexNames[i] = (char)(i + 'A');
//Ациклический граф
/*graph.AddOrientedEdge(0, 1, 3);
graph.AddOrientedEdge(0, 2, 5);
graph.AddOrientedEdge(0, 3, 14);
graph.AddOrientedEdge(1, 2, 11);
graph.AddOrientedEdge(1, 4, 7);
graph.AddOrientedEdge(1, 3, 6);
graph.AddOrientedEdge(2, 4, 3);
graph.AddOrientedEdge(2, 5, 2);
graph.AddOrientedEdge(3, 4, 7);
graph.AddOrientedEdge(3, 6, 6);
graph.AddOrientedEdge(4, 6, 5);
graph.AddOrientedEdge(5, 6, 7);*/
//Дейкстра работает только для графов без рёбер отрицательного веса.
//Т.е дейкстра не работает на ациклический и циклический граф с отрицательными весами

while (true)
{
    Console.Clear();
    Console.WriteLine("Выберите действие:");
    Console.WriteLine("Введите 0 для добавления связи\nВведите 1 для вывода информации о графе\nВведите 2 для вычисления центра графа\nВведите 3 для вывода таблицы кратчайших путей\nВведите 4 для вывода таблицы длиннейших путей\nВведите 5 для вычисления, является ли граф циклическим\nВведите 6 для вычисления всех путей между двумя точками\nВведите 7 для вычисления pre и post значений\nВведите 8 для вычисления кратчайшего пути между двумя точками\nВведите 9 для вычисления длиннейшего пути между двумя точками\nВведите 10 для вычисления стоков и истоков");
    switch (int.Parse(Console.ReadLine()))
    {
        case 0:
            Console.WriteLine("Введите начальную точку, конечную точку и вес");
            var str = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            graph.AddOrientedEdge(int.Parse(str[0]), int.Parse(str[1]), int.Parse(str[2]));
            break;
        case 1:
            graph.PrintInfo();
            break;
        case 2:
            int centerVertex = graph.FindGraphCenter();
            Console.WriteLine("Центр орграфа: " + graph.VertexNames[centerVertex] + "\n");
            break;
        case 3:
            Console.WriteLine("Метод Дейкстры:\n");
            graph.PrintShortestPathsTable();
            Console.WriteLine("Метод Флойда-Уоршелла: \n");
            graph.PrintFloydWarshallShortestTable();
            break;
        case 4:
            if (graph.IsCyclic())
                Console.WriteLine("Граф циклический");
            else
                graph.PrintLongestDAGPathsTable();
            break;
        case 5:
            Console.WriteLine(graph.IsCyclic());
            break;
        case 6:
            Console.WriteLine("Введите начальную точку, конечную точку");
            var strPath = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result = graph.AllPaths(int.Parse(strPath[0]), int.Parse(strPath[1]));
            graph.PrintPathsWithSum(result);
            break;
        case 7:
            Console.WriteLine("Введите стартовую точку:");
            int start = int.Parse(Console.ReadLine());
            var resultOrder = graph.CalculatePrePostOrder(start);
            for (int i = 0; i < graph.VertexCount; i++)
                Console.WriteLine(graph.VertexNames[i] + ": pre = " + resultOrder.pre[i] + ", post = " + resultOrder.post[i]);
            break;
        case 8:
            Console.WriteLine("Введите начальную точку, конечную точку");
            var strPath1 = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result1 = graph.GetPathTo(int.Parse(strPath1[0]), int.Parse(strPath1[1]), true);
            graph.PrintPathWithSum(result1);
            break;
        case 9:
            Console.WriteLine("Введите начальную точку, конечную точку");
            var strPath2 = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var result2 = graph.GetPathTo(int.Parse(strPath2[0]), int.Parse(strPath2[1]), false);
            graph.PrintPathWithSum(result2);
            break;
        case 10:
            graph.SinkAndSources();
            break;
    }

    Console.ReadKey();
}

//Циклический граф
/*graph.AddOrientedEdge(0, 1, 1);
graph.AddOrientedEdge(1, 2, 2);
graph.AddOrientedEdge(2, 3, 2);
graph.AddOrientedEdge(3, 2, 3);
graph.AddOrientedEdge(2, 4, 4);
graph.AddOrientedEdge(4, 3, 5);
graph.AddOrientedEdge(3, 1, 1);*/

class Graph
{
    const int Border = 10000;
    
    public List<List<(int vertex, int weight)>> _Graph { get; private set; }
    public int VertexCount { get; }
    public char[] VertexNames { get; set; }

    public Graph(int count)
    {
        VertexCount = count;
        _Graph = new();
        for (int i = 0; i < VertexCount; i++)
            _Graph.Add(new());
        VertexNames = new char[VertexCount];
    }

    public void PrintInfo()
    {
        for (int i = 0; i < VertexCount; i++)
        {
            foreach ((int vertex, int weight) item in _Graph[i])
            {
                Console.Write(VertexNames[i]);
                Console.Write($" -({item.weight})-> ");
                Console.WriteLine(VertexNames[item.vertex]);
            }
        }
    }

    public void AddEdge(int a, int b, int weight)
    {
        _Graph[a].Add((b, weight));
        _Graph[b].Add((a, weight));
    }

    public void AddOrientedEdge(int a, int b, int weight)
    {
        _Graph[a].Add((b, weight));
    }

    public int[] GetNeighbors(int vertex)
    {
        int[] neighbors = new int[_Graph[vertex].Count];
        int i = 0;
        foreach (var (neighbor, _) in _Graph[vertex])
            neighbors[i++] = neighbor;
        Array.Sort(neighbors);
        return neighbors;
    }

    public int GetEdgeWeight(int a, int b) => _Graph[a].Single(x => x.vertex == b).weight;
        /*foreach (var (vertex, weight) in _Graph[a])
            if (vertex == b)
                return weight;
        return 0;*/

    public void BreadthFirstSearch(int startVertex)
{
    Queue<int> queue = new();
    bool[] visited = new bool[VertexCount];

    queue.Enqueue(startVertex);
    visited[startVertex] = true;

    while (queue.Count > 0)
    {
        int currentVertex = queue.Dequeue();
        Console.Write($"{VertexNames[currentVertex]} ");

        foreach (var neighbor in GetNeighbors(currentVertex))
        {
            if (!visited[neighbor])
            {
                queue.Enqueue(neighbor);
                visited[neighbor] = true;
            }
        }
    }

    Console.WriteLine();
}

public void DepthFirstSearch(int startVertex)
{
    bool[] visited = new bool[VertexCount];
    DFS(startVertex);

    void DFS(int vertex)
    {
        visited[vertex] = true;
        Console.Write($"{VertexNames[vertex]} ");

        foreach (var neighbor in GetNeighbors(vertex))
        {
            if (!visited[neighbor])
            {
                DFS(neighbor);
            }
        }
    }

    Console.WriteLine();
}


    public int[] ShortestPath(int startVertex) //Дейкстра
    {
        int[] distances = new int[VertexCount];
        bool[] visited = new bool[VertexCount];
        Array.Fill(distances, int.MaxValue);

        distances[startVertex] = 0;

        for (int i = 0; i < VertexCount; i++)
        {
            int u = FindVertexWithMinDistance();
            if (u != -1)
            {
                visited[u] = true;

                int[] neighbors = GetNeighbors(u);
                foreach (int v in neighbors)
                {
                    if (!visited[v] && distances[u] != int.MaxValue)
                    {
                        int weight = GetEdgeWeight(u, v);
                        if (distances[u] + weight < distances[v])
                            distances[v] = distances[u] + weight;
                    }
                }
            }
        }

        return distances;
        
        int FindVertexWithMinDistance()
        {
            int minDistance = int.MaxValue;
            int minVertex = -1;

            for (int v = 0; v < VertexCount; v++)
            {
                if (!visited[v] && distances[v] < minDistance)
                {
                    minDistance = distances[v];
                    minVertex = v;
                }
            }

            return minVertex;
        }
    }
    
    //Транспонирование
    
    public List<List<(int vertex, int weight)>> TransposeGraph()
    {
        List<List<(int vertex, int weight)>> transposedGraph = new(VertexCount);

        for (int i = 0; i < VertexCount; i++)
            transposedGraph.Add(new());

        for (int i = 0; i < VertexCount; i++)
        {
            foreach (var (vertex, weight) in _Graph[i])
                transposedGraph[vertex].Add((i, weight));
        }

		return transposedGraph;

    }
    
    //pre и post для ациклических одни значения, для циклических могут варьироваться

    public (int[] pre, int[] post) CalculatePrePostOrder(int start)
    {
        int clock = 0;
        int[] pre = new int[VertexCount];
        int[] post = new int[VertexCount];
        bool[] visited = new bool[VertexCount];
        
        for (int i = start; i < VertexCount; i++)
            if (!visited[i])
                Search(i);
        return (pre, post);
        
        void Search(int vertex)
        {
            visited[vertex] = true;
            pre[vertex] = clock++;
            foreach (int i in GetNeighbors(vertex))
                if (!visited[i])
                    Search(i);
            post[vertex] = clock++;
        }
    }
    
    //проверка является ли граф циклическим
    public bool IsCyclic()
    {
        bool[] visited = new bool[VertexCount];
        bool[] stack = new bool[VertexCount];

        for (int i = 0; i < VertexCount; i++)
        {
            if (!visited[i] && IsCyclicUtil(i))
                return true;
        }

        return false;
        
        bool IsCyclicUtil(int vertex)
        {
            if (!visited[vertex])
            {
                visited[vertex] = true;
                stack[vertex] = true;

                foreach (var (neighbor, _) in _Graph[vertex])
                {
                    if (!visited[neighbor] && IsCyclicUtil(neighbor))
                        return true;
                    if (stack[neighbor])
                        return true;
                }
            }

            stack[vertex] = false;
            return false;
        }
    }
    
    //Работает только для ациклических графов
    public List<int> TopologicalSort()
    {
        List<int> result = new(); //использовать СТЕК
        bool[] visited = new bool[VertexCount];

        for (int i = 0; i < VertexCount; i++)
            if (!visited[i])
                TopologicalSortUtil(i);

        result.Reverse();
        return result;
        
        void TopologicalSortUtil(int vertex)
        {
            visited[vertex] = true;
            foreach (var neighbor in GetNeighbors(vertex))
                if (!visited[neighbor])
                    TopologicalSortUtil(neighbor);
            result.Add(vertex);
        }
    }

    //DFS
    
    public (List<string> sources, List<string> sinks) SinkAndSources()
    {
        List<string> sources = new();
        List<string> sinks = new();

        // Создаем массив, который будет хранить информацию о входящих и исходящих ребрах
        int[] inDegrees = new int[VertexCount];
        int[] outDegrees = new int[VertexCount];

        // Вычисляем in-degrees и out-degrees для каждой вершины
        for (int i = 0; i < VertexCount; i++)
        {
            foreach (var (neighbor, _) in _Graph[i])
            {
                outDegrees[i]++;
                inDegrees[neighbor]++;
            }
        }

        // Определяем истоки и стоки
        for (int i = 0; i < VertexCount; i++)
        {
            if (inDegrees[i] == 0 && outDegrees[i] > 0)
                sources.Add(VertexNames[i].ToString());

            if (outDegrees[i] == 0 && inDegrees[i] > 0)
                sinks.Add(VertexNames[i].ToString());
        }

        Console.WriteLine("Sources: " + string.Join(", ", sources));
        Console.WriteLine("Sinks: " + string.Join(", ", sinks));
        return (sources, sinks);
    }
    
    //Работает только для ациклических графов    
    public int[] LongestPathDAG(int startVertex)
    {
        List<int> topologicalOrder = TopologicalSort();
        int[] longestPaths = new int[VertexCount];
        Array.Fill(longestPaths, int.MinValue);

        longestPaths[startVertex] = 0;

        foreach (int vertex in topologicalOrder)
        {
            foreach (var (adjacentVertex, weight) in _Graph[vertex])
            {
                longestPaths[adjacentVertex] = Math.Max(longestPaths[adjacentVertex], longestPaths[vertex] + weight);
            }
        }

        return longestPaths;
    }

    public void PrintLongestDAGPathsTable()
    {
        // Самый длинный путь в ациклическом графе
        Console.WriteLine("Самый длинный путь:");
        Console.Write("        ");
        for (int i = 0; i < VertexCount; i++)
            Console.Write($"{VertexNames[i]}\t");
        Console.WriteLine();
        for (int i = 0; i < VertexCount; i++)
        {
            Console.Write($"{VertexNames[i]}:\t");
            int[] distances = LongestPathDAG(i);
            foreach (var item in distances)
                Console.Write(item < int.MinValue + Border ? "∞\t" : $"{item}\t");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    
    ///////////////////////////////////////////////
    
    public int FindGraphCenter()
    {
        int[][] eccentricities = new int[VertexCount][];

        for (int i = 0; i < VertexCount; i++)
            eccentricities[i] = ShortestPath(i);

        int[] maxEccentricities = new int[VertexCount];

        for (int i = 0; i < VertexCount; i++)
        {
            int maxEccentricity = int.MinValue;

            for (int j = 0; j < VertexCount; j++)
                maxEccentricity = Math.Max(maxEccentricity, eccentricities[j][i]);

            maxEccentricities[i] = maxEccentricity;
        }

        int minMaxEccentricity = maxEccentricities.Min();
        int centerVertex = Array.IndexOf(maxEccentricities, minMaxEccentricity);

        return centerVertex;
    }

    public List<(List<int> Path, int Sum)> AllPaths(int startVertex, int endVertex)
    {
        List<(List<int> Path, int Sum)> allPaths = new();
        Stack<int> currentPath = new();
        bool[] visited = new bool[VertexCount];

        void DFS(int current, int currentSum)
        {
            visited[current] = true;
            currentPath.Push(current);

            if (current == endVertex)
            {
                var temp = currentPath.ToList();
                temp.Reverse();
                allPaths.Add((temp, currentSum));
            }
            else
            {
                foreach (int neighbor in GetNeighbors(current))
                {
                    if (!visited[neighbor])
                    {
                        int weight =  GetEdgeWeight(current, neighbor);
                        DFS(neighbor, currentSum + weight);
                    }
                }
            }

            visited[current] = false;
            currentPath.Pop();
        }

        DFS(startVertex, 0);
        allPaths.Sort((a, b) => a.Sum.CompareTo(b.Sum));

        return allPaths;
    }
    
    public (List<int> Path, int Sum) GetPathTo(int startVertex, int endVertex, bool isShortest)
    {
        Stack<int> currentPath = new();
        bool[] visited = new bool[VertexCount];
        List<int> bestPath = new();
        int bestPathSum = isShortest ? int.MaxValue : int.MinValue;

        void DFS(int current, int currentSum)
        {
            visited[current] = true;
            currentPath.Push(current);

            if (current == endVertex && (isShortest ? currentSum < bestPathSum : currentSum > bestPathSum))
            {
                bestPath = currentPath.ToList();
                bestPath.Reverse();
                bestPathSum = currentSum;
            }
            else
            {
                foreach (int neighbor in GetNeighbors(current))
                {
                    if (!visited[neighbor])
                    {
                        int weight = GetEdgeWeight(current, neighbor);
                        DFS(neighbor, currentSum + weight);
                    }
                }
            }

            visited[current] = false;
            currentPath.Pop();
        }

        DFS(startVertex, 0);

        return (bestPath, bestPathSum);
    }
    
    public void PrintPath(List<int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Console.Write(VertexNames[path[i]]);
            if (i < path.Count - 1)
            {
                int weight = GetEdgeWeight(path[i], path[i + 1]);
                Console.Write($" -({weight})-> ");
            }
        }
        Console.WriteLine();
    }

    public void PrintPaths(List<List<int>> paths)
    {
        foreach (var path in paths)
            PrintPath(path);
    }

    public void PrintPathWithSum((List<int> Path, int Sum) path)
    {
        PrintPath(path.Path);
        Console.WriteLine($"Sum: {path.Sum}");
    }
    
    public void PrintPathsWithSum(List<(List<int> Path, int Sum)> paths)
    {
        foreach (var (path, sum) in paths)
        {
            PrintPath(path);
            Console.WriteLine($"Sum: {sum}");
        }
    }

    public void PrintShortestPathsTable()
    {
        // Самый короткий путь
        Console.WriteLine("Самый короткий путь:");
        Console.Write("        ");
        for (int i = 0; i < VertexCount; i++)
            Console.Write($"{VertexNames[i]}\t");
        Console.WriteLine();
        for (int i = 0; i < VertexCount; i++)
        {
            Console.Write($"{VertexNames[i]}:\t");
            int[] distances = ShortestPath(i);
            foreach (var item in distances)
                Console.Write(item > int.MaxValue - Border ? "∞\t" : $"{item}\t");
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    
    public int[][] FloyWarshallShortest()
    {
        int[][] distances = new int[VertexCount][];

        for (int i = 0; i < VertexCount; i++)
        {
            distances[i] = new int[VertexCount];
            for (int j = 0; j < VertexCount; j++)
                distances[i][j] = i == j ? 0 : int.MaxValue;
        }

        for (int i = 0; i < VertexCount; i++)
        {
            foreach (var (vertex, weight) in _Graph[i])
            {
                distances[i][vertex] = weight;
            }
        }

        for (int k = 0; k < VertexCount; k++)
        {
            for (int i = 0; i < VertexCount; i++)
            {
                for (int j = 0; j < VertexCount; j++)
                {
                    if (distances[i][k] < int.MaxValue - Border && distances[k][j] < int.MaxValue - Border &&
                        distances[i][k] + distances[k][j] < distances[i][j])
                    {
                        distances[i][j] = distances[i][k] + distances[k][j];
                    }
                }
            }
        }

        return distances;
    }

    public void PrintFloydWarshallShortestTable()
    {
        Console.WriteLine("Кратчайшие расстояния:");
        Console.Write("        ");
        for (int i = 0; i < VertexCount; i++)
            Console.Write($"{VertexNames[i]}\t");
        Console.WriteLine();

        int[][] distances = FloyWarshallShortest();

        for (int i = 0; i < VertexCount; i++)
        {
            Console.Write($"{VertexNames[i]}:\t");
            for (int j = 0; j < VertexCount; j++)
            {
                Console.Write(distances[i][j] > int.MaxValue - Border ? "∞\t" : $"{distances[i][j]}\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

}
