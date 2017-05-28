using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack.Objects
{
    public class ObjHMD : GH_Component
    {
        public GeometryBase HMD;
        /// <summary>
        /// Initializes a new instance of the ObjHMD class.
        /// </summary>
        public ObjHMD()
          : base("ObjHMD", "ObjHMD",
              "HTC Vive HMD 3D model",
              "ViveTrack", "Objects")
        {
            HMD = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.HMD));
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
            pManager.AddGeometryParameter("HMD", "HMD", "HTC Vive HMD 3D model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, HMD);
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
                return Resources.ObjHMD;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{c4c51ad6-b5c8-4ba8-a971-ff3240db33c7}"); }
        }
    }
}