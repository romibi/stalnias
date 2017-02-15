public class TileSet {
    //string name;
    public int columns;
    public int count;
    public int firstgid;
    public string res_name;

    public TileSet(string res_name, int columns, int count, int firstgid=1) {
        this.res_name = res_name;
        this.columns = columns;
        this.count = count;
        this.firstgid = firstgid;
    }
    //terraininfo: in tmx but not (yet) needed
}
