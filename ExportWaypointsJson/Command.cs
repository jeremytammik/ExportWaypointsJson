#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;
#endregion

namespace ExportWaypointsJson
{
  [Transaction( TransactionMode.Manual )]
  public class Command : IExternalCommand
  {
    /// <summary>
    /// Place text note markers along the escape path waypoints?
    /// </summary>
    static bool _create_waypoint_markers = true;

    const string _exit_path_filename = "exitpath.json";

    const string _please_select_model_curve = "Please "
      + "select a single model curve representing the "
      + "exit path";

    /// <summary>
    /// Revit selection filter for model curves.
    /// </summary>
    class CurveSelectionFilter : ISelectionFilter
    {
      public bool AllowElement( Element e )
      {
        return e is ModelCurve;
      }

      public bool AllowReference( Reference r, XYZ p )
      {
        return true;
      }
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;
      View view = uidoc.ActiveView;

      bool create_waypoint_markers 
        = _create_waypoint_markers 
        && view is ViewPlan;

      // Select model curve representing exit path.

      ModelCurve model_curve = null;
      Selection sel = uidoc.Selection;
      ICollection<ElementId> ids = sel.GetElementIds();
      int n = ids.Count;

      if( 0 == n )
      {
        try
        {
          Reference r = sel.PickObject( ObjectType.Element,
            new CurveSelectionFilter(),
            "Please pick a model curve to represent the exit path" );

          model_curve = doc.GetElement( r.ElementId ) as ModelCurve;
        }
        catch( Autodesk.Revit.Exceptions.OperationCanceledException )
        {
          message = "No element selected";
          return Result.Cancelled;
        }
      }
      else
      {
        if( 1 == n )
        {
          model_curve = doc.GetElement(
            ids.First<ElementId>() )
              as ModelCurve;
        }
        if( null == model_curve )
        {
          message = _please_select_model_curve;
          return Result.Failed;
        }
      }

      Curve curve = model_curve.GeometryCurve;

      // Tessellate returns 377 points for our sample 
      // spline curve. We need a way to control the 
      // number of points returned, and the distance 
      // between them.
      //IList<XYZ> pts = curve.Tessellate();

      double length = curve.ApproximateLength;

      double length_m = length * XyzInMetres.convert_feet_to_metre;
      double d = Settings.Load().DistanceInMetres;

      int nPoints = (int) ( ( length_m / d ) + 1 );

      List<XyzInMetres> pts = new List<XyzInMetres>( nPoints );

      double t = curve.GetEndParameter( 0 );
      double tend = curve.GetEndParameter( 1 );
      d = ( tend - t ) / nPoints;

      using( Transaction tx = new Transaction( doc ) )
      {
        ElementId textNoteTypeId
          = ElementId.InvalidElementId;

        if( _create_waypoint_markers )
        {
          tx.Start( "Create waypoint markers" );

          textNoteTypeId 
            = new FilteredElementCollector( doc )
              .OfClass( typeof( TextNoteType ) )
              .FirstElementId();
        }

        int i = 0;

        for( ; t < tend; t += d )
        {
          XYZ p_in_feet = curve.Evaluate( t, false );

          if( _create_waypoint_markers )
          {
            TextNote.Create( doc, view.Id, p_in_feet,
              ( ++i ).ToString(), textNoteTypeId );
          }
          pts.Add( new XyzInMetres( p_in_feet ) );
        }
        if( _create_waypoint_markers )
        {
          tx.Commit();
        }
      }

      // Attempt to register a custom converter to 
      // output XYZ point coordinates truncated to 
      // two decimal places. This does not work, cf.
      // http://stackoverflow.com/questions/12283070/serializing-a-decimal-to-json-how-to-round-off
      //JavaScriptSerializer serializer = new JavaScriptSerializer();
      //serializer.RegisterConverters( 
      //  new JavaScriptConverter[] {
      //    new TwoDecimalPlacesConverter() } );
      // Instead, we implemented XyzInMetres and 
      // created a wokring solution.

      string path = Path.Combine( App.Path, 
        _exit_path_filename );

      File.WriteAllText( path,
        ( new JavaScriptSerializer() ).Serialize(
          pts ) );

      TaskDialog.Show( App.Caption, string.Format(
        "Exported {0} waypoints to {1}.",
        pts.Count, path ) );

      return Result.Succeeded;
    }
  }
}
