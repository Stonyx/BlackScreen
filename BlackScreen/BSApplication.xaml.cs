using System;
using System.Collections.Generic;
using System.Windows;

namespace BlackScreen
{
  // Black Screen Application class
  public partial class BSApplication : Application
  {
    // Startup event handler
    protected void StartupEventHandler(object sender, StartupEventArgs e)
    {
      // Declare needed variables
      List<Int16> screens = new List<Int16>();

      // Loop throught the startup arguments
      foreach (string arg in e.Args)
      {
        // Parse the argument and if successful add it to the list of screens
        Int16 screen;
        if (Int16.TryParse(arg, out screen))
          screens.Add(screen);
      }

      // Check if no screens were specified or successfully parsed and add a default screen to the list of screens
      if (screens.Count == 0)
        screens.Add(0);

      // Loop through the list of screens
      foreach (Int16 screen in screens)
      {
        // Create and show a window on the specified screen
        BSWindow window = new BSWindow(screen);
        window.Show();
      }
    }
  }
}