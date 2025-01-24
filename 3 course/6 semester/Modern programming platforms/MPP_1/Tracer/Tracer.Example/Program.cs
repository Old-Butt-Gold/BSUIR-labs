using Tracer.Core;
using Tracer.Serialization;

var tracerExample = new Tracer.Core.Tracer();

var threads = new Thread[3];

for (int i = 0; i < threads.Length; i++)
{
    threads[i] = new Thread(() =>
    {
        TraceNestedMethods(tracerExample);
        TraceNestedMethods(tracerExample);
    });
    threads[i].Start();
}

// wait for other threads to end
foreach (var thread in threads)
{
    thread.Join();
}

void TraceNestedMethods(ITracer tracer)
{
    tracer.StartTrace();
    Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Start Main Method");

    Thread.Sleep(Random.Shared.Next(0, 100));

    TraceInnerMethods(tracer);
    TraceInnerMethods(tracer);

    Thread.Sleep(Random.Shared.Next(0, 100));
    
    tracer.StopTrace();
    Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] End Main Method");
}

static void TraceInnerMethods(ITracer tracer)
{
    tracer.StartTrace();
    
    Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Start Inner Method");

    Thread.Sleep(Random.Shared.Next(0, 100));

    tracer.StopTrace();
    
    Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] End Inner Method");
}

// Получение результата трассировки
var traceResult = tracerExample.GetTraceResult();

var serializers = PluginLoader.LoadSerializers();

foreach (var serializer in serializers)
{
    var fileName = $"result.{serializer.Format}";
    using var fileStream = new FileStream(fileName, FileMode.Create);
    serializer.Serialize(traceResult, fileStream);
    Console.WriteLine($"Result saved to {fileName}");
}