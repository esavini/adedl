namespace AdeDl.BlazorApp.Services;

public class FileDownloaderService : IFileDownloaderService
{
    public async Task DownloadFileAsync(string url, string path, IBrowserService browserService)
    {
        var newCookies = await browserService.GetCookiesAsync();

        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
        requestMessage.Headers.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
        requestMessage.Headers.Add("Cookie", string.Join("; ", newCookies.Select(c => c.Name + "=" + c.Value)));
        requestMessage.Headers.Add("Accept", "*/*");

        var httpClient = new HttpClient();

        var httpResult = await httpClient.SendAsync(requestMessage);

        if (!httpResult.IsSuccessStatusCode)
        {
            return;
        }

        await using var resultStream = await httpResult.Content.ReadAsStreamAsync();

        if (File.Exists(path)) File.Delete(path);

        await using var fileStream = File.Create(path);
        await resultStream.CopyToAsync(fileStream);
        fileStream.Close();
    } 
}