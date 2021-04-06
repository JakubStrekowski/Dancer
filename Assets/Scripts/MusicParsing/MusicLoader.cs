using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIDIparser.Models;
using System.IO;
using System.Xml.Serialization;

public class MusicLoader
{
    public DancerEvents DancerEvents {get; private set;}

    private readonly string _musicPath = Application.dataPath+"/Resources/Music/";

    public void LoadMusicMoves(string fileName = "Test/test.xml")
    {
        if (File.Exists(_musicPath + fileName))
        {
            System.IO.TextReader reader = new StreamReader(_musicPath + fileName);
            XmlSerializer xml = new XmlSerializer(typeof(DancerEvents));
            DancerEvents = xml.Deserialize(reader) as DancerEvents;
            reader.Close();
        }
        else
        {
            throw new System.Exception(fileName + " not found");
        }
    }

}
