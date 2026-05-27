using Avalonia;
using Avalonia.Headless;
using Avalonia.Themes.Fluent;
using TheRegulator.Next.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace TheRegulator.Next.Tests;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<TestApp>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}

public class TestApp : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }
}
