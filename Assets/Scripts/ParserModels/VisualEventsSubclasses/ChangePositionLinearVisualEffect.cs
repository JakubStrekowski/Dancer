using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIparser.Models.VisualEventsSubclasses
{
    public enum ChangePositionLinearParamsEnum
    {
        duration = 0,
        posX,
        posY,
    }

    public class ChangePositionLinearVisualEffect : VisualEventBase
    {
        public ChangePositionLinearVisualEffect()
        {

        }

        public ChangePositionLinearVisualEffect(int objectID, long startTime, VisualEventTypeEnum type, long duration, float posX, float posY)
        {
            this.objectId = objectID;
            this.startTime = startTime;
            this.eventType = type;
            this.paramsList = new List<string>
            {
                duration.ToString(),
                posX.ToString(),
                posY.ToString()
            };
        }
    }
}
