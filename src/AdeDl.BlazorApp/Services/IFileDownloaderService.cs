namespace AdeDl.BlazorApp.Services;

public interface IFileDownloaderService
{
    Task DownloadFileAsync(string url, string path, IBrowserService browserService);
}