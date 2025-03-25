namespace Cwiczenia3;

public abstract class Container
{
    private static int number = 1;
    protected double LoadWeight { get; set;}
    double Height { get;}
    double OwnWeight  { get;}
    double Depth { get;}
    public string SerialNumber { get;}
    protected double MaxCapacity { get;}

    protected Container(string type, double maxCapacity, double ownWeight, double height, double depth)
    {
        SerialNumber = $"KON-{type}-{number++}";
        MaxCapacity = maxCapacity;
        OwnWeight = ownWeight;
        Height = height;
        Depth = depth;
    }

    public double TotalWeight()
    {
        return OwnWeight + LoadWeight;
    }
    public abstract void Load(double mass);
    public abstract void Unload();

    public virtual void DisplayContainerInfo()
    {
        Console.WriteLine($"Container Serial Number: {SerialNumber}");
        Console.WriteLine($"Container Max Capacity: {MaxCapacity} kg");
        Console.WriteLine($"Container Own Weight: {OwnWeight} kg");
        Console.WriteLine($"Container Current Load: {LoadWeight} kg");
        Console.WriteLine($"Container Total Weight: {TotalWeight()} kg");
        Console.WriteLine($"Container Height: {Height} cm");
        Console.WriteLine($"Container Depth: {Depth} cm");
        
        
        
    }
}