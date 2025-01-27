using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.Input;
using SukiUI;
using SukiUI.Dialogs;
using SukiUI.Toasts;
using TheRegulator.Next.RegexParsing;

namespace TheRegulator.Next.ViewModels;

public class MainViewModel : ViewModelBase
{
    private ISukiDialogManager? _dialogManager;
    private ISukiToastManager? _toastManager;
    private TextBox? _editor;
    private string? _inputText;
    private string? _replaceText;
    private bool _showListResult;
    private string _analyzeResult = string.Empty;

    public TextBox? Editor
    {
        get => _editor;
        set
        {
            SetProperty(ref _editor, value);
            SetupCommands();
        }
    }

    public string? InputText
    {
        get => _inputText;
        set => SetProperty(ref _inputText, value);
    }

    public string? ReplaceText
    {
        get => _replaceText;
        set => SetProperty(ref _replaceText, value);
    }

    public bool ShowListResult
    {
        get => _showListResult;
        private set => SetProperty(ref _showListResult, value);
    }

    public string AnalyzeResult
    {
        get => _analyzeResult;
        set => SetProperty(ref _analyzeResult, value);
    }

    public string TextResult { get; set; } = string.Empty;
    public ObservableCollection<MatchViewModel> ListResult { get; } = [];

    public ICommand SwitchThemeCommand { get; private set; } = new RelayCommand(SwitchTheme);
    public ICommand AddTextCommand { get; private set; }

    public RelayCommand CutCommand { get; private set; }
    public RelayCommand CopyCommand { get; private set; }
    public RelayCommand PasteCommand { get; private set; }
    public RelayCommand SelectAllCommand { get; private set; }
    public RelayCommand DeleteCommand { get; private set; }
    public RelayCommand UndoCommand { get; private set; }
    public RelayCommand RedoCommand { get; private set; }
    
    public RelayCommand MatchCommand { get; private set; }
    public RelayCommand ReplaceCommand { get; private set; }
    public RelayCommand SplitCommand { get; private set; }
    public RelayCommand AnalyzeCommand { get; private set; }

    public void SetDialogHosts(ISukiDialogManager dialogManager, ISukiToastManager toastManager)
    {
        _dialogManager = dialogManager;
        _toastManager = toastManager;
    }

    private static void SwitchTheme()
    {
        SukiTheme.GetInstance().SwitchBaseTheme();
    }

    private void SetupCommands()
    {
        if (Editor is null) return;

        AddTextCommand = new RelayCommand<string>(AddText);
        CutCommand = new RelayCommand(Editor.Cut, () => Editor.CanCut);
        CopyCommand = new RelayCommand(Editor.Copy, () => Editor.CanCopy);
        PasteCommand = new RelayCommand(Editor.Paste, () => Editor.CanPaste);
        UndoCommand = new RelayCommand(Editor.Undo, () => Editor.CanUndo);
        RedoCommand = new RelayCommand(Editor.Redo, () => Editor.CanRedo);
        SelectAllCommand = new RelayCommand(Editor.SelectAll);
        DeleteCommand = new RelayCommand(DeleteText);

        MatchCommand = new RelayCommand(RunMatch);
        ReplaceCommand = new RelayCommand(RunReplace);
        SplitCommand = new RelayCommand(RunSplit);
        AnalyzeCommand = new RelayCommand(Analyze);

        Editor.PropertyChanged += (sender, args) =>
        {
            switch (args.Property.Name)
            {
                case nameof(TextBox.CanCut):
                    CutCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(TextBox.CanCopy):
                    CopyCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(TextBox.CanPaste):
                    PasteCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(TextBox.CanUndo):
                    UndoCommand.NotifyCanExecuteChanged();
                    return;
                case nameof(TextBox.CanRedo):
                    RedoCommand.NotifyCanExecuteChanged();
                    return;
            }
        };
    }

    private void AddText(string toAdd)
    {
        if (string.IsNullOrEmpty(toAdd)) { return; }

        Editor!.Text = string.IsNullOrWhiteSpace(Editor.Text) 
            ? toAdd.TrimStart(' ')
            : Editor.Text.Insert(Editor.CaretIndex, toAdd.TrimStart(' '));

        Editor.CaretIndex += toAdd.Length;
    }

    private void DeleteText()
    {
        Editor!.Text = Editor.Text?.Remove(Editor.SelectionStart, Editor.SelectedText.Length);
        Editor.SelectionEnd = Editor.SelectionStart;
    }

    private void RunMatch()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text))
        {
            ShowToast("Invalid input", "No regular expression input found", NotificationType.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(InputText))
        {
            ShowToast("Invalid input", "No test input found", NotificationType.Error);
            return;
        }

        try
        {
            ListResult.Clear();

            var regex = new Regex(Editor.Text);
            foreach (var capture in regex.Matches(InputText).SelectMany(x => x.Captures))
            {
                ListResult.Add(new MatchViewModel(capture.Value, capture.Index));
            }
            ShowListResult = true;
        }
        catch (Exception e)
        {
            ShowToast("Error occurred", e.Message, NotificationType.Error);
        }
    }
    
    private void RunReplace()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text))
        {
            ShowToast("Invalid input", "No regular expression input found", NotificationType.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(InputText))
        {
            ShowToast("Invalid input", "No test input found", NotificationType.Error);
            return;
        }

        try
        {
            var regex = new Regex(Editor.Text);
            TextResult = regex.Replace(InputText, _replaceText ?? string.Empty);
            OnPropertyChanged(nameof(TextResult));
            ShowListResult = false;
        }
        catch (Exception e)
        {
            ShowToast("Error occurred", e.Message, NotificationType.Error);
        }
    }

    private void RunSplit()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text))
        {
            ShowToast("Invalid input", "No regular expression input found", NotificationType.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(InputText))
        {
            ShowToast("Invalid input", "No test input found", NotificationType.Error);
            return;
        }

        try
        {
            ListResult.Clear();

            var regex = new Regex(Editor.Text);
            var sb = new StringBuilder();
            foreach (var split in regex.Split(InputText))
            {
                sb.AppendLine(split);
            }
            TextResult = sb.ToString();
            OnPropertyChanged(nameof(TextResult));
            ShowListResult = false;
        }
        catch (Exception e)
        {
            ShowToast("Error occurred", e.Message, NotificationType.Error);
        }
    }

    private void Analyze()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text))
        {
            ShowToast("Invalid input", "No regular expression input found", NotificationType.Error);
            return;
        }

        try
        {
            AnalyzeResult = RegexExpression.Interpret(Editor.Text);
        }
        catch (Exception e)
        {
            ShowToast("Error occurred", e.Message, NotificationType.Error);
            AnalyzeResult = string.Empty;
        }
    }
    
    private void ShowToast(string title, string content, NotificationType type)
    {
        _toastManager.CreateToast()
            .Dismiss().After(TimeSpan.FromSeconds(3))
            .Dismiss().ByClicking()
            .OfType(type)
            .WithTitle(title)
            .WithContent(content)
            .Queue();
    }
}