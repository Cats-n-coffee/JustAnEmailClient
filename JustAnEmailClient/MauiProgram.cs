using Microsoft.Extensions.Logging;
using JustAnEmailClient.Controls;
using JustAnEmailClient.Handlers;
using JustAnEmailClient.Views;
using JustAnEmailClient.ViewModels;

namespace JustAnEmailClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(EntryView), typeof(EntryViewHandler));
                    handlers.AddHandler(typeof(EditorView), typeof(EditorViewHandler));
                });

            builder.Services.AddSingleton<MainPage>();
            // Check what this pattern is used for
            /*builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginPageViewModel>();

            builder.Services.AddSingleton<EmailClientPage>();
            builder.Services.AddSingleton<EmailClientViewModel>();

            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<LoadingPageViewModel>();*/

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}