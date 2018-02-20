using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;

public class DMap {
    public int width = 20;
    public int height = 10;
    public int tileresolution = 32;

    public List<DTileSet> tilesets;
    public List<DMapLayer> layers;

    public DMap(string mapId) {
        XmlDocument mapData = loadMapData(mapId);
        XmlNode mapTag = mapData.GetElementsByTagName("map")[0];

        int.TryParse(mapTag.Attributes["width"].Value, out width);
        int.TryParse(mapTag.Attributes["height"].Value, out height);
        String wres = mapTag.Attributes["tilewidth"].Value;
        String hres = mapTag.Attributes["tileheight"].Value;
        if (wres.Equals(hres))
            int.TryParse(wres, out tileresolution);

        tilesets = new List<DTileSet>();
        layers = new List<DMapLayer>();

        foreach (XmlNode mapChild in mapTag.ChildNodes)
        {
            switch (mapChild.Name)
            {
                case "tileset":
                    tilesets.Add(new DTileSet(mapChild, tileresolution));
                    break;
                case "layer":
                    layers.Add(new DMapLayerTiles(mapChild));
                    break;
                case "objectgroup":
                    layers.Add(new DMapLayerObjects(mapChild, width, height));
                    break;
            }
        }
    }

    public DMap(int width, int height, List<DTileSet> tilesets, List<DMapLayer> layers, int tileresolution = 32) {
        this.width = width;
        this.height = height;
        this.tileresolution = tileresolution;
        this.tilesets = tilesets;
        this.layers = layers;
    }

    public DTileSet getTilesetForId(int id) {
        foreach (DTileSet set in tilesets) {
            if (set.firstgid <= id && set.firstgid + set.count > id) {
                return set;
            }
        }
        return null;
    }

    private XmlDocument loadMapData(String mapId)
    {
        String levelFileContent = "";
        String filePath = Application.persistentDataPath + "/levels/" + mapId + ".tmx";
        if (!File.Exists(filePath))
            filePath = Application.persistentDataPath + "/levels/" + mapId + ".xml";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/levels/" + mapId + ".tmx";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/levels/" + mapId + ".xml";

        if (File.Exists(filePath))
            levelFileContent = File.ReadAllText(filePath);
        else
            levelFileContent = (Resources.Load("levels/" + mapId) as TextAsset).text;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(levelFileContent);
        return xmlDoc;
    }
}
