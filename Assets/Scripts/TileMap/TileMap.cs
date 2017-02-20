﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
    Dictionary<string,GameObject> _layers = new Dictionary<string, GameObject>();
    
    int textureResolution = 32;

    public Material material;
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

    void LoadMap() {
        map = new DMap();
        
        foreach(DTileSet ts in map.tilesets) {
            LoadTileset(ts);
        }


        float z = 0.0f;
        foreach(DMapLayer l in map.layers) {
            if (l.GetType() == typeof(DMapLayerTiles)) {
                LoadLayers((DMapLayerTiles)l, z);
            }
            if(l.GetType() == typeof(DMapLayerObjects)) {
                if(l.name=="Living") {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, z);
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

    void LoadTileset(DTileSet ts) {
        if (_lastgid == 0) {
            Color[] emptytile = new Color[textureResolution * textureResolution];
            for (int x = 0; x < (textureResolution * textureResolution); x++) {
                emptytile[x] = new Color(0, 0, 0);
            }
            tiles.Add(0, emptytile);
        }

        Texture2D tex = Resources.Load("textures/" + ts.res_name) as Texture2D;
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
}
