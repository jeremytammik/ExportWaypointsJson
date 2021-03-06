using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "ExportWaypointsJson" )]
[assembly: AssemblyDescription( "A C# .NET Revit add-in to export exit path guide waypoints to JSON for Hololens visualisation" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "Autodesk Inc." )]
[assembly: AssemblyProduct( "ExportWaypointsJson" )]
[assembly: AssemblyCopyright( "Copyright 2016 (C) Jeremy Tammik, Autodesk Inc." )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "321044f7-b0b2-4b1c-af18-e71a19252be0" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
//
// History:
//
// 2016-09-06 2017.0.0.0 initial Revit add-in skeleton
// 2016-09-06 2017.0.0.1 added Newtonsoft.Json nuget package
// 2016-09-06 2017.0.0.2 started implementeing ribbon panel and options
// 2016-09-06 2017.0.0.2 implemented settings form text field input validation
// 2016-09-06 2017.0.0.2 implemented settings storage in both XML and JSOPN
// 2016-09-06 2017.0.0.3 implemented exit path model curve selection
// 2016-09-06 2017.0.0.4 implemented exit path model curve tesselation and export to JSON file
// 2016-09-06 2017.0.0.5 set copy local to false on AdWindows.dll
// 2016-09-06 2017.0.0.5 removed unused Newtonsoft.Json nuget package
// 2016-09-06 2017.0.0.6 implemented properly spaced waypoints according to the distance in metres defined in the user settings
// 2016-09-06 2017.0.0.7 first erroneous attempt to serialise XYZ output coordinates truncated to two decimal places
// 2016-09-06 2017.0.0.8 implemented XyzInMetres with two-digit coordinates
// 2016-09-06 2017.0.0.9 implemented waypoint text note markers
// 2016-09-07 2017.0.0.10 reduced option settings icon from 32x32 to 16x16
// 2016-09-07 2017.0.0.10 removed unused method
// 2016-09-07 2017.0.0.11 cleanup for publication
//
[assembly: AssemblyVersion( "2017.0.0.11" )]
[assembly: AssemblyFileVersion( "2017.0.0.11" )]
