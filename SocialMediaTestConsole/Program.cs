using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SocialMediaTestConsole
{
    internal class Program
    {

        public static HttpClient _client;
        public static string _baseUrl = "https://localhost:7195/api/";
        public static string _token = "";

        static void Main(string[] args)
        {
            _client = new HttpClient();
            while (true)
            {
                
                if (String.IsNullOrEmpty(_token))
                {
                    Login();
                }
                else
                {
                    Console.WriteLine("You are logged in!");
                    Console.WriteLine("1. Post");
                    Console.ReadLine();
                }
            }
        }

        public static void Login()
        {
            Console.Write("Email: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string password = null;
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
            }

            Console.WriteLine();
            Console.WriteLine();

            var payload = new
            {
                Email = user,
                Password = password
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _client.PostAsync(_baseUrl + "login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Erfolgreich gesendet!");
            }
            else
            {
                Console.WriteLine($"Fehler: {response.StatusCode}");
                return;
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);
            _token = responseJson.GetProperty("token").GetString();

        } 
    }
}
