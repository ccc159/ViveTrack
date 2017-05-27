using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace ViveTrack
{
    public class DeviceTracking : GH_Component
    {
        public VrTrackedDevice CurrenTrackedDevice;
        public Plane XyPlane;
        /// <summary>
        /// Initializes a new instance of the DeviceTracking class.
        /// </summary>
        public DeviceTracking()
          : base("DeviceTracking", "DeviceTracking",
              "Description",
              "ViveTrack", "ViveTrack")
        {
            CurrenTrackedDevice = new VrTrackedDevice();
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Vive", "Vive", "Passing Vive Runtime to current component",GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "Index", "Index of the device that you want to track",GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "Plane", "The tracking device plane representation",GH_ParamAccess.item);
            pManager.AddVectorParameter("Translation", "Translation", "3D vector", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion", "Quaternion", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("TransformationMatrix", "TransformationMatrix", "", GH_ParamAccess.item);


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            OpenvrWrapper temp = null;
            int index = -1;
            DA.GetData("Vive", ref temp);
            DA.GetData("Index", ref index);
            if (index < 0 || index >= 16) return;
            CurrenTrackedDevice = temp.TrackedDevices.AllDevices[index];
            CurrenTrackedDevice.ConvertPose();
            XyPlane = Plane.WorldXY;
            XyPlane.Transform(CurrenTrackedDevice.CorrectedMatrix4X4);
            DA.SetData("Plane", XyPlane);
            DA.SetData("Translation", CurrenTrackedDevice.CorrectedTranslation);
            DA.SetData("Quaternion", CurrenTrackedDevice.CorrectedQuaternion);
            DA.SetData("TransformationMatrix", CurrenTrackedDevice.CorrectedMatrix4X4);
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
            get { return new Guid("{15988350-83b4-44f7-95b6-efcd625b10fe}"); }
        }
    }
}