# Black Screen

Simple C# code only W.P.F. .NET 4.8 application that turns one or more screens black by displaying a full screen borderless black window.  To use, simply run the application to turn the primary screen black or specify which screen(s) to turn black via command line arguments.

For example, run `BlackScreen.exe 2` to turn screen 2 black.  Or run `BlackScreen.exe 1 2` to turn both screen 1 and 2 black.  Please note that due to a .NET "feature" (it's not a bug, it's a feature) the screen numbers might not correspond to the screen numbers found in the Windows Control Panel or Settings.

To close the application, press Alt+F4 like you would with any other Windows application.