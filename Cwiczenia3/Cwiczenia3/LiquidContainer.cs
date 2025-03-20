namespace Cwiczenia3;

public class LiquidContainer : Container, IHazardNotifier
{
    public bool IHazardNotifier { get; }

    public LiquidContainer(double maxCapacity, double ownWeight, double height, double depth,
        bool isHazardous)
        : base("L", maxCapacity, ownWeight, height, depth)
    {
        IHazardNotifier = isHazardous;
    }

    public override void Load(double mass)
    {
        double massLimit = IHazardNotifier ? MaxCapacity*0.5 : MaxCapacity*0.9;
        if (mass > massLimit)
        {
            NotifyDanger("The permissible capacity of the container has been exceeded");
            throw new OverfillException($"Load exceeds allowed limit ({massLimit} kg) for {SerialNumber}");
        }
        LoadWeight += mass;
    }

    public override void Unload()
    {
        LoadWeight = 0;
    }

    public void NotifyDanger(string message)
    {
        Console.WriteLine($"Danger {SerialNumber} : {message}");
    }
}