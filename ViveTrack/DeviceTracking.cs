using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Numerics;
using GH_IO.Serialization;
using ViveTrack.Properties;
using Plane = Rhino.Geometry.Plane;
using Quaternion = System.Numerics.Quaternion;

namespace ViveTrack
{
    public class DeviceTracking : GH_Component
    {
        public VrTrackedDevice CurrenTrackedDevice;
        public Plane XyPlane;
        public bool Paused = false;

        private Plane OldPlane;
        private Vector3d OldTranslation;
        private Quaternion OldQuaternion;
        private Transform OldTransform;
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
            pManager.AddVectorParameter("Translation", "Translation", "3D position vector of tracked device", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion", "Quaternion", "Quaternion of tracked device", GH_ParamAccess.list);
            pManager.AddGenericParameter("Transform", "Transform", "Transformation matrix of tracked device", GH_ParamAccess.item);


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
            if (!temp.TrackedDevices.AllDevices.ContainsKey(index))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Current index doesn't exist in tracked devices.");
                return;
            }
            CurrenTrackedDevice = temp.TrackedDevices.AllDevices[index];
            this.Message = CurrenTrackedDevice.device_class;
            CurrenTrackedDevice.ConvertPose();
            XyPlane = Plane.WorldXY;
            XyPlane.Transform(CurrenTrackedDevice.CorrectedMatrix4X4);
            if (!Paused)
            {
                OldPlane = XyPlane;
                OldTranslation = CurrenTrackedDevice.CorrectedTranslation;
                OldQuaternion = CurrenTrackedDevice.CorrectedQuaternion;
                OldTransform = CurrenTrackedDevice.CorrectedMatrix4X4;
            }
            DA.SetData("Plane", OldPlane);
            DA.SetData("Translation", OldTranslation);
            DA.SetData("Quaternion", OldQuaternion);
            DA.SetData("Transform", OldTransform);
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
                return Resources.DeviceTracking;
            }
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Pause", Menu_Click_Pause,true,false).ToolTipText=@"Click Pause to pause the updation of current tracking device.";

            
        }

        private void Menu_Click_Pause(object sender, EventArgs e)
        {
            this.RecordUndoEvent("Pause");
            Paused = !Paused;
            this.ExpireSolution(true);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("Pause",Paused);
            return base.Write(writer);
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