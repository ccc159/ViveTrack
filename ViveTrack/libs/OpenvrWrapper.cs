using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace ViveTrack
{
    public class OpenvrWrapper
    {
        internal CVRSystem vr = null;
        public TrackedDevicePose_t[] Poses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        public VrTrackedDevices TrackedDevices;
        public string errorMsg;
        public bool Success;

        public OpenvrWrapper()
        {
            Connect();
            Update();
        }

        ~OpenvrWrapper()
        {
            OpenVR.Shutdown();
        }

        public void Connect()
        {
            if (vr != null) return;
            //Initialize OpenVR  
            EVRInitError eError = EVRInitError.None;
            vr = OpenVR.Init(ref eError, EVRApplicationType.VRApplication_Background);
            if (eError != EVRInitError.None)
            {
                ReportError(eError);
                OpenVR.Shutdown();
                vr = null;
                Success = false;
                return;
            }
            Success = true;
        }

        public void Update()
        {
            if (!Success) return;
            TrackedDevices = new VrTrackedDevices(this.vr);
            //Initializing object to hold indexes for various tracked objects 
            vr.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseStanding, 0, Poses);

            //# Iterate through the pose list to find the active Devices and determine their type
            for (int i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
            {
                if (Poses[i].bPoseIsValid) TrackedDevices.AddTrackedDevice(i);
            }
        }

        private void ReportError(EVRInitError error)
        {
            switch (error)
            {
                case EVRInitError.None:
                    break;
                case EVRInitError.VendorSpecific_UnableToConnectToOculusRuntime:
                    errorMsg = ("SteamVR Initialization Failed!  Make sure device is on, Oculus runtime is installed, and OVRService_*.exe is running.");
                    break;
                case EVRInitError.Init_VRClientDLLNotFound:
                    errorMsg = ("SteamVR drivers not found!  They can be installed via Steam under Library > Tools.  Visit http://steampowered.com to install Steam.");
                    break;
                case EVRInitError.Driver_RuntimeOutOfDate:
                    errorMsg = ("SteamVR Initialization Failed!  Make sure device's runtime is up to date.");
                    break;
                default:
                    errorMsg = (OpenVR.GetStringForHmdError(error));
                    break;
            }
        }

    }
}
