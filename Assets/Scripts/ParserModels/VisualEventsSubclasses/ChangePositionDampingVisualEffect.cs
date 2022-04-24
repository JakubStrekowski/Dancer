using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIparser.Models.VisualEventsSubclasses
{
    public class ChangePositionDampingVisualEffect : VisualEventBase
    {
        public ChangePositionDampingVisualEffect()
        {

        }

        public ChangePositionDampingVisualEffect(
            int objectID, long startTime, 
            VisualEventTypeEnum type, long duration, float posX, float posY)
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

        public override string ListRepresentation
        {
            get
            {
                return objectId.ToString() + " " + startTime.ToString() + "-" + 
                    (startTime + float.Parse(paramsList[(int)ChangePositionLinearParamsEnum.duration]))
                    + " POS -> [" + this.paramsList[(int)ChangePositionLinearParamsEnum.posX] + 
                    "," + this.paramsList[(int)ChangePositionLinearParamsEnum.posY] + "]";
            }
        }

    }
}
