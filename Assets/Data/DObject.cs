using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DObject {
    private static int _lastSortingOrder = 1;
    public int id = -1;
    public int gid = -1;
    public float x = -99.0f;
    public float y = -99.0f;
    public float w = 0f;
    public float h = 0f;
    public string name = "";
    public int sortingOrder = 0;
    public Dictionary<string,string> properties = new Dictionary<string, string>();

    public DObject(XmlNode objectTag)
    {
        XmlAttributeCollection attr = objectTag.Attributes;
        int.TryParse(attr["id"].Value, out id);
        if(attr["gid"]!=null)
            int.TryParse(attr["gid"].Value, out gid);
        if (attr["x"] != null)
            float.TryParse(attr["x"].Value, out x);
        if (attr["y"] != null)
            float.TryParse(attr["y"].Value, out y);
        if (attr["width"] != null)
            float.TryParse(attr["width"].Value, out w);
        if (attr["height"] != null)
            float.TryParse(attr["height"].Value, out h);
        if (objectTag.Attributes["name"] != null)
        {
            name = objectTag.Attributes["name"].Value;
        } else
        {
            name = ""+id;
        }
        if (objectTag.HasChildNodes)
        {
            foreach (XmlNode child in objectTag.ChildNodes)
            {
                switch (child.Name) {
                    case "properties":
                        handleProperties(child.ChildNodes);
                        break;
                }
            }
        }
        sortingOrder = _lastSortingOrder++;
    }

    private void handleProperties(XmlNodeList props)
    {
        foreach (XmlNode property in props)
        {
            properties.Add(property.Attributes["name"].Value, property.Attributes["value"].Value);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
