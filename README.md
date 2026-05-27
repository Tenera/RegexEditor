# RegexEditor

Regex editor to build and test regexes, based on an old project called **The Regulator**.

<img src="https://github.com/Tenera/RegexEditor/blob/main/Images/About_old.png?raw=true" title="" alt="About Regulator" width="391">

The Regulator is a .NET 2.0 Winforms application, developed in 2004 by **Roy Osherove**. 
It is a very feature rich tool to develop and test regular expressions for .NET applications.

Unfortunately it uses **ComponentOne** and **Syncfusion** controls, so it is impossible to fork and upgrade to newer versions of .NET without licenses for these component libraries.

This is how the UI looked:

![Regulator UI](https://github.com/Tenera/RegexEditor/blob/main/Images/Screenshot_old.png?raw=true)

Because of these expensive dependencies, and to include **MacOS** and **Linux** users, I decided to develop a new (and simpler) version using [Avalonia](https://docs.avaloniaui.net/) and the [SukiUI](https://github.com/kikipoulet/SukiUI) theme.

This is how the UI looks now in light mode:

![TheRegulator.Next UI](https://github.com/Tenera/RegexEditor/blob/main/Images/Screenshot.png?raw=true)

## Download

Pre-built binaries for Windows, Linux, and macOS are available on the [Releases](https://github.com/Tenera/RegexEditor/releases) page. Download the zip for your platform, extract it, and run the executable.

## Build from source

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet build TheRegulator.sln
dotnet run --project src/TheRegulator.Next
```

## Run tests

```bash
dotnet test src/TheRegulator.Next.Tests
```

## Project structure

```
src/
  TheRegulator.Next/         # Main application (Avalonia 12 + SukiUI)
  TheRegulator.Next.Tests/   # Unit tests (NUnit)
```

For the application icon I used a free icon from **Jonathan Patterson**, from his Stash icon pack on the [Iconfinder website](https://www.iconfinder.com/iconsets/stash).
