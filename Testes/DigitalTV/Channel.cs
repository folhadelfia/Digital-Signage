using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalTV
{
    public class Channel
    {
        private short sid;
        private string name;
        private byte type;

        private bool freeCAMode;
        private short videoPid;
        private short audioPid;
        private short modulation;

        public short SID
        {
            get { return sid; }
            set { sid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public byte Type
        {
            get { return type; }
            set { type = value; }
        }
        public bool FreeCAMode
        {
            get { return freeCAMode; }
            set { freeCAMode = value; }
        }
        public short VideoPid
        {
            get { return videoPid; }
            set { videoPid = value; }
        }
        public short AudioPid
        {
            get { return audioPid; }
            set { audioPid = value; }
        }
        public short Modulation
        {
            get { return modulation; }
            set { modulation = value; }
        }

        public override string ToString()
        {
            return "(" + sid + "-" + type.ToString() + ") " + name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Channel)) return false;
            return (obj as Channel).SID == SID;
        }

        public override int GetHashCode()
        {
            int typeInt = Convert.ToInt32(type);
            return Convert.ToInt32(typeInt.ToString() + SID.ToString());
        }
    }
}
