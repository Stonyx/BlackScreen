using System;
using System.Windows;
using WpfScreenHelper;

namespace BlackScreen
{
  // Black Screen Window class
  public partial class BSWindow : Window
  {
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
    private void LoadedEventHandler(object sender, EventArgs args)
    {
      // Maximize the window
      ((Window)sender).WindowState = WindowState.Maximized;
    }

    // Closing event handler
    private void ClosingEventHandler(object sender, EventArgs args)
    {
      // Close all other application windows
      foreach (Window window in Application.Current.Windows)
      {
        if (window != ((Window)sender))
          window.Close();
      }
    }
  }
}