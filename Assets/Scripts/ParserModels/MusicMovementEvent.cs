using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIDIparser.Models
{
    [XmlInclude(typeof(MusicEventBase))]
    public class MusicMovementEvent : MusicEventBase
    {
        public MusicMovementEvent()
        {

        }
        public MusicMovementEvent(EventTypeEnum eventType, long startTimeTick, long durationTick)
        {
            this.EventTypeID = eventType;
            this.StartTime = startTimeTick;
            this.Duration = durationTick;
        }
        public MusicMovementEvent(EventTypeEnum eventType, long startTimeTick)
        {
            this.EventTypeID = eventType;
            this.StartTime = startTimeTick;

            if(EventTypeID > EventTypeEnum.ArrowDownInstant)
            {
                throw new Exception("Only instant movement event can be created without duration.");
            }
        }

    }
}
