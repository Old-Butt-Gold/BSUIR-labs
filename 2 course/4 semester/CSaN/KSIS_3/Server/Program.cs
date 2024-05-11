using System.Net;
using System.Runtime.InteropServices;

class FileServer : IDisposable
{
    readonly HttpListener _httpListener;
    readonly string _logFilePath;
    readonly StreamWriter StreamWriter;
    
    const string BasePath = @"D:\KSIS_3\KSIS_3_Files\";

    public FileServer(string logDirectory, params string[] baseUrls)
    {
        _httpListener = new HttpListener();
        foreach (var baseUrl in baseUrls)
        {
            _httpListener.Prefixes.Add(baseUrl);
        }

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        _logFilePath = Path.Combine(logDirectory, "server_log.txt");
        StreamWriter = new StreamWriter(_logFilePath, true)
        {
            AutoFlush = true,
        };
     
        Log($"{DateTime.Now}: - Сервер запущен.\n");
    }

    public void Start()
    {
        _httpListener.Start();
        Log("Слушает запросы на: " + string.Join("; ", _httpListener.Prefixes));
        while (true)
        {
            var context = _httpListener.GetContext();
            ProcessRequest(context);
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;
        var path = BasePath + context.Request.Url!.LocalPath[1..];
        
        Log($"{DateTime.Now}: - Принят {request.HttpMethod} запрос для {path}");
        
        switch (request.HttpMethod)
        {
            case "GET":
                ServeFile(path, response);
                break;
            case "PUT":
                SaveFile(path, request.InputStream, response);
                break;
            case "POST":
                AppendToFile(path, request.InputStream, response);
                break;
            case "DELETE":
                DeleteFile(path, response);
                break;
            case "COPY":
                CopyFile(path, response);
                break;
            case "MOVE":
                MoveFile(path, BasePath + request.Headers["Destination"], response);
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                break;
        }

        response.Close();
    }

    void ServeFile(string filePath, HttpListenerResponse response)
    {
        if (File.Exists(filePath))
        {
            using var fs = File.OpenRead(filePath);
            response.ContentLength64 = fs.Length;
            fs.CopyTo(response.OutputStream);
            response.StatusCode = (int)HttpStatusCode.OK;
            Log($"Файл {filePath} успешно обслужен.");
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            Log($"Файл {filePath} не найден.");
        }
    }

    void SaveFile(string filePath, Stream inputStream, HttpListenerResponse response)
    {
        try
        {
            using var fs = File.Create(filePath);
            inputStream.CopyTo(fs);
            response.StatusCode = (int)HttpStatusCode.OK;
            Log($"Файл {filePath} успешно сохранен.");
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Log($"Ошибка при сохранении файла {filePath}: {ex.Message}");
        }
    }

    void AppendToFile(string filePath, Stream inputStream, HttpListenerResponse response)
    {
        try
        {
            using var fs = File.Open(filePath, FileMode.Append);
            inputStream.CopyTo(fs);
            response.StatusCode = (int)HttpStatusCode.OK;
            Log($"Данные успешно добавлены в файл {filePath}.");
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Log($"Ошибка при добавлении данных в файл {filePath}: {ex.Message}");
        }
    }

    void DeleteFile(string filePath, HttpListenerResponse response)
    {
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                response.StatusCode = (int)HttpStatusCode.OK;
                Log($"Файл {filePath} успешно удален.");
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Log($"Ошибка при удалении файла {filePath}: {ex.Message}");
            }
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            Log($"Файл {filePath} не найден.");
        }
    }

    void CopyFile(string sourceFilePath, HttpListenerResponse response)
    {
        if (File.Exists(sourceFilePath))
        {
            try
            {
                using var fs = File.Open(sourceFilePath, FileMode.Open);
                fs.CopyTo(response.OutputStream);
                response.StatusCode = (int)HttpStatusCode.OK;
                Log($"Файл {sourceFilePath} успешно скопирован.");
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Log($"Ошибка при копировании файла {sourceFilePath}: {ex.Message}");
            }
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            Log($"Файл {sourceFilePath} не найден.");
        }
    }

    void MoveFile(string sourceFilePath, string destinationFilePath, HttpListenerResponse response)
    {
        if (File.Exists(sourceFilePath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFilePath)!);

                File.Move(sourceFilePath, destinationFilePath, true);

                response.StatusCode = (int)HttpStatusCode.OK;
        
                Log($"Файл {sourceFilePath} успешно перемещен в {destinationFilePath}.");
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Log($"Ошибка при перемещении файла {sourceFilePath}: {ex.Message}");
            }
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.NotFound;
            Log($"Файл {sourceFilePath} не найден.");
        }
    }

    void Log(string message)
    {
        StreamWriter.WriteLine($"{DateTime.Now}: " + message);
    }

    public void Dispose()
    {
        Log($"{DateTime.Now}: - Сервер завершает работу...\n");
        StreamWriter.Close();
        StreamWriter.Dispose();
        _httpListener.Stop();
        _httpListener.Close();
    }
}

class Program
{
    [DllImport("Kernel32")]
    public static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add); //Для корректного завершения программы
    
    public delegate bool HandlerRoutine(CtrlTypes ctrlType);
    
    public enum CtrlTypes
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT,
        CTRL_CLOSE_EVENT,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT
    }
    
    private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
    {
        if (ctrlType == CtrlTypes.CTRL_CLOSE_EVENT)
            Server?.Dispose();
        return true;
    }

    static FileServer? Server { get; set; }
    
    static void Main(string[] args)
    {
        SetConsoleCtrlHandler(ConsoleCtrlCheck, true);
        string logDirectory = @"D:/KSIS_3/Server/logs"; // Путь к каталогу для логов
        try
        {
            Server = new FileServer(logDirectory, "http://localhost:8080/", "http://127.0.0.1:8080/");
            Server.Start();
        }
        finally
        {
            Server?.Dispose();
        }

    }
}