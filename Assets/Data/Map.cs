using System.Collections.Generic;

public class Map {
    public int width;
    public int height;
    public int tileresolution;

    public List<TileSet> tilesets;
    public List<MapLayer> layers;

    public Map() {
        width = 20;
        height = 10;
        tileresolution = 32;
        tilesets = new List<TileSet>();
        tilesets.Add(new TileSet("grass",3,18));
        tilesets.Add(new TileSet("watergrass", 3, 18, 19));

        layers = new List<MapLayer>();
        layers.Add(new MapLayer("bg", width, height, new int[] {16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,
                                                                16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16,16}));

        layers.Add(new MapLayer("something", width, height, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0,25,26,27, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,25,26,24,29,23,27, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,28,29,20,32,32,33, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,28,20,33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0,31,33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}));
    }

    public Map(int width, int height, List<TileSet> tilesets, List<MapLayer> layers, int tileresolution = 32) {
        this.width = width;
        this.height = height;
        this.tileresolution = tileresolution;
        this.tilesets = tilesets;
        this.layers = layers;
    }
}
