using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileMap : MonoBehaviour {
    Dictionary<int,GameObject> _layers = new Dictionary<int, GameObject>();

    List<Texture2D> tileTextures = new List<Texture2D>();
    int textureResolution = 32;

    public Material material;
    public Texture2D mapTileset;

    // Use this for initialization
    void Start() {
        LoadLayers();
    }

    public void reloadLayers() {
        Dictionary<int, GameObject> copy = new Dictionary<int, GameObject>(_layers);
        foreach (KeyValuePair<int,GameObject> layer in copy) {
            _layers.Remove(layer.Key);
            DestroyImmediate(layer.Value);
        }
        tileTextures.Clear();
        LoadLayers();
    }

    void LoadLayers() {
        Dictionary<int, int[][]> layers = new Dictionary<int, int[][]>();
        layers.Add(0, new int[][]{  new int[] { 1,1,1,1,1,1,1,1 },
                            new int[] { 1,1,1,1,1,1,1,1 },
                            new int[] { 1,1,1,1,1,1,1,1 },
                            new int[] { 1,1,1,1,1,1,1,1 },
                            new int[] { 1,1,1,1,1,1,1,1 } });
        layers.Add(1, new int[][]{  new int[] { 0,0,0,0,0,0,0,0 },
                            new int[] { 0,0,0,2,2,0,0,0 },
                            new int[] { 0,0,2,2,2,2,2,0 },
                            new int[] { 0,0,0,2,2,0,0,0 },
                            new int[] { 0,0,0,0,0,0,0,0 } });

        Texture2D tex = Resources.Load("textures/grass") as Texture2D;
        tileTextures.Add(tex);
        PrepareTileset();

        foreach (KeyValuePair<int, int[][]> layer in layers) {
            GameObject tileLayer = new GameObject(layer.Key.ToString(), typeof(TileLayer));
            tileLayer.transform.parent = this.transform;
            TileLayer tlcomp = tileLayer.GetComponent<TileLayer>();
            tlcomp.layerdata = layer.Value;
            
            _layers.Add(layer.Key, tileLayer);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void PrepareTileset() {
        Dictionary<int, Color[]> tiles = new Dictionary<int, Color[]>();
        int gid = 0;
        foreach (Texture2D tileset in tileTextures) {
            int numTilesPerRow = tileset.width / textureResolution;
            int numRows = tileset.height / textureResolution;

            for (int id = 0; id < numRows * numTilesPerRow; id++, gid++) {
                int row = id / numTilesPerRow;
                int idOfRow = id - (numTilesPerRow * row);

                int invertedRow = numRows - row - 1;

                tiles.Add(gid, tileset.GetPixels(idOfRow * textureResolution, invertedRow * textureResolution, textureResolution, textureResolution));
            }
        }

        int tilesetwidth = Mathf.CeilToInt(Mathf.Sqrt(gid));
        mapTileset = new Texture2D(textureResolution * tilesetwidth, textureResolution * tilesetwidth);

        for (int tile = 0; tile < gid; tile++) {
            int x = tile % tilesetwidth;
            int y = tile / tilesetwidth;
            mapTileset.SetPixels(x * textureResolution, y * textureResolution, textureResolution, textureResolution, tiles[tile]);
        }

        mapTileset.filterMode = FilterMode.Point;
        mapTileset.Apply();
    }
}
