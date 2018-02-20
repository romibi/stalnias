using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DTileSet {
    //string name;
    public int columns;
    public int count;
    public int firstgid = 1;
    public string res_name; // fallback to path
    public string res_path;
    public Dictionary<int, List<Vector2[]>> collisionboxes = new Dictionary<int, List<Vector2[]>>();
    private int tileresolution = 32;

    public DTileSet(string res_name, int columns, int count, int firstgid = 1) {
        this.res_name = res_name;
        this.columns = columns;
        this.count = count;
        this.firstgid = firstgid;
    }

    public DTileSet(XmlNode tilesetTag, int tileresolution)
    {
        res_name = tilesetTag.Attributes["name"].Value; // TODO: probably need to use child img source
        int.TryParse(tilesetTag.Attributes["columns"].Value, out columns);
        int.TryParse(tilesetTag.Attributes["tilecount"].Value, out count);
        int.TryParse(tilesetTag.Attributes["firstgid"].Value, out firstgid);
        this.tileresolution = tileresolution;
        foreach (XmlNode childNode in tilesetTag.ChildNodes)
        {
            switch (childNode.Name)
            {
                case "image":
                    parseImageNode(childNode);
                    break;
                case "tile":
                    parseTileInfo(childNode);
                    break;
            }
        }
    }

    private void parseTileInfo(XmlNode tileTag)
    {
        int id = -1;
        int.TryParse(tileTag.Attributes["id"].Value, out id);
        if (id == -1) return;
        id++;
        XmlNode objectGroupTag = tileTag.FirstChild;
        if (objectGroupTag.Name != "objectgroup") return;
        List<Vector2[]> colisionboxesToAdd = new List<Vector2[]>();
        foreach (XmlNode objectTag in objectGroupTag.ChildNodes)
        {
            if (objectTag.Name != "object") continue;
            float xoff = 0f;
            float yoff = 0f;
            float.TryParse(objectTag.Attributes["x"].Value, out xoff);
            float.TryParse(objectTag.Attributes["y"].Value, out yoff);

            foreach (XmlNode objectElement in objectTag.ChildNodes)
            {
                Vector2[] collisionbox = null;
                switch (objectElement.Name)
                {
                    case "polygon":
                         collisionbox = parsePolygonTag(objectElement, xoff, yoff);
                        break;
                    case "bolyline":
                        collisionbox = parsePolygonTag(objectElement, xoff, yoff); //TODO: handle collisionbox for lines different
                        break;
                    case "ellipse":
                        collisionbox = parseEllipse(objectTag);
                        break;
                }
                
                if(collisionbox!=null)
                    colisionboxesToAdd.Add(collisionbox);
            }

            if (objectTag.ChildNodes.Count == 0)
                colisionboxesToAdd.Add(parseRectangle(objectTag));
        }
        setCollisionForTile(id, colisionboxesToAdd);
    }

    private Vector2[] parseEllipse(XmlNode objectTag)
    {
        return parseRectangle(objectTag); // TODO: handle ellipse better
    }

    private Vector2[] parseRectangle(XmlNode objectTag)
    {
        float xoff = 0f;
        float yoff = 0f;
        float.TryParse(objectTag.Attributes["x"].Value, out xoff);
        float.TryParse(objectTag.Attributes["y"].Value, out yoff);

        float objwidth = 0f;
        float objheight = 0f;
        float.TryParse(objectTag.Attributes["width"].Value, out objwidth);
        float.TryParse(objectTag.Attributes["height"].Value, out objheight);

        objwidth /= tileresolution;
        objheight /= tileresolution;
        xoff /= tileresolution;
        yoff /= tileresolution;
        yoff = 1f - yoff;
        objheight = -objheight;
        
        return new Vector2[] { new Vector2(xoff, yoff + objheight), new Vector2(xoff, yoff), new Vector2(xoff + objwidth, yoff), new Vector2(xoff + objwidth, yoff + objheight)};
    }

    private Vector2[] parsePolygonTag(XmlNode polygon, float xoff, float yoff)
    {
        List<Vector2> collisionbox = new List<Vector2>();
        if (polygon.Name != "polygon") return null;
        string pointlist = polygon.Attributes["points"].Value;
        string[] points = pointlist.Split(' ');
        foreach (string point in points)
        {
            string[] xy = point.Split(',');
            string xstr = xy[0];
            string ystr = xy[1];
            float x = 0f;
            float y = 0f;
            float.TryParse(xstr, out x);
            float.TryParse(ystr, out y);

            x = (x + xoff) / tileresolution;
            y = (y + yoff) / tileresolution;

            y = 1f - y;

            collisionbox.Add(new Vector2(x, y));
        }
        return collisionbox.ToArray();
    }

    private void parseImageNode(XmlNode imageTag)
    {
        string source = imageTag.Attributes["source"].Value;
        res_path = source;
        string[] pathParts = source.Replace("../textures/", "↑").Split('↑');
        if (pathParts.Length == 2)
            res_path = "/textures/" + pathParts[1];
    }

    public void setTileFullCollision(int id, bool enable=true, bool globalid=false) {
        List<Vector2> collisionbox = new List<Vector2>();
        if (enable) {
            collisionbox.Add(new Vector2(0f, 0f));
            collisionbox.Add(new Vector2(0f, 1f));
            collisionbox.Add(new Vector2(1f, 1f));
            collisionbox.Add(new Vector2(1f, 0f));
        }
        setCollisionForTile(id, new List<Vector2[]> { collisionbox.ToArray() }, globalid);
    }

    public void setCollisionForTile(int id, List<Vector2[]> list, bool globalid=false) {
        if (globalid) id -= (firstgid - 1);
        collisionboxes.Add(id, list);
    }

    public List<Vector2[]> getCollisionForTile(int id, bool globalid=true) {
        if (globalid) id -= (firstgid-1);
        if (collisionboxes.ContainsKey(id)) {
            return collisionboxes[id];
        } else {
            return null;
        }
    }
    //terraininfo: in tmx but not (yet) needed
}
