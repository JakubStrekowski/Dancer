using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIDIparser.Models
{
    public enum EventTypeEnum
    {
        ArrowUpInstant,
        ArrowRightInstant,
        ArrowLeftInstant,
        ArrowDownInstant,
        ArrowUpDuration,
        ArrowRightDuration,
        ArrowLeftDuration,
        ArrowDownDuration,

        ChangeBackground,
    }

    [Serializable]
    public abstract class MusicEventBase
    {
        private EventTypeEnum eventTypeID;
        private long startTime;
        private long duration;

        [XmlElement("eventTypeID")]
        public EventTypeEnum EventTypeID { get => eventTypeID; set => eventTypeID = value; }
        [XmlElement("startTime")]
        public long StartTime { get => startTime; set => startTime = value; }
        [XmlElement("duration")]
        public long Duration { get => duration; set => duration = value; }

        public void RecalculateTime(int scale, long moveTapThreshold)
        {
            this.Duration *= scale;
            this.StartTime *= scale;
            if (Duration <= moveTapThreshold)
            {
                switch (this.EventTypeID)
                {
                    case EventTypeEnum.ArrowDownDuration:
                        EventTypeID = EventTypeEnum.ArrowDownInstant;
                        break;
                    case EventTypeEnum.ArrowLeftDuration:
                        EventTypeID = EventTypeEnum.ArrowLeftInstant;
                        break;
                    case EventTypeEnum.ArrowRightDuration:
                        EventTypeID = EventTypeEnum.ArrowRightInstant;
                        break;
                    case EventTypeEnum.ArrowUpDuration:
                        EventTypeID = EventTypeEnum.ArrowUpInstant;
                        break;
                }
                Duration = 0;
            }
        }
    }
}
