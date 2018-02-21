using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class AssetLoader
{
    public AssetLoader()
    {
        MapStatus = LoadStatus.Idle;
    }

    public enum LoadStatus { Idle, Loading, Finished }

    public LoadStatus MapStatus { get; set; }
    private DMap mapData;
    public DMap MapData {
        get {
            MapStatus = LoadStatus.Idle;
            return mapData;
        }
        set {
            mapData = value;
        }
    }

    public Dictionary<DTileSet, Texture2D> tilesetTextureMap = new Dictionary<DTileSet, Texture2D>();


    public bool MapFinished()
    {
        return MapStatus == LoadStatus.Finished;
    }

    public IEnumerator loadMapData(String mapId)
    {
        MapStatus = LoadStatus.Loading;
        String levelFileContent = "";
        String filePathTmx = Application.persistentDataPath + "/levels/" + mapId + ".tmx";
        String filePathXml = Application.persistentDataPath + "/levels/" + mapId + ".xml";

        if (!File.Exists(filePathTmx))
            filePathTmx = Application.streamingAssetsPath + "/levels/" + mapId + ".tmx";
        if (!File.Exists(filePathXml))
            filePathXml = Application.streamingAssetsPath + "/levels/" + mapId + ".xml";

        if (File.Exists(filePathTmx) || File.Exists(filePathXml))
        {
            if (File.Exists(filePathTmx))
                levelFileContent = File.ReadAllText(filePathTmx);
            else
                levelFileContent = File.ReadAllText(filePathXml);
        }
        else
        {
            WWW www = new WWW(filePathTmx);
            yield return www;
            if (www.error == null)
                levelFileContent = www.text;
            else
                levelFileContent = (Resources.Load("levels/" + mapId) as TextAsset).text;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(levelFileContent);
        MapData = new DMap(xmlDoc);
        IEnumerator loadTexture = loadTextureData();
        while (loadTexture.MoveNext()) yield return loadTexture.Current;
        MapStatus = LoadStatus.Finished;
    }

    public IEnumerator loadTextureData()
    {
        foreach (DTileSet ts in mapData.tilesets)
        {
            IEnumerator loadFromPath = loadTextureFromPath(ts);
            while (loadFromPath.MoveNext()) yield return loadFromPath.Current;
            if (!tilesetTextureMap.ContainsKey(ts) || tilesetTextureMap[ts]==null)
            {
                IEnumerator loadFromName = loadTextureFromName(ts);
                while (loadFromName.MoveNext()) yield return loadFromName.Current;
            }
        }
    }


    private IEnumerator loadTextureFromName(DTileSet ts)
    {
        string res_name = ts.res_name;
        String filePath = Application.persistentDataPath + "/textures/" + res_name + ".png";
        if (!File.Exists(filePath))
            filePath = Application.persistentDataPath + "/textures/" + res_name + ".jpg";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/textures/" + res_name + ".png";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/textures/" + res_name + ".jpg";
        bool ok = false;
        if (File.Exists(filePath))
        {
            WWW www = new WWW("file:///" + filePath);
            yield return www;
            if(www.error != null)
            {
                tilesetTextureMap.Add(ts, www.texture);
                ok = true;
            }
        }
        if (!ok)
        {
            WWW www = new WWW(filePath);
            yield return www;
            if (www.error == null)
            {
                tilesetTextureMap.Add(ts, Resources.Load("textures/" + res_name) as Texture2D);
            }
            tilesetTextureMap.Add(ts, www.texture);
        }
    }

    private IEnumerator loadTextureFromPath(DTileSet ts)
    {
        string res_path = ts.res_path;
        String filePath = Application.persistentDataPath + res_path;
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + res_path;
        bool ok = false;
        if (File.Exists(filePath))
        {
            WWW www = new WWW("file:///" + filePath);
            yield return www;
            if(www.error==null)
            {
                tilesetTextureMap.Add(ts, www.texture);
                ok = true;
            }
        }
        if (!ok)
        {
            WWW www = new WWW(filePath);
            yield return www;
            tilesetTextureMap.Add(ts, www.texture);
        }
    }

}