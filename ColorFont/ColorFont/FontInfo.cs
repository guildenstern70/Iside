using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorFont
{
    [Serializable]
    public class FontInfo
    {
        public FontFamily Family { get; set; }
        public double Size { get; set; }
        public FontStyle Style { get; set; }
        public FontStretch Stretch { get; set; }
        public FontWeight Weight { get; set; }
        public SolidColorBrush BrushColor { get; set; }

        #region Static Utils

        public static string TypefaceToString(FamilyTypeface ttf)
        {
            StringBuilder sb = new StringBuilder(ttf.Stretch.ToString());
            sb.Append("-");
            sb.Append(ttf.Weight.ToString());
            sb.Append("-");
            sb.Append(ttf.Style.ToString());
            return sb.ToString();
        }

        public static void ApplyFont(Control control, FontInfo font)
        {
            control.FontFamily = font.Family;
            control.FontSize = font.Size;
            control.FontStyle = font.Style;
            control.FontStretch = font.Stretch;
            control.FontWeight = font.Weight;
            control.Foreground = font.BrushColor;
        }

        public static FontInfo GetControlFont(Control control)
        {
            FontInfo font = new FontInfo();
            font.Family = control.FontFamily;
            font.Size = control.FontSize;
            font.Style = control.FontStyle;
            font.Stretch = control.FontStretch;
            font.Weight = control.FontWeight;
            font.BrushColor = (SolidColorBrush)control.Foreground;
            return font;
        }
        #endregion

        public FontInfo()
        {
        }

        //Deserialization constructor.
        public FontInfo(SerializationInfo info, StreamingContext ctxt)
        {
            Family = (FontFamily)info.GetValue("FontInfo_Family", typeof(FontFamily));
            Size = (double)info.GetValue("FontInfo_Size", typeof(double));
            Style = (FontStyle)info.GetValue("FontInfo_Style", typeof(FontStyle));
            Stretch = (FontStretch)info.GetValue("FontInfo_Stretch", typeof(FontStretch));
            Weight = (FontWeight)info.GetValue("FontInfo_Weight", typeof(FontWeight));
            BrushColor = (SolidColorBrush)info.GetValue("FontInfo_BrushColor", typeof(SolidColorBrush));
        }

        public FontInfo(FontFamily fam, double sz, FontStyle style, 
                        FontStretch strc, FontWeight weight, SolidColorBrush c)
        {
            this.Family = fam;
            this.Size = sz;
            this.Style = style;
            this.Stretch = strc;
            this.Weight = weight;
            this.BrushColor = c;
        }

        public FontColor Color
        {
            get
            {
                return AvailableColors.GetFontColor(this.BrushColor);
            }
        }

        public FamilyTypeface Typeface
        {
            get
            {
                FamilyTypeface ftf = new FamilyTypeface();
                ftf.Stretch = this.Stretch;
                ftf.Weight = this.Weight;
                ftf.Style = this.Style;
                return ftf;
            }
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("FontInfo_Family", Family);
            info.AddValue("FontInfo_Size", Size);
            info.AddValue("FontInfo_Style", Style);
            info.AddValue("FontInfo_Stretch", Stretch);
            info.AddValue("FontInfo_Weight", Weight);
            info.AddValue("FontInfo_BrushColor", BrushColor);
        }


        public void Serialize(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Deleting existing "+ filePath);
                    File.Delete(filePath);
                }

                using (StreamWriter stream = File.CreateText(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Serializing FontInfo " + this.GetHashCode().ToString());
                    string contents = System.Windows.Markup.XamlWriter.Save(this);
                    stream.WriteLine(contents);
                }
            }
            catch (IOException ioex)
            {
                System.Diagnostics.Debug.WriteLine("Error in serializing FontInfo: "+ ioex.Message);
            }
        }

        public static FontInfo Deserialize(string filePath)
        {
            FontInfo fi = null;

            try
            {
                using (StreamReader stream = new StreamReader(filePath))
                {
                    System.Diagnostics.Debug.WriteLine("Deserializing FontInfo from " + filePath);
                    string xaml = stream.ReadToEnd();
                    fi = System.Windows.Markup.XamlReader.Parse(xaml) as FontInfo;
                }
            }
            catch (IOException ioex)
            {
                System.Diagnostics.Debug.WriteLine("Error in deserializing FontInfo: " + ioex.Message);
            }

            return fi;

        }

    }
}
