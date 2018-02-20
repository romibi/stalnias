using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DObject {
    public int id = -1;
    public int gid = -1;
    public float x = -99.0f;
    public float y = -99.0f;
    public float w = 0f;
    public float h = 0f;
    public string name = "";

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
