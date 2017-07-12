using System;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;
using Quaternion = System.Numerics.Quaternion;

namespace ViveTrack.Objects
{
    public class ObjHMD : GH_Component
    {
        public GeometryBase HMD;
        public VrTrackedDevice CurrenTrackedDevice;
        public Plane XyPlane;
        public bool Paused = false;
        private Plane OldPlane;
        private Transform OldTransform;
        /// <summary>
        /// Initializes a new instance of the ObjHMD class.
        /// </summary>
        public ObjHMD()
          : base("Vive HMD", "HMD",
              "Tracking of HTC Vive HMD",
              "ViveTrack", "Tracking Device")
        {
            HMD = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.HMD));
            CurrenTrackedDevice = new VrTrackedDevice();
            //(this as IGH_PreviewObject).Hidden = true;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Vive", "Vive", "Passing Vive Runtime to current component", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("HMD", "HMD", "HTC Vive HMD 3D model", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "The HMD plane representation", GH_ParamAccess.item);
            pManager.AddGenericParameter("Matrix", "Matrix", "Transformation matrix of HMD", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            OpenvrWrapper temp = null;
            if(!DA.GetData("Vive", ref temp)) return;
            var list = temp.TrackedDevices.IndexesByClasses["HMD"];
            if (list.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No HMD deteceted");
                return;
            } 
            int index = temp.TrackedDevices.IndexesByClasses["HMD"][0];

            CurrenTrackedDevice = temp.TrackedDevices.AllDevices[index];
            this.Message = "HMD";
            CurrenTrackedDevice.ConvertPose();
            XyPlane = Plane.WorldXY;
            XyPlane.Transform(CurrenTrackedDevice.CorrectedMatrix4X4);
            if (!Paused)
            {
                OldPlane = XyPlane;
                OldTransform = CurrenTrackedDevice.CorrectedMatrix4X4;
            }

            var newHMD = HMD.Duplicate();
            newHMD.Transform(OldTransform);
            DA.SetData("HMD", newHMD);
            DA.SetData("Plane", OldPlane);
            DA.SetData("Matrix", OldTransform);

            
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

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Pause", Menu_Click_Pause, true, false).ToolTipText = @"Click Pause to pause the updation of current tracking device.";


        }

        private void Menu_Click_Pause(object sender, EventArgs e)
        {
            this.RecordUndoEvent("Pause");
            Paused = !Paused;
            this.ExpireSolution(true);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("Pause", Paused);
            return base.Write(writer);
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