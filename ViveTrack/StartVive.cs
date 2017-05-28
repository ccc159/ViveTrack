using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace ViveTrack
{
    public class StartVive : GH_Component
    {
        public OpenvrWrapper Vive;
        public string OutMsg;
        /// <summary>
        /// Initializes a new instance of the StartVive class.
        /// </summary>
        public StartVive()
          : base("StartVive", "StartVive",
              "Start HTV Vive, make sure SteamVR is running",
              "ViveTrack", "ViveTrack")
        {
            Vive = new OpenvrWrapper();
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
            pManager.AddTextParameter("MSG", "MSG", "Running Information of your Vive", GH_ParamAccess.item);
            pManager.AddGenericParameter("Vive", "Vive", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Index", "Index", "The Index of running devices, from 0-16.",GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DetectSteamVR())
            {
                DA.SetData("MSG", "SteamVR not running.");
                AskforRunningSteamVR();
                return;
            }
            if (Vive.Success)
            {
                OutMsg = Vive.TrackedDevices.Summary();
                DA.SetDataList("Index", Vive.TrackedDevices.Indexes);
                DA.SetData("Vive", Vive);
                Vive.TrackedDevices.UpdatePoses();
            }
            else
            {
                OutMsg = "Vive is not setup correctly!! Detailed Reason:\n" + Vive.errorMsg + "\nCheck online the error code for more information.";
            }
            DA.SetData("MSG", OutMsg);

        }

        public bool DetectSteamVR()
        {
            Process[] vrServer = Process.GetProcessesByName("vrserver");
            Process[] vrMonitor = Process.GetProcessesByName("vrmonitor");
            if ((vrServer.Length != 0) && (vrMonitor.Length != 0)) return true;
            OutMsg = "SteamVR not running correctly.(Not detecting 'vrserver' and 'vrmonitor')\nDo you want to start SteamVR now?";
            return false;
        }

        public void AskforRunningSteamVR()
        {
            DialogResult dialogResult = MessageBox.Show($@"{OutMsg}", @"SteamVR not running", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Steam\steamapps\common\SteamVR\bin\win64\vrstartup.exe");
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
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
            get { return new Guid("{730315e3-b05e-4227-aed2-cba81450bce7}"); }
        }
    }
}