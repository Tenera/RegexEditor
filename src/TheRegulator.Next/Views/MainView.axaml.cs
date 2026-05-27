using Avalonia.Controls;
using TheRegulator.Next.ViewModels;

namespace TheRegulator.Next.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContextChanged += (sender, args) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Editor = Editor;
            }
        };
    }
}
