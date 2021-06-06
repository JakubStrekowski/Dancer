using MIDIparser.Models.VisualEventsSubclasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIDIparser.Models
{
    [Serializable]
    [XmlRoot("DancerSong")]
    [XmlInclude(typeof(MusicEventBase))]
    [XmlInclude(typeof(MusicMovementEvent))]
    [XmlInclude(typeof(DancerEvents))]
    [XmlInclude(typeof(ArgbColor))]
    [XmlInclude(typeof(VisualEventBase))]
    [XmlInclude(typeof(CreateDeleteVisualEvent))]
    [XmlInclude(typeof(ChangeColorLinearVisualEffect))]
    [XmlInclude(typeof(ChangeColorArcVisualEffect))]
    [XmlInclude(typeof(ChangePositionLinearVisualEffect))]
    [XmlInclude(typeof(ChangePositionDampingVisualEffect))]
    [XmlInclude(typeof(ChangeRotationLinearVisualEffect))]
    [XmlInclude(typeof(ChangeRotationArcVisualEffect))]
    [XmlInclude(typeof(ChangeSpriteVisualEffect))]
    public class DancerSong
    {
        //general info
        [XmlElement("MusicFilePath")]
        public string musicFilePath; //path to a music file
        [XmlElement("ImagePreviewPath")]
        public string imagePreviewPath; //path to a preview image
        [XmlElement("AuthorAndTitle")]
        public string title; //song title
        [XmlElement("Description")]
        public string additionaldesc; //song additional description
        [XmlElement("TicksPerSecond")]
        public int ticksPerSecond; //how many midi time units counts as a second

        //color scheme
        [XmlElement("UpArrowColor")]
        public ArgbColor upArrowColor;
        [XmlElement("RightArrowColor")]
        public ArgbColor rightArrowColor;
        [XmlElement("LeftArrowColor")]
        public ArgbColor leftArrowColor;
        [XmlElement("DownArrowColor")]
        public ArgbColor downArrowColor;

        [XmlElement("BackgroundColor")]
        public ArgbColor backgroundColor;
        [XmlElement("UiColor")]
        public ArgbColor uiColor;
        [XmlElement("UiTextColor")]
        public ArgbColor uiTextColor;

        //events
        [XmlElement("DancerEvents")]
        public DancerEvents dancerEvents;

        public DancerSong()
        {
            dancerEvents = new DancerEvents();
        }

        public DancerSong(DancerEvents dancerEvents, string title, string description, int ticksPerSecond, string pathToMusic, string pathToImage, ArgbColor[] colorSettings)
        {
            this.dancerEvents = dancerEvents;
            this.title = title;
            this.additionaldesc = description;
            this.ticksPerSecond = ticksPerSecond;
            this.musicFilePath = pathToMusic;
            this.imagePreviewPath = pathToImage;

            if(colorSettings.Length == 7)
            {
                upArrowColor    = colorSettings[0];
                rightArrowColor = colorSettings[1];
                leftArrowColor  = colorSettings[2];
                downArrowColor  = colorSettings[3];
                backgroundColor = colorSettings[4];
                uiColor         = colorSettings[5];
                uiTextColor     = colorSettings[6];
            }
        }
    }
}
