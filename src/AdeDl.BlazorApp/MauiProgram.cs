using System.Text;
using AdeDl.BlazorApp.Services;
using AdeDl.BlazorApp.Strategies.DownloadStrategy;
using Microsoft.Data.Sqlite;
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

            builder.Services.AddDbContext<AdeDlDbContext>(b =>
            {
                var sqliteConnectionStringBuilder = new SqliteConnectionStringBuilder();
                sqliteConnectionStringBuilder.DataSource = databasePath;
                sqliteConnectionStringBuilder.ForeignKeys = true;
                
                var connectionString = sqliteConnectionStringBuilder.ToString();
                
                b.UseSqlite(connectionString);
                b.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<ICredentialService, CredentialService>();
            builder.Services.AddSingleton<ILoginService, LoginService>();
            builder.Services.AddSingleton<IStateKeeper, StateKeeper>();


            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IBrowserService, BrowserService>();
            builder.Services.AddTransient<ICassettoFiscaleService, CassettoFiscaleService>();
            builder.Services.AddTransient<IF24Service, F24Service>();
            builder.Services.AddTransient<ICuService, CuService>();
            builder.Services.AddTransient<IAnagraficaService, AnagraficaService>();
            builder.Services.AddTransient<ICreditoIvaService, CreditoIvaService>();
            builder.Services.AddTransient<IFileDownloaderService, FileDownloaderService>();

            builder.Services.AddTransient<IDownloadContext, DownloadContext>();
            builder.Services.AddTransient<IDownloadStrategy, F24DownloadStrategy>();
            builder.Services.AddTransient<IDownloadStrategy, CuDownloadStrategy>();
            builder.Services.AddTransient<IDownloadStrategy, AnagraficaStrategy>();
            builder.Services.AddTransient<IDownloadStrategy, CreditoIvaStrategy>();

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
            Log.Fatal(exception, "Thrown exception in the app, closing...");
            Log.CloseAndFlush();
            
            throw;
        }
    }
}