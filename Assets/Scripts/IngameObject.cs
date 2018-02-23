using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class IngameObject : MonoBehaviour {
    private DObject objectData;
    public DObject ObjectData {
        get {
            return objectData;
        }
        set {
            objectData = value;
            parseObjectData();
        }

    }
    private List<Vector2[]> collisionbox = null;

    TileMap tilemap {
        get {
            return transform.parent.transform.parent.GetComponent<TileMap>();
        }
    }

    // Use this for initialization
    void Start () {
        parseObjectData();
	}

    private void parseObjectData()
    {
        collisionbox = null;
        if (objectData.gid > 0)
        {
            SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
            sprite_renderer.sortingOrder = objectData.sortingOrder;

            Color[] textureColors = tilemap.tiles[objectData.gid];

            int size = tilemap.textureResolution;

            Texture2D texture = new Texture2D(size, size);
            texture.SetPixels(textureColors);
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            sprite_renderer.sprite = sprite;

            PolygonCollider2D polygon_collider = GetComponent<PolygonCollider2D>();
            polygon_collider.enabled = false;
            if (getCollision() != null)
            {
                polygon_collider.pathCount = getCollision().Count;
                int pathnum = 0;
                foreach (Vector2[] path in getCollision())
                {
                    polygon_collider.SetPath(pathnum, path);
                    pathnum++;
                }
                polygon_collider.isTrigger = true;
                polygon_collider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding with " + collision);
        if(objectData.properties.ContainsKey("action"))
        {
            switch(ObjectData.properties["action"])
            {
                case "tp":
                    actionTp(collision, ObjectData.properties["destination"]);
                    break;
            }
        }
    }

    private void actionTp(Collider2D collision, string v)
    {
        GameObject target = GameObject.Find(v);
        if (target == null)
            return;
        collision.transform.position = target.transform.position + new Vector3(tilemap.tileSize, tilemap.tileSize, 0);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private List<Vector2[]> getCollision()
    {
        if(collisionbox==null)
        {
            collisionbox = new List<Vector2[]>();
            List<Vector2[]>rawCollisionInfo = tilemap.map.getTilesetForId(ObjectData.gid).getCollisionForTile(ObjectData.gid);
            foreach(Vector2[] collpath in rawCollisionInfo)
            {
                Vector2[] newPath = new Vector2[collpath.Length];
                for(int i=0;i<collpath.Length;i++)
                {
                    newPath[i] = collpath[i] * tilemap.tileSize;
                }
                collisionbox.Add(newPath);
            }
        }

        return collisionbox;
    }
}
