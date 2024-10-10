using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

[assembly: AssemblyCompany("Stonyx")]
[assembly: AssemblyCopyright("Copyright © 2022")]
[assembly: AssemblyTitle("Black Screen")]
[assembly: AssemblyVersion("1.0.2")]

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
    // Import Windows API functions from the user32.dll
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern long GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern long SetWindowLongPtr32(IntPtr hWnd, int nIndex, long dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    // Helper method to call GetWindowLongPtr correctly on 32 bit systems
    public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
      if (IntPtr.Size == 8)
        return GetWindowLongPtr64(hWnd, nIndex);
      else
        return new IntPtr(GetWindowLongPtr32(hWnd, nIndex));
    }

    // Helper method to call SetWindowLongPtr correctly on 32 bit systems
    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
      if (IntPtr.Size == 8)
        return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
      else
        return new IntPtr(SetWindowLongPtr32(hWnd, nIndex, dwNewLong.ToInt32()));
    }

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
      Left = targetScreen.Bounds.Left;
      Top = targetScreen.Bounds.Top;
      Width = targetScreen.Bounds.Width;
      Height = targetScreen.Bounds.Height;

      // Set the background to a solid black
      Background = new SolidColorBrush(Colors.Black);

      // Add the loaded and closed event handlers
      Loaded += LoadedEventHandler;
      Closed += ClosedEventHandler;
    }

    // Loaded event handler
    private void LoadedEventHandler(object sender, EventArgs args)
    {
      // Get the window handle
      IntPtr handle = new WindowInteropHelper(this).Handle;

      // Get the existing window styles
      IntPtr styles = GetWindowLongPtr(handle, /* GWL_STYLE */ -16);

      // Remove the Thick Frame window style
      SetWindowLongPtr(handle, /* GWL_STYLE */ -16, (IntPtr)((long)styles ^ /* WS_THICKFRAME */ 0x00040000L));
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