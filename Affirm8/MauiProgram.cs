using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;

using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using Affirm8.Views.Catalog;
using Affirm8.Data;

namespace Affirm8
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureSyncfusionCore()
                .ConfigureSyncfusionToolkit()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMaterialComponents()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Medium.ttf", "Roboto-Medium");
                    fonts.AddFont("Roboto-Regular.ttf", "Roboto-Regular");
                    fonts.AddFont("UIFontIcons.ttf", "FontIcons");
                });
			//Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
			Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf0x3TXxbf1x1ZFRMY15bR3NPIiBoS35Rc0ViWH5fc3dXRWVZU0dz");

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
