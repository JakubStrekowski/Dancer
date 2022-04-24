using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace MIDIparser.Models
{
    [Serializable]
    public class ArgbColor
    {

        [XmlElement("Alpha")]
        public byte alpha;
        [XmlElement("Red")]
        public byte red;
        [XmlElement("Green")]
        public byte green;
        [XmlElement("Blue")]
        public byte blue;

        public ArgbColor()
        {

        }
        public ArgbColor(byte a, byte r, byte g, byte b)
        {
            alpha = a;
            red = r;
            green = g;
            blue = b;
        }

        public Color ToUnityColor()
        {
            return new Color(
                red / 255f, 
                green / 255f, 
                blue / 255f, 
                alpha / 255f);
        }

        public static Color ConvertFromBytes(byte a, byte r, byte g, byte b)
        {
            return new Color(
                r / 255f, 
                g / 255f, 
                b / 255f, 
                a / 255f);
        }
        public static Color ConvertFromBytes(ArgbColor argb)
        {
            return new Color(
                argb.red / 255f, 
                argb.green / 255f, 
                argb.blue / 255f, 
                argb.alpha / 255f);
        }
    }
}
