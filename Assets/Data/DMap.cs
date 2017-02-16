using System.Collections.Generic;

public class DMap {
    public int width;
    public int height;
    public int tileresolution;

    public List<DTileSet> tilesets;
    public List<DMapLayer> layers;

    public DMap() {
        width = 20;
        height = 10;
        tileresolution = 32;
        tilesets = new List<DTileSet>();
        tilesets.Add(new DTileSet("grass",3,18));
        tilesets.Add(new DTileSet("watergrass", 3, 18, 19));

        layers = new List<DMapLayer>();
        layers.Add(new DMapLayerTiles("bg", width, height, new int[] {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16}));

        layers.Add(new DMapLayerTiles("something", width, height, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0,25,26,27, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,25,26,24,29,23,27, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,28,29,20,32,32,33, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,28,20,33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,31,33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}));
        layers.Add(new DMapLayerObjects("Living", width, height));
    }

    public DMap(int width, int height, List<DTileSet> tilesets, List<DMapLayer> layers, int tileresolution = 32) {
        this.width = width;
        this.height = height;
        this.tileresolution = tileresolution;
        this.tilesets = tilesets;
        this.layers = layers;
    }
}
