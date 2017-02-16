using UnityEngine;
using System.Collections.Generic;

public class DTileSet {
    //string name;
    public int columns;
    public int count;
    public int firstgid;
    public string res_name;
    public Dictionary<int,List<Vector2[]>> collisionboxes= new Dictionary<int, List<Vector2[]>>();
    
    public DTileSet(string res_name, int columns, int count, int firstgid=1) {
        this.res_name = res_name;
        this.columns = columns;
        this.count = count;
        this.firstgid = firstgid;
    }

    public void setTileFullCollision(int id, bool enable=true) {
        List<Vector2> collisionbox = new List<Vector2>();
        if (enable) {
            collisionbox.Add(new Vector2(0f, 0f));
            collisionbox.Add(new Vector2(0f, 1f));
            collisionbox.Add(new Vector2(1f, 1f));
            collisionbox.Add(new Vector2(1f, 0f));
        }
        setCollisionForTile(id, new List<Vector2[]> { collisionbox.ToArray() });
    }

    public void setCollisionForTile(int id, List<Vector2[]> list, bool globalid=false) {
        if (globalid) id -= (firstgid - 1);
        collisionboxes.Add(id, list);
    }

    public List<Vector2[]> getCollisionForTile(int id, bool globalid=false) {
        if (globalid) id -= (firstgid-1);
        if (collisionboxes.ContainsKey(id)) {
            return collisionboxes[id];
        } else {
            return null;
        }
    }
    //terraininfo: in tmx but not (yet) needed
}
