namespace Cwiczenia3;

public class RefrigeratedContainer : Container, IHazardNotifier
{
    public string? ProductType { get; set; }
    public double ContainerTemperature { get; private set; }
    public double RequiredTemperature { get; private set; }
    public double ProductMass { get; private set; }
    

    public RefrigeratedContainer(double maxCapasity, double ownWeight, double height, double depth,
        double containerTemperature)
        : base("C", maxCapasity, ownWeight, height, depth)
    {
        ContainerTemperature = containerTemperature;
        ProductType = null;
        RequiredTemperature = 0;
        ProductMass = 0;
        
    }

    public double LoadProductType(string productType, double requiredTemperature, double productMass)
    {
        if (ProductType == null)
        {
            ProductType = productType;
            RequiredTemperature = requiredTemperature;
        }
        else if (ProductType != productType)
        {
            Console.WriteLine($"Cannot load {productType}. This container is already storing {ProductType}");
            return 0;
        }

        if (LoadWeight + productMass > MaxCapacity)
        {
            ProductType = null;
            RequiredTemperature = 0;
            throw new OverfillException(
                $"The maximum load capacity has been exceeded. The current load is ({LoadWeight+productMass} kg), " +
                $"while the maximum permissible load is ({MaxCapacity} kg)");
        }
        if (ContainerTemperature < requiredTemperature)
        {
            ProductType = null;
            RequiredTemperature = 0;
            NotifyDanger($"Container temperature {ContainerTemperature} degrees Celsius is below required temperature {requiredTemperature} Celsius");
            throw new Exception("The container temperature is too low for the stored product");
        }
        return productMass;
    }

    public override void Load(double productMass)
    {
        if (productMass <= 0)
        {
            Console.WriteLine($"Product mass must be a positive number.");
            return;
        }
        LoadWeight += productMass;
        ProductMass += productMass;
        Console.WriteLine($"Loaded {ProductType} kg of {productMass}. Total product mass inside container is {ProductMass}");
    }


    public override void Unload()
    {
        if (LoadWeight == 0)
        {
            Console.WriteLine("Refrigerated container is already empty.");
        }
        else
        {
            LoadWeight = 0;
            ProductMass = 0;
            ProductType = null;
            RequiredTemperature = 0;
            Console.WriteLine("Refrigerated container has been unloaded");
        }
    }
    public override void DisplayContainerInfo()
    {
        Console.WriteLine("===================================");
        base.DisplayContainerInfo();
        if (ProductType == null)
        {
            Console.WriteLine($"Container is empty.");
        }
        else
        {
            Console.WriteLine($"Stored product: {ProductType}");
            Console.WriteLine($"Stored product mass: {LoadWeight} kg");
            Console.WriteLine($"Required product temperature: {RequiredTemperature} degrees Celsius");
            
        }
        Console.WriteLine($"Container temperature: {ContainerTemperature}");
        Console.WriteLine("===================================");
    }

    public void NotifyDanger(string message)
    {
        Console.WriteLine($"Danger {SerialNumber} : {message}");
    }
}