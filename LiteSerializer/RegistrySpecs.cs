/**
 **   LiteSerializer
 **   A Serialization engine type aware
 **
 **   Copyright © 2004-2013 LittleLite Software
 **
 **
 **/
using System;
using System.Text;

namespace LiteSerializer
{
	/// <summary>
    /// RegistrySpecs. Specifies the details about the option to be saved.
	/// </summary>
	public sealed class RegistrySpecs
	{
		private Type regType;
		private string regKey;
		private string regName;
		private object regValue;
		private object regDefaultValue;

		/// <summary>
		/// Specifies the details about the option to be saved.
		/// </summary>
		/// <param name="type">Registry type value. It can be only string, enum or int</param>
		/// <param name="key">It's the key to append to HKCU/Software, ie: "LittleLite Software\\Iside"</param>
		/// <param name="name">This name of this option under the key in the registry</param>
        /// <param name="defaultValue">The default value of this option</param>
		public RegistrySpecs(Type type, string key, string name, object defaultValue)
		{
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

			if ((type != Type.GetType("System.String")) && 
                (type != Type.GetType("System.Int32")) &&
                (type != Type.GetType("System.Boolean")))
			{
                if ((type.FullName != "System.Drawing.Point") && 
                    (type.BaseType != Type.GetType("System.Enum")) &&
                    (type.FullName != "System.Drawing.Font") &&
                    (type.FullName != "System.Drawing.Color"))
				{
					throw new ArgumentException("Type can be only Enum, Point, int, bool or string");
				}
			}
			
			this.regType = type;
			this.regKey = key;
			this.regDefaultValue = defaultValue;
			this.regName = name;
			this.regValue = defaultValue;
		}

        /// <summary>
        /// Type of this option
        /// </summary>
		public Type RegOptionType
		{
			get
			{
				return this.regType;
			}
		}

        /// <summary>
        /// The key of this option
        /// </summary>
		public string RegOptionKey
		{
			get
			{
				return this.regKey;
			}
		}

        /// <summary>
        /// The name of this option under the key
        /// </summary>
		public string RegOptionName
		{
			get
			{
				return this.regName;
			}
		}

        /// <summary>
        /// The value of this option. If no value is found on
        /// the registry, it returns the default value.
        /// </summary>
		public object RegOptionValue
		{
			get
			{
                object retVal = this.regValue;
                if (retVal == null)
                {
                    retVal = this.regDefaultValue;
                }
                return retVal;
			}

			set
			{
				this.regValue =  value;
			}
		}

        /// <summary>
        /// Default value of this option
        /// </summary>
		public object RegDefaultValue
		{
			get
			{
				return this.regDefaultValue;
			}
		}


        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder regOpt = new StringBuilder("Registry: [");
            regOpt.Append(this.regKey);
            regOpt.Append("],[");
            regOpt.Append(this.regName);
            regOpt.Append("],[");
            regOpt.Append(this.regValue);
            regOpt.Append("]");
            return regOpt.ToString();
        }
	}
}
