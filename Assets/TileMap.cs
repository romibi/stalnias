using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour {
    public int size_x = 20;
    public int size_y = 10;
    public float tileSize = 1f;
    
    Texture2D _mapTileset;
    public Texture2D[] tileTextures;
    public int textureResolution = 32;

	// Use this for initialization
	void Start () {
        BuildMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuildMap() {
        BuildTexture();
        BuildTileMap();
    }

    void PrepareTileset() {
        Dictionary<int, Color[]> tiles = new Dictionary<int, Color[]>();
        int gid = 0;
        foreach(Texture2D tileset in tileTextures) {
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
        _mapTileset = new Texture2D(textureResolution*tilesetwidth, textureResolution*tilesetwidth);
        
        for(int tile=0; tile<gid;tile++) {
            int x = tile % tilesetwidth;
            int y = tile / tilesetwidth;
            _mapTileset.SetPixels(x * textureResolution, y * textureResolution, textureResolution, textureResolution, tiles[tile]);
        }

        _mapTileset.filterMode = FilterMode.Point;
        _mapTileset.Apply();
    }
    
    public void BuildTexture() {
        PrepareTileset();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = _mapTileset;
    }

    Vector2[] getUVForTileType(int id) {
        int tilesetwidth = _mapTileset.width / textureResolution;

        int x = id % tilesetwidth;
        int y = id / tilesetwidth;

        Vector2[] uv = new Vector2[4];
        uv[0]=new Vector2((float)x/tilesetwidth, (float)y/ tilesetwidth);
        uv[1] = new Vector2((float)(x + 1) / tilesetwidth, (float)y / tilesetwidth);
        uv[2] = new Vector2((float)x / tilesetwidth, (float)(y+1) / tilesetwidth);
        uv[3] = new Vector2((float)(x + 1) / tilesetwidth, (float)(y+1) / tilesetwidth);

        return uv;
    }

    public void BuildTileMap() {
        int numTiles = size_x * size_y;
        int numVertices = numTiles*4;

        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangleVertices = new int[numTiles * 2 * 3]; // numTiles * numTrianglesPerTile * numVerticesPerTriangle
        
        for(int y=0; y < size_y; y++) {
            for(int x=0; x < size_x; x++) {
                int verticeTileIndex = (y * size_x + x) * 4;
                Vector2[] tileuv = getUVForTileType(15);
                for (int v = 0; v < 4; v++) {
                    int vx = v%2;
                    int vy = v/2;
                    
                    vertices[verticeTileIndex + v] = new Vector3((x+vx) * tileSize, (y+vy) * tileSize);
                    normals[verticeTileIndex + v] = Vector3.forward;
                    uv[verticeTileIndex + v] = tileuv[v];
                }

                int firstTriangleEdgeOfTile = (y * size_x + x) * 2 * 3;     // (tileRow * tileRowSize + tileInRow) * numTrianglesPerTile * numVerticesPerTriangle
                
                triangleVertices[firstTriangleEdgeOfTile + 0] = verticeTileIndex;
                triangleVertices[firstTriangleEdgeOfTile + 1] = verticeTileIndex + 2;
                triangleVertices[firstTriangleEdgeOfTile + 2] = verticeTileIndex + 3;

                triangleVertices[firstTriangleEdgeOfTile + 3] = verticeTileIndex;
                triangleVertices[firstTriangleEdgeOfTile + 4] = verticeTileIndex + 3;
                triangleVertices[firstTriangleEdgeOfTile + 5] = verticeTileIndex + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangleVertices;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
    }
}
