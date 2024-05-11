class Processor
{
    public Client[]? Queue { get; set; }
    public int TimeNeeded { get; set; }
    public int TimeSpent { get; set; }
    public int ProcessTime { get; set; }
    public int InputTime { get; set; }
    
    public Processor()
    {
        InitializeQueue();
        SetProcessAndInputTime();
        TimeNeeded = Queue!.Sum(client => client.WorkTime.Sum());
    }

    void InitializeQueue()
    {
        Queue = new Client[]
        {
            new () { WorkTime = new[] { 3, 2, 4, 3, 4, 5, 3, 2, 1 } },
            new () { WorkTime = new[] { 2, 1, 6, 3, 4, 5, 3, 2, 1, 4 } },
            new () { WorkTime = new[] { 4, 1, 3, 4, 5, 3, 2, 2, 3, 2, 2 } },
            new () { WorkTime = new[] { 6, 3, 3, 3, 2, 1, 2, 3, 4, 5, 6 } },
            new () { WorkTime = new[] { 2, 4, 4, 3, 3, 2, 1, 6, 5, 3, 3 } },
            new () { WorkTime = new[] { 3, 3, 4, 3, 5, 4, 3, 2, 1, 6, 3, 2 } },
        };
    }
    
    public void WorkWithClient(int index)
    {
        var client = Queue![index];

        for (int i = 0; i < client.WorkTime.Length; i++)
        {
            if (client.WorkTime[i] < 1)
                continue;
            
            client.WorkTime[i] -= ProcessTime;

            if (client.WorkTime[i] < 1)
            {
                if (i == client.WorkTime.Length - 1)
                {
                    client.IsDone = true;
                    break;
                }

                if (InputTime != 0)
                {
                    client.WorkingTimeLeft = InputTime + client.WorkTime[i];
                    client.IsWorking = true;
                }
            }
            break;
        }
    }

    public void CheckClientsInput()
    {
        foreach (var client in Queue!.Where(client => client.IsWorking))
        {
            client.WorkingTimeLeft = Math.Max(0, client.WorkingTimeLeft - ProcessTime);
            client.IsWorking = client.WorkingTimeLeft > 0;
        }
    }

    void SetProcessAndInputTime()
    {
        Console.Write("Process Time: ");
        ProcessTime = int.Parse(Console.ReadLine()!);
        ProcessTime = ProcessTime < 1 ? 1 : (ProcessTime > 10 ? 10 : ProcessTime);
        
        Console.Write("Input Time: ");
        InputTime = int.Parse(Console.ReadLine()!);
        InputTime = InputTime < 0 ? 0 : (InputTime > 10 ? 10 : InputTime);
    }
}