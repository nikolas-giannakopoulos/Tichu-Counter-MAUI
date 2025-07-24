using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using The49.Maui.BottomSheet;
namespace Tichu_Counter
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBottomSheet()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("TEST.ttf", "test");
                    fonts.AddFont("feather_bold.ttf", "featherBold");
                    fonts.AddFont("Inter-Bold.ttf", "InterBold");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
            #if ANDROID
                if (handler.PlatformView.Background is Android.Graphics.Drawables.RippleDrawable ripple)
                {
                    ripple.SetColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
                };
            #endif
            });
            Microsoft.Maui.Handlers.ImageButtonHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if ANDROID
                if (handler.PlatformView.Background is Android.Graphics.Drawables.RippleDrawable ripple)
                {
                    ripple.SetColor(Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
                };
#endif
            });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

    }
}
