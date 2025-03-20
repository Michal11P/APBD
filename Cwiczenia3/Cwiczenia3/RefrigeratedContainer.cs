namespace Cwiczenia3;

public class RefrigeratedContainer : Container
{
    public string ProductType { get; }
    public double Temperature { get; }

    public RefrigeratedContainer(double maxCapasity, double ownWeight, double height, double depth,
        double temperature, string productType)
        : base("C", maxCapasity, ownWeight, height, depth)
    {
        ProductType = productType;
        Temperature = temperature;
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
        LoadWeight = 0;
    }
}