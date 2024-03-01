using System.Net;
using System.Text;
using System.Text.Json;

class Program
{
  public static void Main()
  {
    WordInfo[] words = [];

    HttpListener listener = new();
    listener.Prefixes.Add("http://*:5000/");
    listener.Start();

    Console.WriteLine("Server started. Listening for requests...");
    Console.WriteLine("Main page on http://localhost:5000/website/index.html");

    while (true)
    {
      HttpListenerContext context = listener.GetContext();
      HttpListenerRequest request = context.Request;
      HttpListenerResponse response = context.Response;

      string rawPath = request.RawUrl!;
      string absPath = request.Url!.AbsolutePath;

      Console.WriteLine($"Received a request with path: " + rawPath);

      string filePath = "." + absPath;
      bool isHtml = request.AcceptTypes!.Contains("text/html");

      if (File.Exists(filePath))
      {
        byte[] fileBytes = File.ReadAllBytes(filePath);
        if (isHtml) { response.ContentType = "text/html; charset=utf-8"; }
        response.OutputStream.Write(fileBytes);
      }
      else if (isHtml)
      {
        response.StatusCode = (int)HttpStatusCode.Redirect;
        response.RedirectLocation = "/website/404.html";
      }
      else if (absPath == "/getWords")
      {
        string wordsJson = JsonSerializer.Serialize(words);
        byte[] wordsBytes = Encoding.UTF8.GetBytes(wordsJson);
        response.OutputStream.Write(wordsBytes);
      }
      else if (absPath == "/addWord")
      {
        string newWord = GetBody(request);
        WordInfo newWordInfo = new WordInfo(newWord);
        words = [.. words, newWordInfo];
      }

      response.Close();
    }
  }

  public static string GetBody(HttpListenerRequest request)
  {
    return new StreamReader(request.InputStream).ReadToEnd();
  }
}

class WordInfo
{
  public string Word { get; set; }
  public string Reversed { get; set; }
  public bool IsPalindrome { get; set; }
  public int Length { get; set; }

  public WordInfo(string word)
  {
    Word = word;

    Reversed = "";
    for (int i = 0; i < word.Length; i++)
    {
      Reversed += word[^(i + 1)];
    }

    IsPalindrome = Word == Reversed;

    Length = word.Length;
  }
}