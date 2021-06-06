using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIDIparser.Models
{
    public enum VisualEventTypeEnum
    {
        CreateObject,
        DeleteObject,
        ChangeColorObjectLinear,
        ChangeColorObjectArc,
        ChangePosObjectLinear,
        ChangePosObjectArc,
        ChangeRotObjectLinear,
        ChangeRotObjectArc,
        ChangeSprite
    }

    [Serializable]
    public abstract class VisualEventBase
    {
        [XmlElement("eventTypeID")]
        public VisualEventTypeEnum eventType;

        [XmlElement("objectId")]
        public int objectId;

        [XmlElement("startTime")]
        public long startTime;

        [XmlElement("paramsList")]
        public List<string> paramsList;

        public virtual string ListRepresentation
        {
            get { return objectId.ToString() + " " + eventType.ToString() + " " + startTime.ToString(); }
        }
    }
}
