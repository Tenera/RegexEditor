using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace TheRegulator.Next.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string _regex = string.Empty;
    private int _editorCaretIndex;

    public string Regex
    {
        get => _regex;
        set => SetProperty(ref _regex, value);
    }

    public int EditorCaretIndex
    {
        get => _editorCaretIndex;
        set => SetProperty(ref _editorCaretIndex, value);
    }

    public ICommand AddTextCommand { get; set; }

    public MainViewModel()
    {
        AddTextCommand = new RelayCommand<string>(AddText);
    }

    private void AddText(string toAdd)
    {
        if (string.IsNullOrEmpty(toAdd)) { return; }

        Regex = string.IsNullOrWhiteSpace(Regex) 
            ? toAdd.TrimStart(' ')
            : Regex.Insert(EditorCaretIndex, toAdd.TrimStart(' '));
    }
}

