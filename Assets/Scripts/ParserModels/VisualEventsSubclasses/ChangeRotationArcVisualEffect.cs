using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIparser.Models.VisualEventsSubclasses
{
    public class ChangeRotationArcVisualEffect : VisualEventBase
    {
        public ChangeRotationArcVisualEffect()
        {

        }

        public ChangeRotationArcVisualEffect(int objectID, long startTime, VisualEventTypeEnum type, long duration, float roation)
        {
            this.objectId = objectID;
            this.startTime = startTime;
            this.eventType = type;
            this.paramsList = new List<string>
            {
                duration.ToString(),
                roation.ToString(),
            };
        }

        public override string ListRepresentation
        {
            get
            {
                return objectId.ToString() + " " + startTime.ToString() + "-" + (startTime + float.Parse(paramsList[(int)ChangePositionLinearParamsEnum.duration]))
                    + " ROT -> " + this.paramsList[(int)ChangePotationLinearParamsEnum.rotation];
            }
        }
    }
}
