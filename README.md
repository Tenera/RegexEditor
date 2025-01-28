# RegexEditor

Regex editor to build and test regexes (based on The Regulator)

![About Regulator](https://github.com/Tenera/RegexEditor/blob/main/Images/About_old.png?raw=true)

The Regulator is a .NET 2.0 Winforms application, developed in 2004 by Roy Osherove. 
It is a very feature rich tool to develop and test regular expressions for .NET applications.

Unfortunately it uses ComponentOne and Syncfusion controls, so it is impossible to fork and upgrade to newer versions of .NET without licenses for these component libraries.

This is how the UI looked:

![Regulator UI](https://github.com/Tenera/RegexEditor/blob/main/Images/Screenshot_old.png?raw=true)

Because of these expensive dependencies and to include MacOS and Linux users, I decided to develop a new (simpler) version using [Avalonia](https://docs.avaloniaui.net/) with them [SukiUI]([GitHub - kikipoulet/SukiUI: UI Theme for AvaloniaUI](https://github.com/kikipoulet/SukiUI)).

This is how the UI looks now:

![TheRegulator.Next UI](https://github.com/Tenera/RegexEditor/blob/main/Images/Screenshot.png?raw=true)

In the **publish** folder, you can find some OS specific builds. 

You can just put the content of the folder somewhere on your local drive and run the exe. 
