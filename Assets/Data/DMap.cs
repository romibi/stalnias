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
                    tilesets.Add(new DTileSet(mapChild));
                    break;
                case "layer":
                    layers.Add(new DMapLayerTiles(mapChild));
                    break;
                case "objectgroup":
                    layers.Add(new DMapLayerObjects(mapChild, width, height));
                    break;
            }
        }
        
        /* 
        tilesets.Add(new DTileSet("grass",3,18));
        tilesets.Add(new DTileSet("watergrass", 3, 18, 19));

        getTilesetForId(20).setCollisionForTile(20, new List<Vector2[]> { new Vector2[6] { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, .5f), new Vector2(.5f,.5f), new Vector2(.5f,0f) } }, true);
        getTilesetForId(21).setCollisionForTile(21, new List<Vector2[]> { new Vector2[6] { new Vector2(0f, .5f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), new Vector2(.5f, 0f), new Vector2(.5f, .5f) } }, true);
        getTilesetForId(23).setCollisionForTile(23, new List<Vector2[]> { new Vector2[6] { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(.5f, 1f), new Vector2(.5f, .5f), new Vector2(1f, .5f), new Vector2(1f, 0f) } }, true);
        getTilesetForId(24).setCollisionForTile(24, new List<Vector2[]> { new Vector2[6] { new Vector2(0f, 0f), new Vector2(0f, .5f), new Vector2(.5f, .5f), new Vector2(.5f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f) } }, true);
        getTilesetForId(25).setCollisionForTile(25, new List<Vector2[]> { new Vector2[4] { new Vector2(0.5f, 0f), new Vector2(.6f, .4f), new Vector2(1f, .5f), new Vector2(1f, 0f) } }, true);
        getTilesetForId(26).setCollisionForTile(26, new List<Vector2[]> { new Vector2[4] { new Vector2(0f, 0f), new Vector2(0f, .5f), new Vector2(1f, .5f), new Vector2(1f, 0f) } }, true);
        getTilesetForId(27).setCollisionForTile(27, new List<Vector2[]> { new Vector2[4] { new Vector2(0f, 0f), new Vector2(0f, .5f), new Vector2(.4f, .4f), new Vector2(.5f, 0f) } }, true);
        getTilesetForId(28).setCollisionForTile(28, new List<Vector2[]> { new Vector2[4] { new Vector2(.5f, 0f), new Vector2(.5f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f) } }, true);
        getTilesetForId(29).setTileFullCollision(29, true, true);
        getTilesetForId(30).setCollisionForTile(30, new List<Vector2[]> { new Vector2[4] { new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(.5f, 1f), new Vector2(.5f, 0f) } }, true);
        getTilesetForId(31).setCollisionForTile(31, new List<Vector2[]> { new Vector2[4] { new Vector2(.6f, .6f), new Vector2(.5f, 1f), new Vector2(1f, 1f), new Vector2(1f, .5f) } }, true);
        getTilesetForId(32).setCollisionForTile(32, new List<Vector2[]> { new Vector2[4] { new Vector2(0f, .5f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, .5f) } }, true);
        getTilesetForId(33).setCollisionForTile(33, new List<Vector2[]> { new Vector2[4] { new Vector2(0f, .5f), new Vector2(0f, 1f), new Vector2(.5f, 1f), new Vector2(.4f, .6f) } }, true);
        
        layers.Add(new DMapLayerTiles("bg", width, height, new int[] {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16}));

        layers.Add(new DMapLayerTiles("something", width, height, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0,25,26,27, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0,25,26,26,24,29,23,27, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0,28,29,29,20,32,21,30, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0,28,29,20,33, 0,31,33, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0,31,32,33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}));
        layers.Add(new DMapLayerObjects("Living", width, height));
        */
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
            if (set.firstgid < id && set.firstgid + set.count >= id) {
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
