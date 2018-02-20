using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class DMapLayerTiles : DMapLayer {
    int[] tilearray;
    public DMapLayerTiles(XmlNode tileLayerTag) : base(tileLayerTag)
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
