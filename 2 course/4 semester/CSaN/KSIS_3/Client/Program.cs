class Program
{
    static HttpClient HttpClient { get; set; }

    const string Url = "http://localhost:8080/";

    public static async Task Main(string[] args)
    {
        HttpClient = new HttpClient(new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        });

        while (true)
        {
            Console.WriteLine("Введите ваш метод: ");
            var method = Console.ReadLine();
            switch (method!.ToUpper())
            {
                case "GET":
                    await SendGetRequest();
                    break;
                case "PUT":
                    await SendPutRequest();
                    break;
                case "COPY":
                    await SendCopyRequest();
                    break;
                case "MOVE":
                    await SendMoveRequest();
                    break;
                case "POST":
                    await SendPostRequest();
                    break;
                case "DELETE":
                    await SendDeleteRequest();
                    break;
                default:
                    Console.WriteLine("Неподдерживаемый метод.");
                    break;
            }
        }
    }
    
    static async Task SendGetRequest()
    {
        try
        {
            Console.WriteLine("Введите имя файла: ");
            var fileName = Console.ReadLine();
            HttpResponseMessage response = await HttpClient.GetAsync(Url + fileName);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
        
            Console.WriteLine($"Содержимое файла {fileName}: ");
            Console.WriteLine(content); 
            Console.WriteLine("\nДля продолжения нажмите любую клавишу");
            Console.ReadKey();
            Console.Clear();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка получения GET-запроса: {e.Message}");
        }
    }
    
    static async Task SendPutRequest()
    {
        try
        {
            Console.WriteLine("Введите путь к файлу: ");
            var path = Console.ReadLine();
            Console.WriteLine("Введите имя файла для перезаписи");
            var fileName = Console.ReadLine();
            if (File.Exists(path))
            {
                await using var fileStream = File.Open(path, FileMode.Open);
                HttpResponseMessage response = await HttpClient.PutAsync(Url + fileName, new StreamContent(fileStream));
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Файл {fileName} успешно перезаписан новым {path} файлом");    
            }
            else
            {
                Console.WriteLine("Файла не существует");
            }
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка PUT-запроса: {e.Message}");
        }
    }
    
    static async Task SendCopyRequest()
    {
        try
        {
            Console.WriteLine("Введите путь для сохранения файла: ");
            var path = Console.ReadLine();
            Console.WriteLine("Введите имя файла для копирования");
            var fileName = Console.ReadLine();
            HttpRequestMessage request = new(HttpMethod.Parse("COPY"), Url + fileName);
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            await using var fileStream = File.Open(path!, FileMode.Create);
            var responseStream = await response.Content.ReadAsStreamAsync();
            await responseStream.CopyToAsync(fileStream);

            Console.WriteLine($"Файл {fileName} успешно сохранен в {path}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка PUT-запроса: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка, введите корректный путь для сохранения файла {e.Message}");
        }
    }
    
    static async Task SendMoveRequest()
    {
        try
        {
            Console.WriteLine("Введите имя файла");
            var fileName = Console.ReadLine();
            Console.WriteLine("Введите новое расположение файла");
            var path = Console.ReadLine();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Parse("MOVE"), Url + fileName);
            request.Headers.Add("Destination", path);
            HttpResponseMessage response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("MOVE-запрос выполнен успешно.");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка MOVE-запроса: {e.Message}");
        }
    }
    
    static async Task SendPostRequest()
    {
        try
        {
            Console.WriteLine("Введите путь к файлу: ");
            var path = Console.ReadLine();
            Console.WriteLine("Введите имя файла для дозаписи");
            var fileName = Console.ReadLine();
            if (File.Exists(path))
            {
                await using var fileStream = File.Open(path, FileMode.Open);
                HttpResponseMessage response = await HttpClient.PostAsync(Url + fileName, new StreamContent(fileStream));
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"Файл {fileName} успешно был обновлен информацией из {path} файла");    
            }
            else
            {
                Console.WriteLine("Файла не существует");
            }
            
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка POST-запроса: {e.Message}");
        }
    }
    
    static async Task SendDeleteRequest()
    {
        try
        {
            Console.WriteLine("Введите имя файла для удаления");
            var fileName = Console.ReadLine();
            HttpResponseMessage response = await HttpClient.DeleteAsync(Url + fileName);
            response.EnsureSuccessStatusCode();
            Console.WriteLine("DELETE-запрос выполнен успешно.");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка DELETE-запроса: {e.Message}");
        }
    }

}
