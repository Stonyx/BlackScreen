using System;
using System.Linq;
using System.Windows;
using WpfScreenHelper;

namespace BlackScreen
{
  // Black Screen Window class
  public partial class BSWindow : Window
  {
    // Member variables
    protected bool IsClosing = false;

    // Constructor
    public BSWindow(Int16 screenNumber) : base()
    {
      // Load the XAML window
      InitializeComponent();

      // Set the window startup location
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
    }

    // Loaded event handler
    protected void LoadedEventHandler(object sender, EventArgs args)
    {
      // Maximize the window
      // Note: we maximize the window to display it correctly when DPI scaling is involved and we do so in the loaded
      //   event handler to allow the window to show on the specified screen before being maximized
      WindowState = WindowState.Maximized;
    }

    // Closing event handler
    protected void ClosingEventHandler(object sender, EventArgs args)
    {
      // Set the closing flag
      IsClosing = true;

      // Close all application windows that aren't already closing
      foreach (BSWindow window in Application.Current.Windows.OfType<BSWindow>())
      {
        if (!window.IsClosing)
          window.Close();
      }
    }
  }
}