using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDIparser.Models.VisualEventsSubclasses
{

    public class ChangeSpriteVisualEffect : VisualEventBase
    {
        public ChangeSpriteVisualEffect()
        {

        }
        public ChangeSpriteVisualEffect(
            int objectID, long startTime, 
            VisualEventTypeEnum type, string spritePath = null)
        {
            this.objectId = objectID;
            this.startTime = startTime;
            this.eventType = type;
            this.paramsList = new List<string> {spritePath};
        }
        public override string ListRepresentation
        {
            get
            {
                return objectId.ToString() + " " + 
                    eventType.ToString() + " " + 
                    startTime.ToString();
            }
        }
    }
}
