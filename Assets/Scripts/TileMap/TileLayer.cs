﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class TileLayer : MonoBehaviour {
    Texture2D _layerTexture;

    TileMap tilemap {
        get {
            return transform.parent.GetComponent<TileMap>();
        }
    }
    
    public int textureResolution = 32;

    public DMapLayerTiles layerdata;

    // Use this for initialization
    void Start () {
        BuildMap();
        generateLayerTexture();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void generateLayerTexture() {
        _layerTexture = new Texture2D(layerdata.width * textureResolution, layerdata.height * textureResolution,TextureFormat.RGBA32,false); //to avoid some texture bugs disable mipmap for non NPOT textures
        Color transparent = new Color(0, 0, 0, 0);
        Color[] allColors = _layerTexture.GetPixels();
        for(int c=0;c<allColors.Length;c++) {
            allColors[c] = transparent;
        }
        _layerTexture.SetPixels(allColors);

        for (int y=0; y < layerdata.height; y++) {
            for(int x=0; x < layerdata.width; x++) {
                if (layerdata.tileTypeIdAt(x, y) != 0) {
                    _layerTexture.SetPixels(x * textureResolution, y * textureResolution, textureResolution, textureResolution, tilemap.tiles[layerdata.tileTypeIdAt(x, y)]);
                }
            }
        }

        _layerTexture.wrapMode = TextureWrapMode.Clamp;
        _layerTexture.filterMode = FilterMode.Point;
        _layerTexture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials = new Material[] { new Material(tilemap.material) };
        mesh_renderer.sharedMaterials[0].mainTexture = _layerTexture;
    }

    public void BuildMap() {
        BuildTileMap();
    }

    public void BuildTileMap() {
        int size_x = layerdata.width;
        int size_y = layerdata.height;
        
        int numTiles = size_x * size_y;
        int numVertices = numTiles*4;

        Vector3[] vertices = new Vector3[numVertices];
        Vector3[] normals = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];
        int[] triangleVertices = new int[numTiles * 2 * 3]; // numTiles * numTrianglesPerTile * numVerticesPerTriangle

        List<Vector2[]> collisionPaths = new List<Vector2[]>();
        
        for(int y=0; y < size_y; y++) {
            for(int x=0; x < size_x; x++) {
                if (layerdata.tileTypeIdAt(x, y) == 0) continue;
                int verticeTileIndex = (y * size_x + x) * 4;
                for (int v = 0; v < 4; v++) {
                    int vx = v%2;
                    int vy = v/2;
                    
                    vertices[verticeTileIndex + v] = new Vector3((x+vx) * tilemap.tileSize, (y+vy) * tilemap.tileSize);
                    normals[verticeTileIndex + v] = Vector3.forward;
                    uv[verticeTileIndex + v] = new Vector2((float)(x+vx)/(layerdata.width), (float)(y + vy) / layerdata.height);
                }

                int tileId = layerdata.tileTypeIdAt(x, y);
                List<Vector2[]> collisions = tilemap.map.getTilesetForId(tileId).getCollisionForTile(tileId);
                if (collisions != null) {
                    foreach (Vector2[] collision in collisions) {
                        Vector2[] path = new Vector2[collision.Length];
                        for (int cv = 0; cv < collision.Length; cv++) {
                            path[cv] = (Vector2)(vertices[verticeTileIndex])+(collision[cv]*tilemap.tileSize);
                        }
                        collisionPaths.Add(path);
                    }
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
        
        PolygonCollider2D polygon_collider = GetComponent<PolygonCollider2D>();
        polygon_collider.enabled = false;
        polygon_collider.pathCount = collisionPaths.Count;
        int pathnum = 0;
        foreach(Vector2[] path in collisionPaths) {
            polygon_collider.SetPath(pathnum, path);
            pathnum++;
        }
        polygon_collider.enabled = true;

        mesh_filter.mesh = mesh;
    }
}
