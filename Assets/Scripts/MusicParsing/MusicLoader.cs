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

    private readonly string MUSIC_PATH = Application.dataPath+"/Resources/Music/";

    public void LoadMusicMoves(string folderName = "Test")
    {
        string fileName = folderName + '/' + folderName + ".xml";
        if (File.Exists(MUSIC_PATH + fileName))
        {
            System.IO.TextReader reader = new StreamReader(MUSIC_PATH + fileName);
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
        AudioType audioType = AudioType.WAV;
        if (DancerSongParsed.musicFilePath.EndsWith("mp3"))
        {
            audioType = AudioType.MPEG;
        }

        if (callback == null) yield break;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://"+ MUSIC_PATH + '/'+ folderName + '/' + DancerSongParsed.musicFilePath, audioType))
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


    public IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }



}
