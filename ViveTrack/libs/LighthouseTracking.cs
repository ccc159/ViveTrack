using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR;

namespace ViveTrack
{
    class LighthouseTracking
    {
        // Basic stuff
        private CVRSystem m_pHMD = null;
        private TrackedDevicePose_t[] m_rTrackedDevicePose = new TrackedDevicePose_t[16];
        private HmdMatrix34_t[] m_rmat4DevicePose = new HmdMatrix34_t[16];

        // Position and rotation of pose
        private HmdVector3_t GetPosition(HmdMatrix34_t matrix)
        {
            
        }

        private HmdQuaternion_t GetRotation(HmdMatrix34_t matrix)
        {
            
        }

        // If false the program will parse tracking data continously and not wait for openvr events
        private bool bWaitForEventsBeforeParsing = false;
       

        // Process a VR event and print some general info of what happens
        public bool ProcessVREvent(VREvent_t e)
        {
            

        }

        // Parse a tracking frame and print its position / rotation
        public void ParseTrackingFrame()
        {
            

        }

        // Destructor
        ~LighthouseTracking()
        {
            if (m_pHMD != null)
            {
                OpenVR.Shutdown();
                m_pHMD = null;
            }
            
        }

        public LighthouseTracking()
        {
            EVRInitError eError = EVRInitError.None;
            m_pHMD = OpenVR.Init(ref eError, EVRApplicationType.VRApplication_Background);
            if (eError != EVRInitError.None)
            {
                m_pHMD = null;
                string errMsg = "Unable to init VR runtime: %s";
            }
        }

        /*
        * Loop-listen for events then parses them (e.g. prints the to user)
        * Returns true if success or false if openvr has quit
        */
        // Main loop that listens for openvr events and calls process and parse routines, if false the service has quit
        public bool RunProcedure(bool bWaitForEvents)
        {
            // Either A) wait for events, such as hand controller button press, before parsing...
            if (bWaitForEvents)
            {
                // Process VREvent
                VREvent_t eventT = new VREvent_t();
                var size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t));
                while (OpenVR.System.PollNextEvent(ref eventT, size))
                {
                    
                }
            }

        }


    }
}
