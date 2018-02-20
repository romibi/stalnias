using System.Xml;

public class DMapLayer {
    public string name;
    public int width;
    public int height;
    public bool visible = true;
    public float opacity = 1f;

    public DMapLayer(XmlNode tileLayerTag) : this(tileLayerTag, int.Parse(tileLayerTag.Attributes["width"].Value), int.Parse(tileLayerTag.Attributes["height"].Value)) { }

    public DMapLayer(XmlNode tileLayerTag, int width, int height)
    {
        name = tileLayerTag.Attributes["name"].Value;
        this.width = width;
        this.height = height;
        if (tileLayerTag.Attributes["visible"] != null)
        {
            int xmlintvisible = 1;
            if (int.TryParse(tileLayerTag.Attributes["visible"].Value, out xmlintvisible))
                visible = (xmlintvisible != 0);
            else
                bool.TryParse(tileLayerTag.Attributes["visible"].Value, out visible);
        }
        if (tileLayerTag.Attributes["opacity"] != null)
            float.TryParse(tileLayerTag.Attributes["opacity"].Value, out opacity);
    }
}
