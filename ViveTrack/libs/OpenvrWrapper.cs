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
        private CVRSystem vr = null;
        public TrackedDevicePose_t[] Poses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];
        public VrTrackedDevices TrackedDevices;

        public OpenvrWrapper()
        {
            //Initialize OpenVR  
            EVRInitError eError = EVRInitError.None;
            vr = OpenVR.Init(ref eError,EVRApplicationType.VRApplication_Background);
            TrackedDevices = new VrTrackedDevices(this.vr);
            //Initializing object to hold indexes for various tracked objects 
            vr.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseStanding, 0, Poses);

            //# Iterate through the pose list to find the active Devices and determine their type
            for (int i = 0; i < OpenVR.k_unMaxTrackedDeviceCount; i++)
            {
                if (Poses[i].bPoseIsValid)TrackedDevices.AddTrackedDevice(i);
            }
        }
 
    }
}
