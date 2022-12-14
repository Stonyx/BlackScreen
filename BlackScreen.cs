using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

[assembly: AssemblyCompany("Stonyx")]
[assembly: AssemblyCopyright("Copyright © 2022")]
[assembly: AssemblyTitle("Black Screen")]
[assembly: AssemblyVersion("1.0.0")]

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
      List<string> devices = new List<string>();

      // Loop throught the arguments
      foreach (string arg in args)
      {
        // Parse the argument and add to the list of devices if argument is successfully parsed
        int screen;
        if (Int32.TryParse(arg, out screen))
          devices.Add(@"\\.\DISPLAY" + screen);
      }

      // Check if no screens were specified or successfully parsed and add the primary screen
      //   device to the list of devices
      if (devices.Count == 0)
        devices.Add(Screen.PrimaryScreen.DeviceName);

      // Create the application
      BSApplication application = new BSApplication();

      // Loop through the list of devices
      foreach (string device in devices)
      {
        // Create and show a window
        BSWindow window = new BSWindow(device);
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
    public BSWindow(string device) : base()
    {
      // Set the window title, style, and startup location
      Title = "Black Screen";
      WindowStyle = WindowStyle.None;
      WindowStartupLocation = WindowStartupLocation.Manual;

      // Find the specified device screen defaulting to the primary screen
      Screen screen = Screen.PrimaryScreen;
      foreach (Screen i in Screen.AllScreens)
      {
        if (i.DeviceName == device)
          screen = i;
      }

      // Set the left, top, width, and height
      Left = screen.WorkingArea.Left;
      Top = screen.WorkingArea.Top;
      Width = screen.WorkingArea.Width;
      Height = screen.WorkingArea.Height;

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
