using Avalonia.Headless.XUnit;
using FluentAssertions;
using TheRegulator.Next.ViewModels;
using Xunit;

namespace TheRegulator.Next.Tests.ViewModels;

public class MainViewModelTests
{
    [AvaloniaFact]
    public void DefaultOptions_IgnoreCaseIsTrue()
    {
        var vm = new MainViewModel();
        vm.IgnoreCase.Should().BeTrue();
    }

    [AvaloniaFact]
    public void DefaultOptions_MultiLineIsTrue()
    {
        var vm = new MainViewModel();
        vm.MultiLine.Should().BeTrue();
    }

    [AvaloniaFact]
    public void DefaultOptions_IgnoreWhitespaceIsTrue()
    {
        var vm = new MainViewModel();
        vm.IgnoreWhitespace.Should().BeTrue();
    }

    [AvaloniaFact]
    public void DefaultOptions_SingleLineIsFalse()
    {
        var vm = new MainViewModel();
        vm.SingleLine.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultOptions_RightToLeftIsFalse()
    {
        var vm = new MainViewModel();
        vm.RightToLeft.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultOptions_ExplicitCaptureIsFalse()
    {
        var vm = new MainViewModel();
        vm.ExplicitCapture.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultOptions_EcmaScriptIsFalse()
    {
        var vm = new MainViewModel();
        vm.EcmaScript.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultOptions_CultureInvariantIsFalse()
    {
        var vm = new MainViewModel();
        vm.CultureInvariant.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultOptions_NonBacktrackingIsFalse()
    {
        var vm = new MainViewModel();
        vm.NonBacktracking.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultState_ListResultIsEmpty()
    {
        var vm = new MainViewModel();
        vm.ListResult.Should().BeEmpty();
    }

    [AvaloniaFact]
    public void DefaultState_ShowListResultIsFalse()
    {
        var vm = new MainViewModel();
        vm.ShowListResult.Should().BeFalse();
    }

    [AvaloniaFact]
    public void DefaultState_AnalyzeResultIsEmpty()
    {
        var vm = new MainViewModel();
        vm.AnalyzeResult.Should().BeEmpty();
    }

    [AvaloniaFact]
    public void SetProperty_InputTextRaisesPropertyChanged()
    {
        var vm = new MainViewModel();
        var raised = false;
        vm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainViewModel.InputText))
                raised = true;
        };

        vm.InputText = "test";

        raised.Should().BeTrue();
    }

    [AvaloniaFact]
    public void SetProperty_AnalyzeResultRaisesPropertyChanged()
    {
        var vm = new MainViewModel();
        var raised = false;
        vm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MainViewModel.AnalyzeResult))
                raised = true;
        };

        vm.AnalyzeResult = "result";

        raised.Should().BeTrue();
    }
}
