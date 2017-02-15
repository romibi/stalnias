public class MapLayer {
    public string name;
    public int width;
    public int height;
    int[] tilearray;

    public MapLayer(string name, int width, int height, int[] tilearray) {
        this.name = name;
        this.width = width;
        this.height = height;
        this.tilearray = tilearray;
    }

    public int tileTypeIdAt(int x, int y) {
        int invertedy = height - (y + 1);
        return tilearray[invertedy * width + x];
    }
}
