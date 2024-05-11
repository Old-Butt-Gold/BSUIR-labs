Processor processor = new Processor();

Console.WriteLine("Таблица значений тактов перед работой процессора:");
for (int i = 0; i < processor.Queue!.Length; i++)
{
    Console.BackgroundColor = i > 2 ? ConsoleColor.Blue : ConsoleColor.Green;
    Console.Write($"{i + 1}\t".PadLeft(5));
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Red;
    var workTimes = processor.Queue[i].WorkTime;

    foreach (var time in workTimes)
        Console.Write(time.ToString().PadLeft(4));
    Console.ResetColor();
    Console.WriteLine();
}
Console.WriteLine();
while (true)
{
    for (int i = 0; i < processor.Queue!.Length; i++)
    {
        var client = processor.Queue[i];
        if (!client.IsDone && !client.IsWorking)
        {
            processor.CheckClientsInput();
            processor.WorkWithClient(i);
        }
        
        /*Console.WriteLine();
        for (int j = 0; j < processor.Queue!.Length; j++)
        {
            Console.BackgroundColor = j > 2 ? ConsoleColor.Blue : ConsoleColor.Green;
            Console.Write($"{j + 1}\t".PadLeft(5));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            var workTimes = processor.Queue[j].WorkTime;

            foreach (var time in workTimes)
                Console.Write(time.ToString().PadLeft(4));
            Console.ResetColor();
            Console.WriteLine();
        }
        Console.WriteLine();*/
        
    }
    
    if (processor.Queue!.All(client => client.IsDone)) break;
    
    if (processor.Queue.All(client => client.IsWorking || client.IsDone))
    {
        processor.TimeSpent += processor.ProcessTime;
        processor.CheckClientsInput();
    }
}

int totalIdleTime = processor.Queue!.Sum(client => client.WorkTime.Sum(Math.Abs));
float efficiency = (float)(100 * processor.TimeNeeded) / (processor.TimeNeeded + processor.TimeSpent + totalIdleTime);

Console.WriteLine($"Время, которое необходимо было для обработки всех клиентов: {processor.TimeNeeded}");
Console.WriteLine($"Простой процессора во время ввода данных: {processor.TimeSpent}");
Console.WriteLine($"Простой процессора: {totalIdleTime}");
Console.WriteLine($"Общий простой процессора: {processor.TimeSpent + totalIdleTime}");
Console.WriteLine($"КПД процессора: {efficiency}%");

Console.WriteLine("\nТаблица значений тактов после окончания работы:");
for (int i = 0; i < processor.Queue.Length; i++)
{
    Console.BackgroundColor = i > 2 ? ConsoleColor.Blue : ConsoleColor.Green;
    Console.Write($"{i + 1}\t".PadLeft(5));
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Red;
    var workTimes = processor.Queue[i].WorkTime;

    foreach (var time in workTimes)
        Console.Write(time.ToString().PadLeft(4));
    Console.ResetColor();
    Console.WriteLine();
}

Console.ReadLine();