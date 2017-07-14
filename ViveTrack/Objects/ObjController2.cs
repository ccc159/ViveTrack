using System;
using System.Collections.Generic;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Rhino.Geometry;
using ViveTrack.Properties;

namespace ViveTrack.Objects
{
    public class ObjController2 : GH_Component
    {
        public GeometryBase controller;
        public VrTrackedDevice CurrenTrackedDevice;
        public Plane XyPlane;
        public bool Paused = false;
        private Plane OldPlane;
        private Transform OldTransform;
        /// <summary>
        /// Initializes a new instance of the ObjController class.
        /// </summary>
        public ObjController2()
          : base("Controller2", "Controller2",
              "Tracking of HTC Vive Controller",
              "ViveTrack", "Tracking Device")
        {
            controller = GH_Convert.ByteArrayToCommonObject<GeometryBase>(System.Convert.FromBase64String(Resources.controller));
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
            pManager.AddGeometryParameter("Controller", "Controller", "HTC Vive Controller 3D model", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "The Lighthouse plane representation", GH_ParamAccess.item);
            pManager.AddGenericParameter("Matrix", "Matrix", "Transformation matrix of Lighthouse", GH_ParamAccess.item);
            pManager.AddGenericParameter("TriggerState", "TriggerState", "Return the states of trigger. A list of\n" +
                                                                         "1. TriggerPressed(True or False)\n" +
                                                                         "2. TriggerClicked(True or False)\n" +
                                                                         "3. TriggerValue(value frome 0 to 1, 1 means fully pressed)", GH_ParamAccess.list);
            pManager.AddGenericParameter("TouchPadState", "TouchPadState", "Return the states of TouchPad. A list of\n" +
                                                                           "1. TouchPadTouched(True or False)\n" +
                                                                           "2. TouchPadClicked(True or False)\n" +
                                                                           "3. TouchPadnValueX(value from -1 to 1, the touchpad from left to right)\n" +
                                                                           "4. TouchPadnValueY(value from -1 to 1, the touchpad from bottom to top)", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            OpenvrWrapper temp = null;
            if (!DA.GetData("Vive", ref temp)) return;
            var list = temp.TrackedDevices.IndexesByClasses["Controller"];
            if (list.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No Controller deteceted");
                return;
            }
            if (list.Count == 1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There's only one Controller, please use Controller1");
                return;
            }
            int index = temp.TrackedDevices.IndexesByClasses["Controller"][1];

            CurrenTrackedDevice = temp.TrackedDevices.AllDevices[index];
            this.Message = "Controller2";
            CurrenTrackedDevice.ConvertPose();
            XyPlane = Plane.WorldXY;
            XyPlane.Transform(CurrenTrackedDevice.CorrectedMatrix4X4);
            if (!Paused)
            {
                OldPlane = XyPlane;
                OldTransform = CurrenTrackedDevice.CorrectedMatrix4X4;
            }

            var newcontroller = controller.Duplicate();
            newcontroller.Transform(OldTransform);
            DA.SetData("Controller", newcontroller);
            DA.SetData("Plane", OldPlane);
            DA.SetData("Matrix", OldTransform);

            CurrenTrackedDevice.GetControllerTriggerState();
            DA.SetDataList("TriggerState", CurrenTrackedDevice.TriggerStates);
            DA.SetDataList("TouchPadState", CurrenTrackedDevice.TouchPadStates);
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
                return Resources.ObjController2;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1addfee6-dcb0-4696-98d7-5ecb46cb9e33}"); }
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