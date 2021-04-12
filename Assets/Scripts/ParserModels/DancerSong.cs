
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
    public class DancerSong
    {
        [XmlElement("MusicFilePath")]
        public string musicFilePath; //path to a music file
        [XmlElement("ImagePreviewPath")]
        public string imagePreviewPath; //path to a preview image
        [XmlElement("AuthorAndTitle")]
        public string title; //song title
        [XmlElement("Description")]
        public string additionaldesc; //song additional description
        [XmlElement("DancerEvents")]
        public DancerEvents dancerEvents;

        public DancerSong()
        {
            dancerEvents = new DancerEvents();
        }

        public DancerSong(DancerEvents dancerEvents,string title, string description, string pathToMusic, string pathToImage)
        {
            this.dancerEvents = dancerEvents;
            this.title = title;
            this.additionaldesc = description;
            this.musicFilePath = pathToMusic;
            this.imagePreviewPath = pathToImage;
        }
    }
}
