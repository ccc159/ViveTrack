using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack.Objects
{
    public class ObjTracker : GH_Component
    {
        public GeometryBase tracker;
        /// <summary>
        /// Initializes a new instance of the ObjTracker class.
        /// </summary>
        public ObjTracker()
          : base("ObjTracker", "ObjTracker",
              "HTC Vive Tracker 3D model",
              "ViveTrack", "Objects")
        {
            tracker = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.tracker));
            (this as IGH_PreviewObject).Hidden = true;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Tracker", "Tracker", "HTC Vive Tracker 3D model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, tracker);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.ObjTracker;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{bbe2bc88-530b-4f06-bca9-d77ca9db52da}"); }
        }
    }
}