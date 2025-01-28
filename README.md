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

In the **publish** folder, you can find some OS specific builds. 

You can just put the content of the folder somewhere on your local drive and run the exe. 

I used a free icon from **Jonathan Patterson**, from his Stash icon pack on the [Iconfinder website](https://www.iconfinder.com/iconsets/stashhttps://www.iconfinder.com/iconsets/stash).
