using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TheRegulator.Next.ViewModels;
using TheRegulator.Next.Views;

namespace TheRegulator.Next;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            var vm = new MainViewModel();
            vm.SetDialogHosts(mainWindow.DialogHost.Manager, mainWindow.ToastHost.Manager);
            mainWindow.DataContext = vm;
            desktop.MainWindow = mainWindow;
        }
        base.OnFrameworkInitializationCompleted();
    }
}
