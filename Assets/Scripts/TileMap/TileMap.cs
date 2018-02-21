using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
    public bool prerender_textures = false;
    Dictionary<string,GameObject> _layers = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> _objects = new Dictionary<string, GameObject>();

    public int textureResolution = 32;
    int tilesetTexturePadding = 1; // currently supported values: 0 and 1

    public Material material;
    public Texture2D mapTileset;
    public float tileSize = 0.01f;
    public DMap map;
    

    int _lastgid = 0;
    public Dictionary<int, Color[]> tiles = new Dictionary<int, Color[]>();

    // Use this for initialization
    void Start() {
        LoadMap();
    }

    // Update is called once per frame
    void Update() {

    }

    public void reloadLayers() {
        ClearMap();
        LoadMap();
    }

    void ClearMap() {
        _layers.Clear();
        var mapchilds = transform.Cast<Transform>().ToList();
        foreach (var child in mapchilds) {
            DestroyImmediate(child.gameObject);
        }
        _lastgid = 0;
        tiles.Clear();
    }

    void LoadMap(String mapId="demoMap") {
        map = new DMap(mapId);
        
        foreach(DTileSet ts in map.tilesets) {
            LoadTileset(ts);
        }

        if(!prerender_textures) {
            PrepareTileset();
        }


        float z = 0.0f;
        foreach(DMapLayer l in map.layers) {
            if (l.GetType() == typeof(DMapLayerTiles)) {
                LoadLayers((DMapLayerTiles)l, z);
            }
            if(l.GetType() == typeof(DMapLayerObjects)) {
                DMapLayerObjects ol = (DMapLayerObjects)l;
                GameObject layerobject = GetObjectLayer(ol, z);
                foreach(DObject obj in ol.objectsById.Values)
                {
                    if (obj.name == "Player")
                    {
                        DObject dPlayer = obj;

                        GameObject player = GameObject.FindGameObjectWithTag("Player");
                        float newX = player.transform.position.x;
                        float newY = player.transform.position.y;
                        if (dPlayer != null)
                        {
                            Vector2 newPos = MapObjCenterInUPos(dPlayer);
                            newX = newPos.x;
                            newY = newPos.y;
                        }

                        player.transform.position = new Vector3(newX, newY, z);
                        SpriteRenderer sprite_renderer = player.GetComponent<SpriteRenderer>();
                        sprite_renderer.sortingOrder = dPlayer.sortingOrder;
                    }
                    else
                    {
                        LoadObject(obj, layerobject);
                    }
                }
            }
            z -= 0.1f;
        }
    }

    void LoadLayers(DMapLayerTiles l, float z) {
        GameObject tileLayer = new GameObject(l.name, typeof(TileLayer));
        tileLayer.hideFlags = HideFlags.DontSave;
        tileLayer.transform.parent = this.transform;
        tileLayer.transform.position = tileLayer.transform.position+new Vector3(0,0,z);
        TileLayer tlcomp = tileLayer.GetComponent<TileLayer>();
        tlcomp.layerdata = l;

        _layers.Add(l.name, tileLayer);
    }

    GameObject GetObjectLayer(DMapLayerObjects l, float z)
    {
        GameObject olayer = new GameObject(l.name);
        olayer.hideFlags = HideFlags.DontSave;
        olayer.transform.parent = this.transform;
        olayer.transform.position = olayer.transform.position + new Vector3(0, 0, z);
        return olayer;
    }

    void LoadObject(DObject o, GameObject parent)
    {
        GameObject ingameObject = new GameObject(o.name, typeof(IngameObject));
        ingameObject.hideFlags = HideFlags.DontSave;
        ingameObject.transform.parent = parent.transform;
        Vector2 v = MapPosToUPos(o.x, o.y);
        float scaleX = o.w / textureResolution;
        float scaleY = o.h / textureResolution;

        ingameObject.transform.position = ingameObject.transform.position + new Vector3(v.x, v.y, 0);
        ingameObject.transform.localScale = new Vector3(scaleX, scaleY, 1);
        IngameObject incomp = ingameObject.GetComponent<IngameObject>();
        incomp.objectData = o;

        _objects.Add(o.name, ingameObject);
    }

    void LoadTileset(DTileSet ts) {
        if (_lastgid == 0) {
            Color[] emptytile = new Color[textureResolution * textureResolution];
            for (int x = 0; x < (textureResolution * textureResolution); x++) {
                emptytile[x] = new Color(0, 0, 0);
            }
            tiles.Add(0, emptytile);
        }

        Texture2D tex = loadTextureFromPath(ts.res_path);
        if (tex == null)
            tex = loadTextureFromName(ts.res_name);

        if (textureResolution != (tex.width / ts.columns)) {
            Debug.LogWarning("tileset resolution not equal to map resolution");
        }

        if(_lastgid>ts.firstgid) {
            Debug.LogWarning("Tileset First GID "+ts.firstgid+" is smaller than the last used GID "+_lastgid);
        }
        _lastgid = ts.firstgid;
        
        int numRows = tex.height / textureResolution;

        for (int id = 1; id <= ts.count; id++, _lastgid++) {
            int row = (id-1) / ts.columns;
            int idOfRow = (id-1) - (ts.columns * row);

            int invertedRow = numRows - row - 1;
            
            tiles.Add(_lastgid, tex.GetPixels(idOfRow * textureResolution, invertedRow * textureResolution, textureResolution, textureResolution));
        }
    }

    private Texture2D loadTextureFromName(string res_name)
    {
        String filePath = Application.persistentDataPath + "/textures/" + res_name + ".png";
        if (!File.Exists(filePath))
            filePath = Application.persistentDataPath + "/textures/" + res_name + ".jpg";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/textures/" + res_name + ".png";
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath + "/textures/" + res_name + ".jpg";
        if (File.Exists(filePath))
            return new WWW("file:///" + filePath).texture;
        return Resources.Load("textures/" + res_name) as Texture2D;
    }

    private Texture2D loadTextureFromPath(string res_path)
    {
        String filePath = Application.persistentDataPath + res_path;
        if (!File.Exists(filePath))
            filePath = Application.streamingAssetsPath+ res_path;
        if(File.Exists(filePath))
            return new WWW("file:///" + filePath).texture;
        return Resources.Load(res_path.TrimStart('/')) as Texture2D;
    }

    void PrepareTileset() {
        int resWithPad = textureResolution + (2 * tilesetTexturePadding);
        int tilesetwidth = Mathf.CeilToInt(Mathf.Sqrt(_lastgid));
        int texturesize = Mathf.NextPowerOfTwo(tilesetwidth * resWithPad);
        mapTileset = new Texture2D(texturesize, texturesize, TextureFormat.RGBA32, false);
        
        for (int tile = 0; tile < _lastgid; tile++) {
            int x = tile % tilesetwidth;
            int y = tile / tilesetwidth;
            if (tiles.ContainsKey(tile)) {
                mapTileset.SetPixels((x * resWithPad)+tilesetTexturePadding, (y * resWithPad)+tilesetTexturePadding, textureResolution, textureResolution, tiles[tile]);

                if (tilesetTexturePadding != 0) {
                    Color[] bottom = mapTileset.GetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad + tilesetTexturePadding, textureResolution, tilesetTexturePadding);
                    Color[] top = mapTileset.GetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad + textureResolution, textureResolution, tilesetTexturePadding);
                    Color[] left = mapTileset.GetPixels((x * resWithPad) + tilesetTexturePadding, (y * resWithPad) + tilesetTexturePadding, tilesetTexturePadding, textureResolution);
                    Color[] right = mapTileset.GetPixels((x * resWithPad) + textureResolution, (y * resWithPad) + tilesetTexturePadding, tilesetTexturePadding, textureResolution);

                    Color[] bottomLeft = mapTileset.GetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad + tilesetTexturePadding, tilesetTexturePadding, tilesetTexturePadding);
                    Color[] topLeft = mapTileset.GetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad + textureResolution, tilesetTexturePadding, tilesetTexturePadding);
                    Color[] bottomRight = mapTileset.GetPixels((x * resWithPad) + textureResolution, (y * resWithPad) + tilesetTexturePadding, tilesetTexturePadding, tilesetTexturePadding);
                    Color[] topRight = mapTileset.GetPixels((x * resWithPad) + textureResolution, (y * resWithPad) + textureResolution, tilesetTexturePadding, tilesetTexturePadding);

                    mapTileset.SetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad, textureResolution, tilesetTexturePadding, bottom);
                    mapTileset.SetPixels(x * resWithPad + tilesetTexturePadding, y * resWithPad + tilesetTexturePadding + textureResolution, textureResolution, tilesetTexturePadding, top);
                    mapTileset.SetPixels(x * resWithPad, y * resWithPad + tilesetTexturePadding, tilesetTexturePadding, textureResolution, left);
                    mapTileset.SetPixels(x * resWithPad + tilesetTexturePadding + textureResolution, y * resWithPad + tilesetTexturePadding, tilesetTexturePadding, textureResolution, right);

                    mapTileset.SetPixels(x * resWithPad, y * resWithPad, tilesetTexturePadding,tilesetTexturePadding,bottomLeft);
                    mapTileset.SetPixels(x * resWithPad+textureResolution+tilesetTexturePadding, y * resWithPad, tilesetTexturePadding, tilesetTexturePadding, bottomRight);
                    mapTileset.SetPixels(x * resWithPad, y * resWithPad + textureResolution + tilesetTexturePadding, tilesetTexturePadding, tilesetTexturePadding, topLeft);
                    mapTileset.SetPixels(x * resWithPad + textureResolution + tilesetTexturePadding, y * resWithPad + textureResolution + tilesetTexturePadding, tilesetTexturePadding, tilesetTexturePadding, topRight);
                }
            }
        }

        mapTileset.wrapMode = TextureWrapMode.Clamp;
        mapTileset.filterMode = FilterMode.Point;
        mapTileset.Apply();
        material.mainTexture = mapTileset;
    }

    public Vector2[] getUVForTileType(int id) {
        int usedtilesetcolumns = Mathf.CeilToInt(Mathf.Sqrt(_lastgid));
        int paddedTileRes = textureResolution + (2 * tilesetTexturePadding);
        float realtilesetcolumns = ((float)mapTileset.width) / paddedTileRes;

        float paddingProcent = (float)tilesetTexturePadding / mapTileset.width;
        //paddingProcent = 0; // to debug tileset padding generation

        int x = id % usedtilesetcolumns;
        int y = id / usedtilesetcolumns;
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(((float)x / realtilesetcolumns)+paddingProcent, ((float)y / realtilesetcolumns)+paddingProcent);
        uv[1] = new Vector2(((float)(x + 1) / realtilesetcolumns)-paddingProcent, ((float)y / realtilesetcolumns) + paddingProcent);
        uv[2] = new Vector2(((float)x / realtilesetcolumns) + paddingProcent, ((float)(y + 1) / realtilesetcolumns)-paddingProcent);
        uv[3] = new Vector2(((float)(x + 1) / realtilesetcolumns) - paddingProcent, ((float)(y + 1) / realtilesetcolumns)- paddingProcent);
        return uv;
    }

    public Vector2 MapPosToUPos(float x, float y)
    {
        float newX = x * tileSize / textureResolution;
        float newY = (map.height * textureResolution - y) * tileSize / textureResolution;
        return new Vector2(newX, newY);
    }

    public Vector2 MapObjCenterInUPos(DObject obj)
    {
        Vector2 pos = MapPosToUPos(obj.x, obj.y);
        float width = obj.w * tileSize / textureResolution;
        float height = obj.h * tileSize / textureResolution;
        pos.x += width / 2;
        pos.y -= height / 2;
        return pos;
    }
}
