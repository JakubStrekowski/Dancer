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


    public string musicPath;

    public void LoadMusicMoves(string folderName = "AVGVSTA - Together Again")
    {
        string fileName = folderName + '/' + folderName + ".xml";
        if (File.Exists(musicPath + fileName))
        {
            System.IO.TextReader reader = new StreamReader(musicPath + fileName);
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
        if (DancerSongParsed.musicFilePath.EndsWith("ogg"))
        {
            audioType = AudioType.OGGVORBIS;
        }

        if (callback == null) yield break;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(
            "file://"+ musicPath + '/'+ folderName + '/' + DancerSongParsed.musicFilePath, 
            audioType))
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
