class Program
{
    static void Main(string[] args)
    {
        BinarySearchTree<int> tree = new BinarySearchTree<int>();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Добавить узел");
            Console.WriteLine("2 - Удалить узел");
            Console.WriteLine("3 - Поиск узла");
            Console.WriteLine("4 - Вывести дерево");
            Console.WriteLine("5 - Балансировать дерево");
            Console.WriteLine("6 - Вывести дерево всеми способами");
            Console.WriteLine("7 - Выйти из программы");
            Console.WriteLine("8 - Прошить дерево");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.Write("Введите значение для добавления: ");
                        tree.Add(int.Parse(Console.ReadLine()));
                        break;
                    case 2:
                        Console.Write("Введите значение для удаления: ");
                        Console.WriteLine(tree.Remove(int.Parse(Console.ReadLine())) ? "Узел удален." : "Узел не найден");
                        break;
                    case 3:
                        Console.Write("Введите значение для поиска: ");
                        TreeNode<int>? foundNode = tree.Search(int.Parse(Console.ReadLine()));
                        Console.WriteLine(foundNode != null ? ("Узел найден: " + foundNode.Value) : "Узел не найден");
                        break;
                    case 4:
                        Console.WriteLine("Дерево:");
                        tree.PrintTree();
                        break;
                    case 5:
                        tree.BalanceTree();
                        Console.WriteLine("Дерево балансировано.");
                        break;
                    case 6:
                        Console.WriteLine("Прямой обход дерева:");
                        var temp = tree.PreOrderTraversal();
                        Console.WriteLine();
                        foreach (var value in temp)
                            Console.Write(value + "\t");
                        Console.WriteLine("\nОбратный обход дерева:");
                        temp = tree.PostOrderTraversal();
                        Console.WriteLine();
                        foreach (var value in temp)
                            Console.Write(value + "\t");
                        Console.WriteLine("\nСимметричный обход дерева:");
                        temp = tree.InOrderTraversal();
                        Console.WriteLine();
                        foreach (var value in temp)
                            Console.Write(value + "\t");
                        Console.WriteLine("\nОбратный симметричный обход дерева:");
                        foreach (var value in tree.ReverseInOrderTraversal())
                            Console.Write(value + "\t");
                        Console.WriteLine("\nОбход в ширину дерева:");
                        foreach (var value in tree.LevelOrderTraversal())
                            Console.Write(value + "\t");
                        Console.WriteLine();
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    case 8:
                        var rightThreadedTree = tree.Clone() as BinarySearchTree<int>;
                        rightThreadedTree?.ThreadRightTree();
                        rightThreadedTree?.ThreadedTraversal();
                        break;
                    default:
                        Console.WriteLine("Некорректная команда. Пожалуйста, выберите действие из списка.");
                        break;
                }
            }
            Console.WriteLine("Нажмите кнопку для продолжения");
            Console.ReadKey();
        }
    }
}
