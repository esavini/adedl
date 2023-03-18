using System.Text.Json.Serialization;
using PuppeteerSharp;
using IBrowser = PuppeteerSharp.IBrowser;

namespace AdeDl.BlazorApp.Services
{
    public class BrowserService : IBrowserService
    {
        private IBrowser _browser;

        private IEnumerable<CookieParam> _cookies;

        public async Task CreateClientAsync(bool visible, Func<IEnumerable<CookieParam>, bool>? doneFunc = null,
            Action? callback = null)
        {
            string? path = null;

            if (File.Exists(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"))
            {
                path = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            }
            else if (File.Exists(@"C:\Program Files\Google\Chrome\Application\chrome.exe"))
            {
                path = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            }

            var launchOptions = new LaunchOptions
            {
                ExecutablePath = path,
                Headless = !visible,
                Args = new[] { "--no-sandbox" },
                DefaultViewport = null,
            };

            if (visible)
            {
                launchOptions.IgnoredDefaultArgs = new[]
                    { "--enable-automation", "--enable-blink-features=IdleDetection" };
                launchOptions.Args = new[] { "--start-maximized", "--app=https://www.agenziaentrate.gov.it" };
            }

            _browser = await Puppeteer.LaunchAsync(launchOptions);
            var pages = await _browser.PagesAsync();
        }

        public async Task<IEnumerable<CookieParam>> GetCookiesAsync()
        {
        var pages = await _browser.PagesAsync();
            var firstPage = pages.FirstOrDefault() ?? await _browser.NewPageAsync();
            
           var a =await  firstPage.Client.SendAsync<CookiesContainer>("Network.getAllCookies");

           return a.Cookies.Select(x => new CookieParam
            {
                Name = x.Name,
                Value = x.Value,
                Domain = x.Domain,
                Path = x.Path,
                Expires = x.Expires,
                HttpOnly = x.HttpOnly,
                Secure = x.Secure,
            });
        }

        public async Task GoToAsync(string url, IEnumerable<CookieParam>? cookies = null)
        {
            var pages = await _browser.PagesAsync();
            var firstPage = pages.FirstOrDefault() ?? await _browser.NewPageAsync();

            if (cookies is not null)
            {
                await firstPage.SetCookieAsync(cookies.ToArray());
            }

            await firstPage.GoToAsync(url);
        }

        public async Task<T> ActAsync<T>(string code)
        {
            var firstPage = (await _browser.PagesAsync()).First();
            return await firstPage.EvaluateExpressionAsync<T>(code);
        }

        public async Task ActAsync(string code)
        {
            var firstPage = (await _browser.PagesAsync()).First();
            await firstPage.EvaluateExpressionAsync(code);
        }

        public Task Close()
        {
            return _browser.CloseAsync();
        }

        public async Task PdfAsync(string pathRiepilogo)
        {
            var firstPage = (await _browser.PagesAsync()).First();
            await firstPage.PdfAsync(pathRiepilogo);
        }

        public async Task<string> GetCurrentAddress()
        {
            var firstPage = (await _browser.PagesAsync()).First();
            return firstPage.MainFrame.Url;
        }
    }

    public class CookiesContainer
    {
        [JsonPropertyName("cookies")]
        public IEnumerable<Cookie> Cookies { get; set; }

        public class Cookie
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }
            
            [JsonPropertyName("value")]
            public string Value { get; set; }
            
            [JsonPropertyName("domain")]
            public string Domain { get; set; }
            
            [JsonPropertyName("path")]
            public string Path { get; set; }
            
            [JsonPropertyName("expires")]
            public int Expires { get; set; }
            
            [JsonPropertyName("size")]
            public int Size { get; set; }
            
            [JsonPropertyName("httpOnly")]
            public bool HttpOnly { get; set; }
            
            [JsonPropertyName("secure")]
            public bool Secure { get; set; }
            
            [JsonPropertyName("session")]
            public bool Session { get; set; }
            
            [JsonPropertyName("priority")]
            public string Priority { get; set; }
            
            [JsonPropertyName("sameParty")]
            public bool SameParty { get; set; }
            
            [JsonPropertyName("sourceScheme")]
            public string SourceScheme { get; set; }
            
            [JsonPropertyName("sourcePort")]
            public int SourcePort { get; set; }
        }
    }
}