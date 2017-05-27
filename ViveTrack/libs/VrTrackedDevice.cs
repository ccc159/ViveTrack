using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;
using Rhino.Geometry;
using System.Numerics;
using Quaternion = Rhino.Geometry.Quaternion;

namespace ViveTrack
{
    public class VrTrackedDevice
    {
        private string device_class;
        private uint index;
        private CVRSystem vr;
        private VrTrackedDevices TrackedDevices;
        public string Battery { get { return GetStringProperty(ETrackedDeviceProperty.Prop_DeviceBatteryPercentage_Float); } }
        public string ModelNumber { get { return GetStringProperty(ETrackedDeviceProperty.Prop_ModelNumber_String); } }
        public string SerialNumber { get { return GetStringProperty(ETrackedDeviceProperty.Prop_SerialNumber_String); } }
        public HmdMatrix34_t Pose;
        public Transform Matrix4x4;
        public Vector3d Translation;
        public Quaternion Quaternion;





        public VrTrackedDevice(VrTrackedDevices trackedDevices, int iindex)
        {
            this.vr = trackedDevices.vr;
            this.TrackedDevices = trackedDevices;
            this.index = Convert.ToUInt32(iindex);
            this.device_class = GetClass();

        }

        public VrTrackedDevice(){}

        string GetStringProperty(ETrackedDeviceProperty prop)
        {
            var error = ETrackedPropertyError.TrackedProp_Success;
            var capactiy = vr.GetStringTrackedDeviceProperty(this.index, prop, null, 0, ref error);
            if (capactiy > 1)
            {
                var result = new System.Text.StringBuilder((int)capactiy);
                vr.GetStringTrackedDeviceProperty(this.index, prop, result, capactiy, ref error);
                return result.ToString();
            }
            return (error != ETrackedPropertyError.TrackedProp_Success) ? error.ToString() : "<unknown>";
        }

        public string GetTrackedDeviceString()
        {
            var error = ETrackedPropertyError.TrackedProp_Success;
            var capacity = vr.GetStringTrackedDeviceProperty(this.index, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, null, 0, ref error);
            if (capacity > 1)
            {
                var result = new System.Text.StringBuilder((int)capacity);
                vr.GetStringTrackedDeviceProperty(this.index, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, result, capacity, ref error);
                return result.ToString();
            }
            return null;
        }


        private string GetClass()
        {
            var type = vr.GetTrackedDeviceClass(this.index);
            if (type == ETrackedDeviceClass.Controller)
            {
                this.TrackedDevices.Controllers += 1;
                this.TrackedDevices.IndexesByClasses["Controller"].Add(Convert.ToInt16(index));
                return "Controller";
            }
            if (type == ETrackedDeviceClass.HMD)
            {
                this.TrackedDevices.HMDs += 1;
                this.TrackedDevices.IndexesByClasses["HMD"].Add(Convert.ToInt16(index));
                return "HMD";
            }
            if (type == ETrackedDeviceClass.GenericTracker)
            {
                this.TrackedDevices.Trackers += 1;
                this.TrackedDevices.IndexesByClasses["Tracker"].Add(Convert.ToInt16(index));
                return "Tracker";
            }
            if (type == ETrackedDeviceClass.TrackingReference)
            {
                this.TrackedDevices.TrackingReferences += 1;
                this.TrackedDevices.IndexesByClasses["Lighthouse"].Add(Convert.ToInt16(index));
                return "Lighthouse";
            }
            return "unknown";
        }

        public void ConvertPose()
        {
            GetMatrix4x4FromPose();
            GetTranslationFromPose();
            GetQuaternionFromPose();
        }

        public void GetMatrix4x4FromPose()
        {
           
        }

        public void GetTranslationFromPose()
        {
            this.Translation = new Vector3d(Pose.m3, Pose.m7, Pose.m11);
        }

        public void GetQuaternionFromPose()
        {
            var w = Math.Sqrt(Math.Max(0, 1 + Pose.m0 + Pose.m5 + Pose.m10)) / 2;
            var x = Math.Sqrt(Math.Max(0, 1 + Pose.m0 - Pose.m5 - Pose.m10)) / 2;
            var y = Math.Sqrt(Math.Max(0, 1 - Pose.m0 + Pose.m5 - Pose.m10)) / 2;
            var z = Math.Sqrt(Math.Max(0, 1 - Pose.m0 - Pose.m5 + Pose.m10)) / 2;
            x = Math.Abs(x) * Math.Sign(Pose.m9 - Pose.m6);
            y = Math.Abs(y) * Math.Sign(Pose.m2 - Pose.m8);
            z = Math.Abs(z) * Math.Sign(Pose.m4 - Pose.m1);
            this.Quaternion = new Rhino.Geometry.Quaternion(x,y,z,w);
        }

        public override string ToString()
        {
            return "Name: " + device_class + ",Model: " + ModelNumber + ",Serial: " +  SerialNumber + ",Battery: " + Battery;
        }

        public string PosetoString()
        {
            return $"{Pose.m0},{Pose.m1},{Pose.m2},{Pose.m3},{Pose.m4},{Pose.m5},{Pose.m6},{Pose.m7},{Pose.m8},{Pose.m9},{Pose.m10},{Pose.m11}";

        }

    }


}
