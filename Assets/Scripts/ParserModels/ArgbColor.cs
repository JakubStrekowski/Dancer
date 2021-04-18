using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

    }
}
