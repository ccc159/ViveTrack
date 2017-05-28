using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack.Objects
{
    public class ObjController : GH_Component
    {
        public GeometryBase controller;
        /// <summary>
        /// Initializes a new instance of the ObjController class.
        /// </summary>
        public ObjController()
          : base("ObjController", "ObjController",
              "HTC Vive Controller 3D model",
              "ViveTrack", "Objects")
        {
            controller = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.controller));
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
            pManager.AddGeometryParameter("Controller", "Controller", "HTC Vive Controller 3D model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, controller);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1fcf483a-0221-42d8-9b89-2468a716e43b}"); }
        }
    }
}