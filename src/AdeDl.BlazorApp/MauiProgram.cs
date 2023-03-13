using System.Text;
using AdeDl.BlazorApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AdeDl.BlazorApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var appDataPath = Path.Combine(FileSystem.AppDataDirectory, "AdeDl");

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        var databasePath = Path.Combine(appDataPath, "database.db");
        var logsPath = Path.Combine(appDataPath, "logs");

        if (!Directory.Exists(logsPath))
        {
            Directory.CreateDirectory(logsPath);
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.File(
                Path.Combine(logsPath, "log_.log"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 52_428_800,
                retainedFileTimeLimit: TimeSpan.FromDays(15),
                encoding: Encoding.UTF8
            )
            .CreateLogger();
        
        try
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

            builder.Services.AddDbContext<AdeDlDbContext>(b => b.UseSqlite($"DataSource={databasePath}"));

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddScoped<ICredentialService, CredentialService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AdeDlDbContext>();
            dbContext.Database.Migrate();

            return app;
        }
        catch (Exception exception)
        {
            Log.Fatal("Thrown exception {@Exception} in the app, closing...", exception);
            Log.CloseAndFlush();
            
            throw;
        }
    }
}