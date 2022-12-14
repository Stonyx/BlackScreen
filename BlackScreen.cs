using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace BlackScreen
{
  // Black Screen Application class
  public partial class BSApplication : System.Windows.Application
  {
    // Main function
    [STAThread]
    public static void Main(string[] args)
    {
      // Declare and define needed variables
      int monitor;

      // Check if a monitor number was not specified and set it to 0 which is interepreted by the
      //   Black Screen Window class constructor as the primary monitor or first monitor
      if (!(args.Length > 0) || !Int32.TryParse(args[0], out monitor))
        monitor = 0;

      // Create the application and window objects
      BSApplication application = new BSApplication();
      BSWindow window = new BSWindow(monitor);

      // Show the window
      window.Show();

      // Add the main window to the application and run it
      application.MainWindow = window;
      application.Run();
    }
  }

  // Black Screen Window class
  public partial class BSWindow : Window
  {
    // Constructor
    public BSWindow(int monitor) : base()
    {
      // Set the window style to none
      WindowStyle = WindowStyle.None;
      WindowStartupLocation = WindowStartupLocation.Manual;

      // Find the specified monitor defaulting to the primary monitor or the first monitor
      Screen? screen = null;
      foreach (Screen i in Screen.AllScreens)
      {
        if (i.DeviceName == @"\\.\DISPLAY" + monitor)
          screen = i;
      }
      if (screen == null)
      {
        screen = Screen.PrimaryScreen ?? Screen.AllScreens[0];
      }

      // Set the left, top, width, and height
      Left = screen.WorkingArea.Left;
      Top = screen.WorkingArea.Top;
      Width = screen.WorkingArea.Width;
      Height = screen.WorkingArea.Height;

      // Set the background to a solid black
      Background = new SolidColorBrush(Colors.Black);

      // Add our loaded event handler
      Loaded += LoadedEventHandler;
    }

    // Loaded event handler
    private void LoadedEventHandler(object sender, RoutedEventArgs e)
    {
      // Maximize the window
      ((Window)sender).WindowState = WindowState.Maximized;
    }
  }
}
