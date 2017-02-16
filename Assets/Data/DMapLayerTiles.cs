public class DMapLayerTiles : DMapLayer {
    int[] tilearray;
    public DMapLayerTiles(string name, int width, int height, int[] tilearray) : base(name, width, height) {
        this.tilearray = tilearray;
    }

    public int tileTypeIdAt(int x, int y) {
        int invertedy = height - (y + 1);
        return tilearray[invertedy * width + x];
    }
}
