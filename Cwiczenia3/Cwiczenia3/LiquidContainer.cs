namespace Cwiczenia3;

public class LiquidContainer : Container, IHazardNotifier
{
    public bool HazardNotifier { get; }

    public LiquidContainer(double maxCapacity, double ownWeight, double height, double depth,
        bool isHazardous)
        : base("L", maxCapacity, ownWeight, height, depth)
    {
        HazardNotifier = isHazardous;
    }

    public override void Load(double mass)
    {
        double massLimit = HazardNotifier ? MaxCapacity*0.5 : MaxCapacity*0.9;
        if (mass > massLimit)
        {
            NotifyDanger("The permissible capacity of the container has been exceeded");
            throw new OverfillException(
                $"The maximum load capacity has been exceeded. The current load is ({mass} kg), " +
                $"while the maximum permissible load is ({massLimit} kg)");
        }
        LoadWeight += mass;
    }

    public override void Unload()
    {
        if (LoadWeight == 0)
        {
            Console.WriteLine("Liquid container is already empty");
        }
        else
        {
            LoadWeight = 0;
            Console.WriteLine("Liquid container has been unloaded");
        }
    }

    public override void DisplayContainerInfo()
    {
        Console.WriteLine("===================================");
        base.DisplayContainerInfo();
        Console.WriteLine($"Hazardous Load: {HazardNotifier}");
        Console.WriteLine("===================================");
    }

    public void NotifyDanger(string message)
    {
        Console.WriteLine($"Danger {SerialNumber} : {message}");
    }
}