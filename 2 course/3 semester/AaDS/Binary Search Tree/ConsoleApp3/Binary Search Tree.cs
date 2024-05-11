public class BinarySearchTree<TValue> : ICloneable where TValue : IComparable<TValue>
{
    public int Count { get; set; }
    public bool IsEmpty => Count == 0;
    TreeNode<TValue>? Root;
    public BinarySearchTree() { }

    public BinarySearchTree(TValue value) => (Root, Count) = (new(value), 1);

    public BinarySearchTree(IEnumerable<TValue>? collection)
    {
        if (collection != null)
        {
            var sortedList = collection.ToList();
            sortedList.Sort();
            Root = CreateTreeFromList(sortedList, 0, sortedList.Count - 1);
        }
    }

    TreeNode<TValue>? CreateTreeFromList(List<TValue> list, int start, int end)
    {
        if (end < start) return null;
        int mid = (start + end) / 2;
        return new(list[mid])
        {
            Left = CreateTreeFromList(list, start, mid - 1),
            Right = CreateTreeFromList(list, mid + 1, end)
        };
    }

    public void Add(TValue value)
    {
        Root = AddToNode(Root, value);
        Count++;
    }
    
    TreeNode<TValue>? AddToNode(TreeNode<TValue>? node, TValue value)
    {
        if (node == null)
            return new(value);
        int comparison = value.CompareTo(node.Value);
        if (comparison < 0)
            node.Left = AddToNode(node.Left, value);
        else if (comparison > 0)
            node.Right = AddToNode(node.Right, value);
        return node;
    }

    public TreeNode<TValue>? Search(TValue value) => SearchNode(Root, value);

    public bool Contains(TValue value) => Search(value) != null;
    
    TreeNode<TValue>? SearchNode(TreeNode<TValue>? Root, TValue value)
    {
        if (Root == null) return null;
        
        int comparison = value.CompareTo(Root.Value);
        if (comparison == 0)
            return Root;
        
        return SearchNode(comparison < 0 ? Root.Left : Root.Right, value);
    }

    public bool Remove(TValue value)
    {
        bool result = false;
        Root = RemoveNode(Root, value, ref result);
        if (!result) return false;
        Count--;
        return true;
    }

    TreeNode<TValue>? RemoveNode(TreeNode<TValue>? root, TValue value, ref bool result)
    {
        if (root is null)
            return root;
        int comparison = value.CompareTo(root.Value);

        if (comparison < 0)
            root.Left = RemoveNode(root.Left, value, ref result);
        else if (comparison > 0)
            root.Right = RemoveNode(root.Right, value, ref result);
        else
        {
            result = true;
            if (root.Left == null)
                return root.Right;

            if (root.Right == null)
                return root.Left;

            root.Value = FindMinNode(root.Right)!.Value;
            root.Right = RemoveNode(root.Right, root.Value, ref result);
        }

        return root;
    }
    
    TreeNode<TValue>? FindMinNode(TreeNode<TValue>? node) => node?.Left == null ? node : FindMinNode(node.Left);
    
    public List<TValue> PreOrderTraversal()
    {
        List<TValue> list = new();
        PreOrderTraversal(Root, list.Add);
        return list;
    }

    void PreOrderTraversal(TreeNode<TValue>? node, Action<TValue> action)
    {
        if (node == null)
        {
            Console.Write("NULL ");
            return;
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(node.Value + " ");
        Console.ResetColor();
        action(node.Value);
        PreOrderTraversal(node.Left, action);
        Console.Write(node.Value + " ");
        Console.ResetColor();
        PreOrderTraversal(node.Right, action);
        Console.Write(node.Value + " ");
        Console.ResetColor();
    }

    public List<TValue> InOrderTraversal()
    {
        List<TValue> list = new();
        InOrderTraversal(Root, list.Add);
        return list;
    }

    void InOrderTraversal(TreeNode<TValue>? node, Action<TValue> action)
    {
        if (node == null)
        {
            Console.Write("NULL ");
            return;
        }
        Console.Write(node.Value + " ");
        Console.ResetColor();
        InOrderTraversal(node.Left, action);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(node.Value + " ");
        Console.ResetColor();
        action(node.Value);
        InOrderTraversal(node.Right, action);
        Console.Write(node.Value + " ");
        Console.ResetColor();
    }

    public List<TValue> ReverseInOrderTraversal()
    {
        List<TValue> list = new();
        ReverseInOrderTraversal(Root, list.Add);
        return list;
    }

    void ReverseInOrderTraversal(TreeNode<TValue>? node, Action<TValue> action)
    {
        if (node == null) return;
        
        ReverseInOrderTraversal(node.Right, action);
        action(node.Value);
        ReverseInOrderTraversal(node.Left, action);
    }

    public List<TValue> PostOrderTraversal()
    {
        List<TValue> list = new();
        PostOrderTraversal(Root, list.Add);
        return list;
    }

    void PostOrderTraversal(TreeNode<TValue>? node, Action<TValue> action)
    {
        if (node == null)
        {
            Console.Write("NULL ");
            return;
        }
        Console.Write(node.Value + " ");
        Console.ResetColor();
        PostOrderTraversal(node.Left, action);
        Console.Write(node.Value + " ");
        Console.ResetColor();
        PostOrderTraversal(node.Right, action);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(node.Value + " ");
        Console.ResetColor();
        action(node.Value);
    }

    public List<TValue> LevelOrderTraversal()
    {
        List<TValue> list = new();
        LevelOrderTraversal(list.Add);
        return list;
    }
    void LevelOrderTraversal(Action<TValue> action) //Breadth-first traversal (в ширину)
    {
        if (Root == null) return;

        Queue<TreeNode<TValue>> queue = new();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            TreeNode<TValue> node = queue.Dequeue();
            action(node.Value);

            if (node.Left != null)
                queue.Enqueue(node.Left);

            if (node.Right != null)
                queue.Enqueue(node.Right);
        }
    }

    public void Clear()
    {
        ClearRecursive(Root);
        Root = null;
        Count = 0;
    }

    void ClearRecursive(TreeNode<TValue>? node)
    {
        if (node == null)
            return;

        ClearRecursive(node.Left);
        ClearRecursive(node.Right);

        node.Left = null;
        node.Right = null;
    }
    
    public void BalanceTree()
    {
        List<TValue> list = InOrderTraversal();
        Clear();
        Root = CreateTreeFromList(list, 0, list.Count - 1);
    }
    
    public void PrintTree() => PrintTree(Root, "", true);

    void PrintTree(TreeNode<TValue>? node, string prefix, bool isTail)
    {
        if (node == null) return;

        Console.Write(prefix + (isTail ? "└── " : "├── ") + node.Value + "\n");
        PrintTree(node.Left, prefix + (isTail ? "    " : "│   "), false);
        PrintTree(node.Right, prefix + (isTail ? "    " : "│   "), true);
    }

    //Только после Clone для создания Прошитого симметричного справа дерева
    
    public void ThreadRightTree() => sim_print(Root);

    TreeNode<TValue>? Threaded_Root;

    void sim_print(TreeNode<TValue>? node)
    {
        if (node != null)
        {
            sim_print(node.Left);
            RightSew(node);
            sim_print(node.Right);
        }
        void RightSew(TreeNode<TValue> right)
        {
            if (Threaded_Root is not null)
            {
                if (Threaded_Root.Right is null)
                {
                    Threaded_Root.RightThreaded = true;
                    Threaded_Root.Right = right;
                }
            }
            Threaded_Root = right;
        }
    }
    
    public void ThreadedTraversal()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        TreeNode<TValue>? current = LeftMostNode();
        while (current != null)
        {
            Console.Write(current.Value + " ");
            if (current.Right is null)
            {
                Console.Write("(" + Root.Value + ") ");
                break;
            }
            current = current.RightThreaded ? current.Right : LeftMostNode(current.Right);
        }
        Console.ResetColor();
    }

    TreeNode<TValue>? LeftMostNode(TreeNode<TValue>? node = null)
    {
        node ??= Root; //Если null, то присвоить Root, иначе нет

        while (node is not null && node.Left is not null)
        {
            Console.Write(node.Value + " ");
            node = node.Left;
        }
        return node;
    }

    public object Clone()
    {
        BinarySearchTree<TValue> clonedTree = new();

        if (Root != null)
        {
            clonedTree.Root = CloneNode(Root);
            clonedTree.Count = Count;
        }

        return clonedTree;
    }

    TreeNode<TValue>? CloneNode(TreeNode<TValue>? node)
    {
        if (node == null)
            return null;

        TreeNode<TValue> clonedNode = new(node.Value);
        clonedNode.Left = CloneNode(node.Left);
        clonedNode.Right = CloneNode(node.Right);
        return clonedNode;
    }

}