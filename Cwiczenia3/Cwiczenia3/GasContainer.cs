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
            NotifyDanger("The permissible capacity of the container has been exceeded");
            throw new OverfillException(
                $"The maximum load capacity has been exceeded. The current load is ({mass + LoadWeight} kg), " +
                $"while the maximum permissible load is ({MaxCapacity} kg)");
        }
        LoadWeight += mass;
    }

    public override void Unload()
    {
        if (LoadWeight == 0)
        {
            Console.WriteLine("Gas container is already empty");
        }
        else
        {
            LoadWeight = LoadWeight * 0.05;
            Console.WriteLine("Gas container has been unloaded but 5% of the load remains.");
        }
    }

    public override void DisplayContainerInfo()
    {
        Console.WriteLine("===================================");
        base.DisplayContainerInfo();
        Console.WriteLine($"Pressure inside container: {Pressure} Pa");
        Console.WriteLine("===================================");
    }

    public void NotifyDanger(string message)
    {
        Console.WriteLine($"Danger {SerialNumber} : {message}");
    }
    
}