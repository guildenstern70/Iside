using System;
using System.Drawing;
using System.Text;

namespace LiteSerializer
{

    /// <summary>
    /// A base class for RegistryOption implementations
    /// </summary>
    public abstract class RegistryOptionsBase
    {
        protected static string appKey;

        protected RegistryOption pserialNumber;
        protected RegistryOption pserialNumberName;
        protected RegistryOption pclientSize;
        protected RegistryOption pwindowLocation;

        protected RegistryOptionsBase(string applicationKey)
        {
            appKey = applicationKey;

            RegistrySpecs spcSerialNumber = new RegistrySpecs(typeof(System.String), appKey, "SrNumber", "nonreg");
            RegistrySpecs spcSerialNumberName = new RegistrySpecs(typeof(System.String), appKey, "SrNumberName", String.Empty);
            RegistrySpecs spcClientSize = new RegistrySpecs(typeof(System.Drawing.Point), appKey, "ClientSize", new Point(0, 0));
            RegistrySpecs spcWindowLocation = new RegistrySpecs(typeof(System.Drawing.Point), appKey, "WindowLocation", new Point(0, 0));

            this.pserialNumber = new RegistryOption(spcSerialNumber);
            this.pserialNumberName = new RegistryOption(spcSerialNumberName);
            this.pclientSize = new RegistryOption(spcClientSize);
            this.pwindowLocation = new RegistryOption(spcWindowLocation);

        }

        /// <summary>
        /// Window starting location
        /// </summary>
        public Point WindowLocation
        {
            get
            {
                return (Point)this.pwindowLocation.RegistryValue;
            }

            set
            {
                this.pwindowLocation.RegistryValue = value;
            }
        }

        /// <summary>
        /// Size of the client window
        /// </summary>
        public Size ClientSize
        {
            get
            {
                Point regPoint = (Point)this.pclientSize.RegistryValue;
                Size retSize = new Size(regPoint);
                return retSize;
            }

            set
            {
                this.pclientSize.RegistryValue = (Point)value;
            }
        }

        /// <summary>
        /// Serial Number
        /// </summary>
        public string SerialNumber
        {
            get
            {
                string sn = (string)this.pserialNumber.RegistryValue;
                if (sn == null)
                {
                    sn = String.Empty;
                }
                return sn;
            }

            set
            {
                this.pserialNumber.RegistryValue = value;
            }
        }

        /// <summary>
        /// Serial Name
        /// </summary>
        public string SerialNumberName
        {
            get
            {
                string sn = (string)this.pserialNumberName.RegistryValue;
                if (sn == null)
                {
                    sn = String.Empty;
                }
                return sn;
            }

            set
            {
                this.pserialNumberName.RegistryValue = value;
            }
        }


        /// <summary>
        /// Use this for Boolean keys
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        protected static bool FromInt(object x)
        {
            if (x.GetType() == Type.GetType("System.Boolean"))
            {
                return (bool)x;
            }
            else
            {
                try
                {
                    int xx = (int)x;
                    if (xx > 0)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
