using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace TheRegulator.Next.ViewModels;

public class MainViewModel : ViewModelBase
{
    private TextBox? _editor;
    private string? _inputText;
    private string? _replaceText;

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

    public string ReplaceResultText { get; set; } = string.Empty;

    public ObservableCollection<MatchViewModel> Matches { get; set; } = [];

    public ICommand AddTextCommand { get; set; }

    public RelayCommand CutCommand { get; set; }
    public RelayCommand CopyCommand { get; set; }
    public RelayCommand PasteCommand { get; set; }
    public RelayCommand SelectAllCommand { get; set; }
    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand UndoCommand { get; set; }
    public RelayCommand RedoCommand { get; set; }
    
    public RelayCommand MatchCommand { get; set; }
    public RelayCommand ReplaceCommand { get; set; }

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
    }

    private void DeleteText()
    {
        Editor!.Text = Editor.Text?.Remove(Editor.SelectionStart, Editor.SelectedText.Length);
        Editor.SelectionEnd = Editor.SelectionStart;
    }

    private void RunMatch()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text) || string.IsNullOrWhiteSpace(InputText)) return;

        try
        {
            Matches.Clear();

            var r = new Regex(Editor.Text);
            foreach (var capture in r.Matches(InputText).SelectMany(x => x.Captures))
            {
                Matches.Add(new MatchViewModel(capture.Value, capture.Index));
            }
        }
        catch (Exception e)
        {
            //SetErrorText(e.Message);
        }
    }
    
    private void RunReplace()
    {
        if (string.IsNullOrWhiteSpace(Editor.Text) || string.IsNullOrWhiteSpace(InputText)) return;

        try
        {
            Matches.Clear();

            var r = new Regex(Editor.Text);
            ReplaceResultText = r.Replace(InputText, _replaceText ?? string.Empty);
            OnPropertyChanged(nameof(ReplaceResultText));
        }
        catch (Exception e)
        {
            //SetErrorText(e.Message);
        }
    }
}