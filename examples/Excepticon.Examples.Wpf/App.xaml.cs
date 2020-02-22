using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Excepticon.Examples.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
public partial class App : Application
{
    private IDisposable _excepticonSdk;

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        _excepticonSdk = ExcepticonSdk.Init("{Your ApiKey Here}");
        MainWindow wnd = new MainWindow();
        wnd.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _excepticonSdk.Dispose();

        base.OnExit(e);
    }
}
}
