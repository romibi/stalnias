using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class DMapLayerTiles : DMapLayer {
    int[] tilearray;
    public DMapLayerTiles(string name, int width, int height, int[] tilearray) : base(name, width, height) {
        this.tilearray = tilearray;
    }

    public DMapLayerTiles(XmlNode tileLayerTag) : 
        base(
            tileLayerTag.Attributes["name"].Value,
            int.Parse(tileLayerTag.Attributes["width"].Value),
            int.Parse(tileLayerTag.Attributes["height"].Value)
        )
    {
        String layerdataCSV = tileLayerTag.FirstChild.InnerText;
        tilearray = StringToIntList(layerdataCSV).Cast<int>().ToArray();
    }

    public int tileTypeIdAt(int x, int y) {
        int invertedy = height - (y + 1);
        return tilearray[invertedy * width + x];
    }

    public static IEnumerable<int> StringToIntList(string str)
    // by SLaks @ https://stackoverflow.com/a/1763626
    {
        if (String.IsNullOrEmpty(str))
            yield break;

        foreach (var s in str.Split(','))
        {
            int num;
            if (int.TryParse(s, out num))
                yield return num;
        }
    }
}
