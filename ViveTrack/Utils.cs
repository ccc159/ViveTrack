using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace ViveTrack
{
    static class Utils
    {
        //public static Plane ConvertFromGHPlaneToRhinoPlane(GH_Plane ghPlane)
        //{
        //    Point3d o = new Point3d(ghPlane.Origin.x, ghPlane.Origin.y, ghPlane.Origin.z);
        //    Point3d x = new Point3d(ghPlane.XAxis.x, ghPlane.XAxis.y, ghPlane.XAxis.z);
        //    Point3d y = new Point3d(ghPlane.YAxis.x, ghPlane.YAxis.y, ghPlane.YAxis.z);
        //    return new Plane(o, x, y);
        //}

        //public static GH_Plane ConvertFromRhinoToGHPlane(Plane plane)
        //{
        //    GH_Point3D o = new GH_Point3D(plane.Origin.X, plane.Origin.Y, plane.Origin.Z);
        //    GH_Point3D x = new GH_Point3D(plane.XAxis.X, plane.XAxis.Y, plane.XAxis.Z);
        //    GH_Point3D y = new GH_Point3D(plane.YAxis.X, plane.YAxis.Y, plane.YAxis.Z);
        //    return new GH_Plane(o, x, y);
        //}


    }
}
