using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIDIparser.Models;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.Networking;
using System;

public class MusicLoader
{
    public DancerSong DancerSongParsed {get; private set; }
    AudioClip newMusic;

    private string _musicPath = Application.dataPath+"/Resources/Music/";

    public void LoadMusicMoves(string fileName = "Test/test.xml")
    {
        if (File.Exists(_musicPath + fileName))
        {
            System.IO.TextReader reader = new StreamReader(_musicPath + fileName);
            XmlSerializer xml = new XmlSerializer(typeof(DancerSong));
            DancerSongParsed = xml.Deserialize(reader) as DancerSong;
            reader.Close();
        }
        else
        {
            throw new System.Exception(fileName + " not found");
        }
    }


    public IEnumerator GetAudioClipFromFile(string folderName, Action<AudioClip> callback)
    {
        if (callback == null) yield break;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://"+ _musicPath + '/'+ folderName + '/' + DancerSongParsed.musicFilePath, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                callback(null);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                myClip.name = DancerSongParsed.musicFilePath;
                callback(myClip);
            }
        }
    }



}
