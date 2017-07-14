using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace ViveTrack
{
    public class ViveTrackInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "ViveTrack";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "ViveTrack is used to get VIVE tracking device pose in grasshopper";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("d4ebf406-f89c-4f60-a6c1-3c888519df13");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Jingcheng Chen";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "mail@chenjingcheng.com";
            }
        }
    }
}
