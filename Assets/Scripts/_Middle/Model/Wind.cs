namespace ColanderSource
{
    public class Wind
    {
        public int strength { get; set; }
        public string windDirectionEnum { get; set; }
    }

    public enum WindDirection
    {
        NORTH,
        SOUTH,
        WEST,
        EAST
    }
}
