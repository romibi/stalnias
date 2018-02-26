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
    public List<Collider2D> ignoreTriggerOnce = new List<Collider2D>();

    TileMap tilemap {
        get {
            return transform.parent.transform.parent.GetComponent<TileMap>();
        }
    }
    
    float Width {
        get {
            return ObjectData.w * tilemap.tileSize / tilemap.textureResolution;
        }
    }
    float Height {
        get {
            return ObjectData.w * tilemap.tileSize / tilemap.textureResolution;
        }
    }

    public Vector3 getGroundCenterCoordinates()
    {
        return transform.position + new Vector3(Width / 2f, Height / 2f, 0f);
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

            setSize(new Vector2(objectData.w, objectData.h) / tilemap.textureResolution);

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
        if (ignoreTriggerOnce.Remove(collision))
            return;
        if(objectData.properties.ContainsKey("onCollision"))
        {
            switch(ObjectData.properties["onCollision"])
            {
                case "tp":
                    teleport(collision, ObjectData.properties["target"]);
                    break;
            }
        }
    }

    private void teleport(Collider2D collision, string v)
    {
        GameObject target = GameObject.Find(v);
        if (target == null)
            return;

        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player == null)
            return;

        IngameObject targetObject = target.GetComponent<IngameObject>();
        if (targetObject != null)
        {
            targetObject.ignoreTriggerOnce.Add(collision);
            player.teleportTo(targetObject.getGroundCenterCoordinates());
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    private List<Vector2[]> getCollision()
    {
        Vector2 scale = getSpriteScale2D();
        Vector2 inverse_scale = new Vector2(1f,1f);
        inverse_scale.x /= scale.x;
        inverse_scale.y /= scale.y;
        if (collisionbox==null)
        {
            collisionbox = new List<Vector2[]>();
            List<Vector2[]>rawCollisionInfo = tilemap.map.getTilesetForId(ObjectData.gid).getCollisionForTile(ObjectData.gid);
            foreach(Vector2[] collpath in rawCollisionInfo)
            {
                Vector2[] newPath = new Vector2[collpath.Length];
                for(int i=0;i<collpath.Length;i++)
                {
                    newPath[i] = Vector2.Scale(collpath[i], inverse_scale);
                }
                collisionbox.Add(newPath);
            }
        }

        return collisionbox;
    }

    public void setSize(Vector2 size)
    {
        setSize(new Vector3(size.x, size.y, 1f));
    }

    public void setSize(Vector3 size)
    {
        Vector3 sprite_scale = getSpriteScale3D();
        sprite_scale.Scale(size);
        transform.localScale = sprite_scale;
    }

    private Vector3 getSpriteScale3D()
    {
        Vector3 scale = (Vector3)getSpriteScale2D();
        scale.z = 1f;
        return scale;
    }

    private Vector2 getSpriteScale2D()
    {
        SpriteRenderer sprite_renderer = GetComponent<SpriteRenderer>();
        Sprite sprite = sprite_renderer.sprite;
        if (sprite == null)
            Debug.LogWarning("Cannot get sprite scale, no sprite and therefore (texture) size unknown!");
        Texture2D texture = sprite.texture;
        if (texture == null)
            Debug.LogWarning("Cannot get sprite scale, sprite texture size unknown!");
        return new Vector2(tilemap.tileSize * sprite.pixelsPerUnit / texture.width, tilemap.tileSize * sprite.pixelsPerUnit / texture.width);
    }
}
