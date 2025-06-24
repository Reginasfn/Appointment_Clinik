using System.Configuration;
using System.Data;
using System.Windows;
using System.Globalization;
using System.Threading;

namespace Main_project
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Установка русской культуры
            var culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Инициализация Material Design
            MaterialDesignThemes.Wpf.ColorZoneAssist.SetMode(new DependencyObject(), MaterialDesignThemes.Wpf.ColorZoneMode.Light);

            base.OnStartup(e);
        }
    }
}
