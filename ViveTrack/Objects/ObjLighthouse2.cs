using System;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack.Objects
{
    public class ObjLighthouse2 : GH_Component
    {
        public GeometryBase lighthouse;
        public VrTrackedDevice CurrenTrackedDevice;
        public Plane XyPlane;
        public bool Paused = false;
        private Plane OldPlane;
        private Transform OldTransform;
        /// <summary>
        /// Initializes a new instance of the ObjLighthouse class.
        /// </summary>
        public ObjLighthouse2()
          : base("Lighthouse2", "Lighthouse2",
              "Tracking of HTC Vive Lighthouse",
              "ViveTrack", "Tracking Device")
        {
            lighthouse = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.lighthouse));
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
            pManager.AddGeometryParameter("Lighthouse", "Lighthouse", "HTC Vive Lighthouse 3D model", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "The Lighthouse plane representation", GH_ParamAccess.item);
            pManager.AddGenericParameter("Matrix", "Matrix", "Transformation matrix of Lighthouse", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            OpenvrWrapper temp = null;
            if (!DA.GetData("Vive", ref temp)) return;
            var list = temp.TrackedDevices.IndexesByClasses["Lighthouse"];
            if (list.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No Lighthouse deteceted");
                return;
            }
            if (list.Count == 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There's only one Lighthouse, please use Lighthouse1");
                return;
            }
            int index = temp.TrackedDevices.IndexesByClasses["Lighthouse"][1];

            CurrenTrackedDevice = temp.TrackedDevices.AllDevices[index];
            this.Message = "Lighthouse2";
            CurrenTrackedDevice.ConvertPose();
            XyPlane = Plane.WorldXY;
            XyPlane.Transform(CurrenTrackedDevice.CorrectedMatrix4X4);
            if (!Paused)
            {
                OldPlane = XyPlane;
                OldTransform = CurrenTrackedDevice.CorrectedMatrix4X4;
            }

            var newlighthouse = lighthouse.Duplicate();
            newlighthouse.Transform(OldTransform);
            DA.SetData("Lighthouse", newlighthouse);
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
                return Resources.ObjLighthouse2;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{2938302e-5d57-4002-997d-17a662d26663}"); }
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
    }
}