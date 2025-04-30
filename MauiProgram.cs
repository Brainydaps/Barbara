using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System.Diagnostics;
using Barbara.Services;
using Barbara.ViewModels;
using Barbara.Views;

namespace Barbara
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Debug.WriteLine("[MauiProgram] CreateMauiApp start");
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
                   .ConfigureFonts(fonts =>
                   {
                       Debug.WriteLine("[MauiProgram] Configuring fonts");
                       fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                   });

            Debug.WriteLine("[MauiProgram] Registering services...");
            // BotService picks the right Azure AI Agent based on the active Tab
            builder.Services.AddSingleton<BotService>();

            // ViewModels
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<AnalystViewModel>();
            builder.Services.AddTransient<JournalHistorianViewModel>();
            builder.Services.AddTransient<PsychologistViewModel>();

            // Pages
            builder.Services.AddTransient<AnalystPage>();
            builder.Services.AddTransient<JournalHistorianPage>();
            builder.Services.AddTransient<PsychologistPage>();

            // Shell
            builder.Services.AddSingleton<AppShell>();

            Debug.WriteLine("[MauiProgram] Building app...");
            var app = builder.Build();
            Debug.WriteLine("[MauiProgram] App built");

            return app;
        }
    }
}