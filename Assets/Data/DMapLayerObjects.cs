using System.Xml;

public class DMapLayerObjects : DMapLayer
{
    int[] tilearray;
    public DMapLayerObjects(string name, int width, int height) : base(name, width, height)
    {

    }

    public DMapLayerObjects(XmlNode objectLayerTag, int width, int height) :
        base(objectLayerTag.Attributes["name"].Value, width, height)
    {
    }
}
