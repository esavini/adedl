using PuppeteerSharp;
using IBrowser = PuppeteerSharp.IBrowser;

namespace AdeDl.App.Services
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

        public IEnumerable<CookieParam> GetCookies() => _cookies.ToArray();

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
}