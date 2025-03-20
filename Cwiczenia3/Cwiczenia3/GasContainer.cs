namespace Cwiczenia3;

public class GasContainer:Container, IHazardNotifier
{
    public double Pressure { get; }
    
    public GasContainer(double maxCapacity, double ownWeight, double height, double depth, double pressure)
        : base("G", maxCapacity, ownWeight, height, depth)
    {
        Pressure = pressure;
    }

    public override void Load(double mass)
    {
        if (LoadWeight + mass > MaxCapacity)
        {
            throw new OverfillException($"Load exceeds allowed limit ({MaxCapacity} kg) for {SerialNumber}");
        }
        LoadWeight += mass;
    }

    public override void Unload()
    {
        LoadWeight = LoadWeight * 0.05;
    }

    public void NotifyDanger(string message)
    {
        Console.WriteLine($"Danger {SerialNumber} : {message}");
    }
}