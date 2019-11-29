using IdentityModel.Client;
using IdentityModel.OidcClient;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NativeConsolePKCEClient
{
    public class Program
    {
        static string _authority = "https://localhost:44364";
        static string _api = "https://localhost:44342";

        static OidcClient _oidcClient;
        static HttpClient _apiClient = new HttpClient { BaseAddress = new Uri(_api) };

        public static void Main(string[] args) => RunAsync().GetAwaiter().GetResult();

        public static async Task RunAsync()
        {
            await Login();
        }

        private static async Task Login()
        {
            var browser = new SystemBrowser(45656);
            string redirectUri = "https://127.0.0.1:45656";

            var options = new OidcClientOptions
            {
                Authority = _authority,
                ClientId = "native.code",
                RedirectUri = redirectUri,
                Scope = "openid profile native_api",
                FilterClaims = false,
                Browser = browser,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                LoadProfile = true
            };

            var serilog = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.LiterateConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}{Exception}{NewLine}")
                .CreateLogger();

            options.LoggerFactory.AddSerilog(serilog);

            _oidcClient = new OidcClient(options); 
             var result = await _oidcClient.LoginAsync(new LoginRequest());

            ShowResult(result);
            await NextSteps(result);
        }

        private static void ShowResult(LoginResult result)
        {
            if (result.IsError)
            {
                Console.WriteLine("\n\nError:\n{0}", result.Error);
                return;
            }

            Console.WriteLine("\n\nClaims:");
            foreach (var claim in result.User.Claims)
            {
                Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
            }

            Console.WriteLine($"\nidentity token: {result.IdentityToken}");
            Console.WriteLine($"access token:   {result.AccessToken}");
            Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
        }

        private static async Task NextSteps(LoginResult result)
        {
            var currentAccessToken = result.AccessToken;
            var currentRefreshToken = result.RefreshToken;

            var menu = " x:exit \n b:call api All \n c:call api with route \n d:post api with body \n e:call api with query parameter";
            if (currentRefreshToken != null)
            {
                menu += "r:refresh token";
            }

            await CallApi(currentAccessToken);

            while (true)
            {
                Console.Write(menu);
                Console.Write("\n");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.X) return;
                if (key.Key == ConsoleKey.B) await CallApi(currentAccessToken);
                if (key.Key == ConsoleKey.C) await CallApiwithRouteValue(currentAccessToken, "phil");
                if (key.Key == ConsoleKey.D) await CallApiwithBodyValue(currentAccessToken, "mike");
                if (key.Key == ConsoleKey.E) await CallApiwithQueryStringParam(currentAccessToken, "orange");
                if (key.Key == ConsoleKey.R)
                {
                    var refreshResult = await _oidcClient.RefreshTokenAsync(currentRefreshToken);
                    if (result.IsError)
                    {
                        Console.WriteLine($"Error: {refreshResult.Error}");
                    }
                    else
                    {
                        currentRefreshToken = refreshResult.RefreshToken;
                        currentAccessToken = refreshResult.AccessToken;

                        Console.WriteLine($"access token:   {result.AccessToken}");
                        Console.WriteLine($"refresh token:  {result?.RefreshToken ?? "none"}");
                    }
                }
            }
        }

        private static async Task CallApi(string currentAccessToken)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync("/api/values");

            if (response.IsSuccessStatusCode)
            {
                var json = JArray.Parse(await response.Content.ReadAsStringAsync());
                Console.WriteLine("\n" + json);
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task CallApiwithBodyValue(string currentAccessToken, string user)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.PostAsJsonAsync(
                "/api/values", 
                new BodyData { User = user }
            );

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }
        private static async Task CallApiwithRouteValue(string currentAccessToken, string user)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync($"/api/values/{user}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }

        private static async Task CallApiwithQueryStringParam(string currentAccessToken, string fruit)
        {
            _apiClient.SetBearerToken(currentAccessToken);
            var response = await _apiClient.GetAsync($"/api/values/q?fruit={fruit}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine( $"\n{result}");
            }
            else
            {
                Console.WriteLine($"Error: {response.ReasonPhrase}");
            }
        }
    }

    public class BodyData
    {
        public string User { get; set; }
    }
}