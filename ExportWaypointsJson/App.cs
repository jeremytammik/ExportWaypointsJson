#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System.IO;
#endregion

namespace ExportWaypointsJson
{
  class App : IExternalApplication
  {
    SplitButton sbFarClip;

    /// <summary>
    /// this external application class instance.
    /// </summary>
    internal static App _app = null;

    /// <summary>
    /// Provide access to this class instance.
    /// </summary>
    public static App Instance
    {
      get { return _app; }
    }

    void CreateRibbonTab(
      UIControlledApplication a )
    {
      Assembly assembly = Assembly.GetExecutingAssembly();

      string ass_path = assembly.Location;
      string ass_name = assembly.GetName().Name;

      // create ribbon tab 

      string tab_name = "Export Waypoints";

      try
      {
        a.CreateRibbonTab( tab_name );
      }
      catch( Autodesk.Revit.Exceptions.ArgumentException )
      {
        // Assume error generated is due to tab already existing
      }

      PushButtonData pbSecAdjust = new PushButtonData(
        "FarSideClip", "Far Clip", ass_path,
        ass_name + ".SectionFarClipReset" );

      PushButtonData pbSecAdjustOpt = new PushButtonData(
        "FarSideClipOpt", "Settings", ass_path,
        ass_name + ".SectionFarClipResetOptions" );

      pbSecAdjust.LargeImage = NewBitmapImage( assembly,
        "SplitButtonOptionConcept.FarClip.png" );

      pbSecAdjustOpt.LargeImage = NewBitmapImage( assembly,
        "SplitButtonOptionConcept.FarClipSetting.png" );

      // add button tips (when data, must be defined prior to adding button.)

      pbSecAdjust.ToolTip = "Reset a section's far clipping "
        + "boundary to a close distance.";

      pbSecAdjust.LongDescription = "Start this command and "
        + "pick a section line. The far clipping distance "
        + "will be reset to be close to the line.";

      //   Add new ribbon panel. 

      string panel_name = "Export Waypoints";

      RibbonPanel thisNewRibbonPanel = a.CreateRibbonPanel(
        tab_name, panel_name );

      // add button to ribbon panel

      SplitButtonData sbFarClipData = new SplitButtonData(
        "splitFarClip", "FarClip" );

      sbFarClip = thisNewRibbonPanel.AddItem( sbFarClipData )
        as SplitButton;

      sbFarClip.AddPushButton( pbSecAdjust );
      sbFarClip.AddPushButton( pbSecAdjustOpt );
    }

    /// <summary>
    /// Load a new icon bitmap from embedded resources.
    /// For the BitmapImage, make sure you reference WindowsBase
    /// and PresentationCore, and import the System.Windows.Media.Imaging namespace. 
    /// </summary>
    BitmapImage NewBitmapImage(
      System.Reflection.Assembly a,
      string imageName )
    {
      Stream s = a.GetManifestResourceStream( imageName );
      BitmapImage img = new BitmapImage();
      img.BeginInit();
      img.StreamSource = s;
      img.EndInit();
      return img;
    }

    public void SetSplitButtonToThisOrTop(
      string _bName,
      SplitButton _splitButton )
    {
      IList<PushButton> sbList = _splitButton.GetItems();
      foreach( PushButton pb in sbList )
      {
        if( pb.Name.Equals( _bName ) )
        {
          _splitButton.CurrentButton = pb;
          return;
        }
      }
      _splitButton.CurrentButton = sbList[0];
    }

    public void SetSplitButtonFarClipToTop()
    {
      IList<PushButton> sbList = sbFarClip.GetItems();
      sbFarClip.CurrentButton = sbList[0];
    }


    public Result OnStartup( UIControlledApplication a )
    {
      _app = this;
      CreateRibbonTab( a );
      return Result.Succeeded;
    }

    public Result OnShutdown( UIControlledApplication a )
    {
      return Result.Succeeded;
    }
  }
}
