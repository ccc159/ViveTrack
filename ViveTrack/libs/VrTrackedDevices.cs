using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace ViveTrack
{
    public class VrTrackedDevices
    {
        public CVRSystem vr;
        public List<int> Indexes = new List<int>();
        public int HMDs = 0;
        public int Controllers = 0;
        public int Trackers = 0;
        public int TrackingReferences = 0;
        public int NumDetected = 0;
        public Dictionary<int, VrTrackedDevice>  AllDevices;
        public TrackedDevicePose_t[] Poses = new TrackedDevicePose_t[OpenVR.k_unMaxTrackedDeviceCount];



        public VrTrackedDevices(CVRSystem ivr)
        {
            this.vr = ivr;
            AllDevices = new Dictionary<int, VrTrackedDevice>();
        }

        public void AddTrackedDevice(int index)
        {
            AllDevices.Add(index, new VrTrackedDevice(this, index));
            Indexes.Add(index);
            NumDetected += 1;
        }

        public string Summary()
        {
            if (NumDetected == 0) return "No devices detected";
            else return $"Found: HDMs x {HMDs} , Controllers x {Controllers}, Trackers x {Trackers}, Lighthouses x {TrackingReferences}";
        }

        public void UpdatePoses()
        {
            vr.GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin.TrackingUniverseStanding, 0, Poses);
            foreach (var index in Indexes)
            {
                this.AllDevices[index].Pose = Poses[index].mDeviceToAbsoluteTracking;
            }
        }
 

    }


}
