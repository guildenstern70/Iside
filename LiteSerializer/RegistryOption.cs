/**
 **   LiteSerializer
 **   A Serialization engine type aware
 **
 **   Copyright © 2004-2013 LittleLite Software
 **
 **
 **/
using System;
using System.Drawing;
using AxsUtils.Win32;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LiteSerializer
{
	/// <summary>
	/// Class for an option that can be inserted into registry
	/// </summary>
	public class RegistryOption
	{
		private RegistrySpecs regKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:RegistryOption"/> class.
        /// </summary>
        /// <param name="specs">The specs.</param>
		public RegistryOption(RegistrySpecs specs)
		{
			this.regKey = specs;
		}

        /// <summary>
        /// Gets the registry key.
        /// </summary>
        /// <value>The registry key.</value>
		public RegistrySpecs RegistryKey
		{
			get
			{
				return this.regKey;
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
            return this.regKey.ToString();
        }

        /// <summary>
        /// Gets or sets the registry value.
        /// </summary>
        /// <value>The registry value.</value>
		public object RegistryValue
		{
			set
			{
				this.regKey.RegOptionValue = value;
				Type type = this.regKey.RegOptionType;

				if (type == Type.GetType("System.String"))
				{
					this.WriteStringToRegistry();
				}
				else if (type == Type.GetType("System.Int32"))
				{
					this.WriteIntToRegistry();
				}
				else if (type.BaseType == Type.GetType("System.Enum"))
				{
					this.WriteEnumToRegistry();
				}
                else if (type == Type.GetType("System.Boolean"))
                {
                    this.WriteBoolToRegistry();
                }
                else if (type.FullName == "System.Drawing.Point")
                {
                    this.WritePointToRegistry();
                }
                else if (type.FullName == "System.Drawing.Font")
                {
                    this.WriteFontToRegistry();
                }
                else if (type.FullName == "System.Drawing.Color")
                {
                    this.WriteColorToRegistry();
                }
                else
                {
                    throw new ArgumentException("Type can be only Enum, int or string");
                }
			}

			get
			{
				Type type = this.regKey.RegOptionType;

				if (type == Type.GetType("System.String"))
				{
					this.ReadStringFromRegistry();
				}
				else if (type == Type.GetType("System.Int32"))
				{
					this.ReadIntFromRegistry();
				}
				else if (type.BaseType == Type.GetType("System.Enum"))
				{
					this.ReadEnumFromRegistry();
				}
                else if (type == Type.GetType("System.Boolean"))
                {
                    this.ReadBoolFromRegistry();
                }
                else if (type.FullName == "System.Drawing.Point")
                {
                    this.ReadPointFromRegistry();
                }
                else if (type.FullName == "System.Drawing.Font")
                {
                    this.ReadFontFromRegistry();
                }
                else if (type.FullName == "System.Drawing.Color")
                {
                    this.ReadColorFromRegistry();
                }
                else
                {
                    throw new ArgumentException("Type can be only Enum, Point, Font, Color, int or string");
                }

				return this.regKey.RegOptionValue;
			}
		}

        private void WriteColorToRegistry()
        {
            Color ccc = (Color)this.regKey.RegOptionValue;
            string colorString = String.Format("{0}", ccc.ToArgb());
            WinRegistry.SetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName, colorString);
        }

        private void ReadColorFromRegistry()
        {
            string sss = null;
            Color retCol = Color.Empty;

            try
            {
                sss = WinRegistry.GetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                int argb = Int32.Parse(sss);
                retCol = Color.FromArgb(argb);
                this.regKey.RegOptionValue = retCol;
            }
            catch (FormatException ex)
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("RegistryOption.ReadColorFromRegistry > Intercepted format exception: " + ex.Message);
            }
            catch (OverflowException ovex)
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("RegistryOption.ReadColorFromRegistry > Intercepted overflow exception: " + ovex.Message);
            }
            catch (ArgumentNullException nulEx)
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("RegistryOption.ReadColorFromRegistry > Intercepted null exception: " + nulEx.Message);
            }

        }

        private void ReadFontFromRegistry()
        {
            string serializedFont = null;
            Font f;

            try
            {
                serializedFont = WinRegistry.GetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                IFormatter formatter = new BinaryFormatter();
                byte[] fontbytes = Convert.FromBase64String(serializedFont);
                Stream stream = new MemoryStream(fontbytes);
                f = (Font)formatter.Deserialize(stream);
                stream.Close();
                this.regKey.RegOptionValue = f;
            }
            catch (Exception ex)
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("RegistryOption.DeserializeFont > Intercepted exception: " + ex.Message);
            }
        }

        private void WriteFontToRegistry()
        {
            Font f = (Font)this.regKey.RegOptionValue;
            System.Diagnostics.Debug.WriteLine("Writing to registry " + this.regKey.RegOptionName + " = FONT " + f.ToString());

            try
            {
                IFormatter formatter = new BinaryFormatter();
                byte[] buffer = new byte[2048];
                Stream stream = new MemoryStream(buffer, true);
                formatter.Serialize(stream, f);
                stream.Close();
                WinRegistry.SetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName, Convert.ToBase64String(buffer));
            }
            catch (Exception x)
            {
                System.Diagnostics.Debug.WriteLine("RegistryOption.SaveFontToRegistry > Intercepted Exception: " + x.Message);
            }
        }

        private void ReadBoolFromRegistry()
        {
            try
            {
                int retVal = WinRegistry.GetHKCUDWORDValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                if (retVal == Int32.MinValue)
                {
                    this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                    System.Diagnostics.Debug.WriteLine("Can't find option for " + this.regKey.RegOptionName + ": defaulting to " + this.regKey.RegOptionValue.ToString());
                }
                else
                {
                    this.regKey.RegOptionValue = retVal;
                    System.Diagnostics.Debug.WriteLine("Option for " + this.regKey.RegOptionName + " in registry is: " + Convert.ToString(this.regKey.RegOptionValue));
                }
            }
            catch
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("Cannot read option from registry for " + this.regKey.RegOptionName + ". Using default value.");
            }
        }

        private void WriteBoolToRegistry()
        {
            try
            {
                bool optionBool = (bool)this.regKey.RegOptionValue;
                System.Diagnostics.Debug.WriteLine("Writing to registry " + this.regKey.RegOptionName + " = " + optionBool.ToString());
                int optionInt = 0;
                if (optionBool)
                {
                    optionInt = 1;
                }
                WinRegistry.SetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName, optionInt);
            }
            catch
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("Cannot write to registry for {0}" + this.regKey.RegOptionName + " = " + this.regKey.RegOptionValue);
            }
        }

		/// <summary>
		/// Store the value read from registry in this.regValue.
		/// If got error, the optionValue is set to default value
		/// </summary>
		protected void ReadStringFromRegistry()
		{
			try
			{
                string retVal = WinRegistry.GetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                if (retVal == null)
                {
                    this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                    System.Diagnostics.Debug.WriteLine("Can't find option for " + this.regKey.RegOptionName + ", defaulting to: " + this.regKey.RegOptionValue);
                }
                else
                {
                    this.regKey.RegOptionValue = retVal;
                    System.Diagnostics.Debug.WriteLine("Option for " + this.regKey.RegOptionName + " in registry is: " + this.regKey.RegOptionValue);
                }
				
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot read option from registry for "+this.regKey.RegOptionName+". Using default value");
			}

		}

		protected void WriteStringToRegistry()
		{
			try
			{
				string optionString = (string)this.regKey.RegOptionValue;
				System.Diagnostics.Debug.WriteLine("Writing to registry "+this.regKey.RegOptionName+" = "+optionString);
				WinRegistry.SetHKCUValue(this.regKey.RegOptionKey,this.regKey.RegOptionName,optionString);
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot write to registry for {0}"+this.regKey.RegOptionName+" = "+this.regKey.RegOptionValue);
			}
		}

		/// <summary>
		/// Store the value read from registry in this.regValue.
		/// If got error, the optionValue is set to default value
		/// </summary>
		protected void ReadIntFromRegistry()
		{
			try
			{
                int retVal = WinRegistry.GetHKCUDWORDValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                if (retVal == Int32.MinValue)
                {
                    this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                    System.Diagnostics.Debug.WriteLine("Can't find option for " + this.regKey.RegOptionName + ": defaulting to " + this.regKey.RegOptionValue.ToString());
                }
                else
                {
                    this.regKey.RegOptionValue = retVal;
                    System.Diagnostics.Debug.WriteLine("Option for " + this.regKey.RegOptionName + " in registry is: " + Convert.ToString(this.regKey.RegOptionValue));
                }
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot read option from registry for "+this.regKey.RegOptionName+". Using default value.");
			}
		}

		protected void WriteIntToRegistry()
		{
			try
			{
                int optionInt = (int)this.regKey.RegOptionValue;
				System.Diagnostics.Debug.WriteLine("Writing to registry "+this.regKey.RegOptionName+" = "+Convert.ToString(optionInt));
				WinRegistry.SetHKCUValue(this.regKey.RegOptionKey,this.regKey.RegOptionName,optionInt);
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot write to registry for {0}"+this.regKey.RegOptionName+" = "+this.regKey.RegOptionValue);
			}
		}

		/// <summary>
		/// Store the value read from registry in this.regValue.
		/// If got error, the optionValue is set to default value
		/// </summary>
		protected void ReadEnumFromRegistry()
		{
			try
			{
				string enumValue = WinRegistry.GetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                if (enumValue != null)
                {
                    this.regKey.RegOptionValue = Enum.Parse(this.regKey.RegOptionType, enumValue);
                    System.Diagnostics.Debug.WriteLine("Option for " + this.regKey.RegOptionName + " in registry is: " + Convert.ToString(this.regKey.RegOptionValue));
                }
                else
                {
                    this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                    System.Diagnostics.Debug.WriteLine("Cant find option from registry for " + this.regKey.RegOptionName + ". Using default value");
		
                }
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot read option from registry for "+this.regKey.RegOptionName+". Using default value");
			}

		}

		protected void WriteEnumToRegistry()
		{
			try
			{
				string enumString = ((Enum)this.regKey.RegOptionValue).ToString();
				System.Diagnostics.Debug.WriteLine("Writing to registry "+this.regKey.RegOptionName+" = "+enumString);
				WinRegistry.SetHKCUValue(this.regKey.RegOptionKey,this.regKey.RegOptionName  ,enumString);
			}
			catch
			{
				this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
				System.Diagnostics.Debug.WriteLine("Cannot write to registry for {0}"+this.regKey.RegOptionName+" = "+this.regKey.RegOptionValue);
			}
		}

        protected void ReadPointFromRegistry()
        {
            try
            {
                string pointString = WinRegistry.GetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName);
                if (pointString != null)
                {
                    string[] points = pointString.Split('-');
                    int pX = Int32.Parse(points[0]);
                    int pY = Int32.Parse(points[1]);
                    this.regKey.RegOptionValue = new Point(pX, pY);
                    System.Diagnostics.Debug.WriteLine("Option for " + this.regKey.RegOptionName + " in registry is: " + ((Point)this.regKey.RegOptionValue).ToString());
                }
                else
                {
                    this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                    System.Diagnostics.Debug.WriteLine("Cant find option from registry for " + this.regKey.RegOptionName + ". Using default value");
                }
            }
            catch
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("Cannot read option from registry for " + this.regKey.RegOptionName + ". Using default value");
            }

        }

        protected void WritePointToRegistry()
        {
            try
            {
                Point thePoint = (Point)this.regKey.RegOptionValue;
                string stringPoint = String.Format("{0}-{1}", thePoint.X, thePoint.Y);
                System.Diagnostics.Debug.WriteLine("Writing to registry " + this.regKey.RegOptionName + " = " + stringPoint);
                WinRegistry.SetHKCUValue(this.regKey.RegOptionKey, this.regKey.RegOptionName, stringPoint);
            }
            catch
            {
                this.regKey.RegOptionValue = this.regKey.RegDefaultValue;
                System.Diagnostics.Debug.WriteLine("Cannot write to registry for {0}" + this.regKey.RegOptionName + " = " + this.regKey.RegOptionValue);
            }
        }

	}
}
