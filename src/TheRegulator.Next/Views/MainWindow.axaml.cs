using SukiUI.Controls;
using SukiUI.Dialogs;
using SukiUI.Toasts;

namespace TheRegulator.Next.Views;

public partial class MainWindow : SukiWindow
{
    public static ISukiDialogManager DialogManager { get; } = new SukiDialogManager();
    public static ISukiToastManager ToastManager { get; } = new SukiToastManager();

    public MainWindow()
    {
        InitializeComponent();
        DialogHost.Manager = DialogManager;
        ToastHost.Manager = ToastManager;
    }
}
