using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

[assembly: AssemblyCompany("Stonyx")]
[assembly: AssemblyCopyright("Copyright © 2022")]
[assembly: AssemblyTitle("Black Screen")]
[assembly: AssemblyVersion("1.0.1")]

namespace BlackScreen
{
  // Black Screen Application class
  public partial class BSApplication : System.Windows.Application
  {
    // Main function
    [STAThread]
    public static void Main(string[] args)
    {
      // Declare needed variables
      List<int> screens = new List<int>();

      // Loop throught the arguments
      foreach (string arg in args)
      {
        // Parse the argument and add to the list of screens if argument is successfully parsed
        int screen;
        if (Int32.TryParse(arg, out screen))
          screens.Add(screen);
      }

      // Check if no screens were specified or successfully parsed and add a "default" screen to
      //   the list of screens
      if (screens.Count == 0)
        screens.Add(0);

      // Create the application
      BSApplication application = new BSApplication();

      // Loop through the list of screens
      foreach (int screen in screens)
      {
        // Create and show a window on the current screen
        BSWindow window = new BSWindow(screen);
        window.Show();
      }

      // Run the application
      application.Run();
    }
  }

  // Black Screen Window class
  public partial class BSWindow : Window
  {
    // Constructor
    public BSWindow(int screenNumber) : base()
    {
      // Set the window title, style, and startup location
      Title = "Black Screen";
      WindowStyle = WindowStyle.None;
      WindowStartupLocation = WindowStartupLocation.Manual;

      // Find the specified screen defaulting to the primary screen
      Screen targetScreen = Screen.PrimaryScreen;
      foreach (Screen screen in Screen.AllScreens)
      {
        if (screen.DeviceName == @"\\.\DISPLAY" + screenNumber)
          targetScreen = screen;
      }

      // Set the left, top, width, and height
      Left = targetScreen.WorkingArea.Left;
      Top = targetScreen.WorkingArea.Top;
      Width = targetScreen.WorkingArea.Width;
      Height = targetScreen.WorkingArea.Height;

      // Set the background to a solid black
      Background = new SolidColorBrush(Colors.Black);

      // Add the loaded and closed event handlers
      Loaded += LoadedEventHandler;
      Closed += ClosedEventHandler;
    }

    // Loaded event handler
    private void LoadedEventHandler(object sender, EventArgs args)
    {
      // Maximize the window
      ((Window)sender).WindowState = WindowState.Maximized;
    }

    // Closed event handler
    private void ClosedEventHandler(object sender, EventArgs args)
    {
      // Close all other application windows
      foreach (Window window in System.Windows.Application.Current.Windows)
      {
        window.Close();
      }
    }
  }
}
