using System.Collections.Generic;
using System.Xml;

public class DMapLayerObjects : DMapLayer
{
    public Dictionary<int, DObject> objectsById = new Dictionary<int,DObject>();
    public Dictionary<string, DObject> objectsByName = new Dictionary<string, DObject>();
    public DMapLayerObjects(XmlNode objectLayerTag, int width, int height) : base(objectLayerTag, width, height)
    {
        foreach(XmlNode objectTag in objectLayerTag.ChildNodes)
        {
            DObject theobject = new DObject(objectTag);
            objectsById.Add(theobject.id, theobject);
            objectsByName.Add(theobject.name, theobject);
        }
    }
}
