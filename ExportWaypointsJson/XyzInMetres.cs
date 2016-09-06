using Autodesk.Revit.DB;
using System;

namespace ExportWaypointsJson
{
  class XyzInMetres
  {
    public const double convert_inch_to_cm = 2.54;
    public const double convert_feet_to_metre = 0.01 * 12 * convert_inch_to_cm;

    public double X;
    public double Y;
    public double Z;

    static double FeetToMetre( double d_in_feet )
    {
      return Math.Round( 
        convert_feet_to_metre * d_in_feet, 
        2 );
    }

    public XyzInMetres( XYZ p_in_feet )
    {
      X = FeetToMetre( p_in_feet.X );
      Y = FeetToMetre( p_in_feet.Y );
      Z = FeetToMetre( p_in_feet.Z );

    }
  }
}
