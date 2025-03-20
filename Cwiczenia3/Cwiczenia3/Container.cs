namespace Cwiczenia3;

public abstract class Container
{
    private static int number = 1;
    public string SerialNumber { get;}
    protected double LoadWeight { get; set;}
    double Height { get;}
    double OwnWeight  { get;}
    double Depth { get;}
    protected double MaxCapacity { get;}

    protected Container(string type, double maxCapacity, double ownWeight, double height, double depth)
    {
        SerialNumber = $"KON-{type}-{number++}";
        MaxCapacity = maxCapacity;
        OwnWeight = ownWeight;
        Height = height;
        Depth = depth;
    }
    
    public abstract void Load(double mass);
    public abstract void Unload();
}