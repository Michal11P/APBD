namespace Cwiczenia3;

public class Ship : IHazardNotifier
{
    public string Name { get; }
    public double MaxSpeedKnots { get; }
    public int MaxContainerCount { get; }
    public double MaxWeightTons { get; }
    public List<Container> Containers { get; private set; }

    public Ship(string name, double maxSpeedKnots, int maxContainerCount, double maxWeightTons)
    {
        Name = name;
        MaxSpeedKnots = maxSpeedKnots;
        MaxContainerCount = maxContainerCount;
        MaxWeightTons = maxWeightTons;
        Containers = new List<Container>();
    }

    public void LoadContainer(Container container)
    {
        if (IsShipFull())
        {
            NotifyDanger("Ship cannot carry more containers.");
            return;
        }

        if (IsWeightLimitExceeded(container))
        {
            NotifyDanger("Ship's weight limit exceeded.");
            return;
        }
        Containers.Add(container);
        Console.WriteLine($"Container {container.SerialNumber} has been loaded onto the ship {Name}.");
    }

    public void UnloadContainer(Container container)
    {
        if (Containers.Remove(container))
        {
            Console.WriteLine($"Container {container.SerialNumber} has been unloaded from the ship {Name}.");
        }
        else
        {
            throw new Exception($"Container {container.SerialNumber} has not been found on the ship {Name}.");
        }
    }

    public void TransferContainer(Ship targetShip, Container container)
    {
        if (Containers.Contains(container))
        {
            UnloadContainer(container);
            targetShip.LoadContainer(container);
            Console.WriteLine($"Container {container.SerialNumber} has been transfered to the ship {Name}.");
        }
        else
        {
            throw new Exception("Container has not been found on the ship.");
        }
    }

    public void ReplaceContainer(string serialNumber,Container newContainer)
    {
        int containerIndex = Containers.FindIndex(x => x.SerialNumber == serialNumber);
        if (containerIndex == -1)
        {
            Console.WriteLine($"Container with serial number {serialNumber}, has not been found on the ship.");
            return;
        }
        if (Containers[containerIndex].SerialNumber == newContainer.SerialNumber)
        {
            Console.WriteLine($"The container to replace is the same as new container. There is no replacement needed");
            return;
        }

        if (GetTotalWeight() - Containers[containerIndex].TotalWeight() + newContainer.TotalWeight() > MaxWeightTons * 1000)
        {
            NotifyDanger("Ship's weight limit will be exceeded with new container.");
            return;
            
        }
        Containers[containerIndex] = newContainer;
        Console.WriteLine($"Container with serial number {serialNumber} has been replaced with a new container with serial number {newContainer.SerialNumber}.");
    }

    public double GetTotalWeight()
    {
        return Containers.Sum(container => container.TotalWeight());
    }

    private bool IsShipFull()
    {
        return Containers.Count>=MaxContainerCount;
    }

    private bool IsWeightLimitExceeded(Container container)
    {
        return GetTotalWeight() + container.TotalWeight() > MaxWeightTons * 1000;
    }

    public void DisplayShipInfo()
    {
        Console.WriteLine($"Ship name: {Name}");
        Console.WriteLine($"Max speed: {MaxSpeedKnots} knots");
        Console.WriteLine($"Max amount of containers: {MaxContainerCount}");
        Console.WriteLine($"Max weight: {MaxWeightTons} tons");
        Console.WriteLine($"Current load: {Containers.Count} containers");
        Console.WriteLine($"Total weight: {GetTotalWeight()} kg");

        Console.WriteLine("Containers pon the ship:");
        foreach (var container in Containers)
        {
            container.DisplayContainerInfo();
        }
    }
    public void NotifyDanger(string message)
    {
        Console.WriteLine($"DANGER: {message}");
    }
}
