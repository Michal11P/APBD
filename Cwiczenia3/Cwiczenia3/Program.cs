namespace Cwiczenia3
{
    class Program
    {
        public static void Main(string[] args)
        {
            List<Ship> ships = new List<Ship>();
            List<Container> containers = new List<Container>();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("======Cargo Management System======");
                Console.WriteLine("1. Create a new Container");
                Console.WriteLine("2. Create a new Ship");
                Console.WriteLine("3. Load cargo into a container");
                Console.WriteLine("4. Unload cargo from a container");
                Console.WriteLine("5. Load container onto a ship");
                Console.WriteLine("6. Unload container from a ship");
                Console.WriteLine("7. Replace a container on a ship");
                Console.WriteLine("8. Transfer a container between ships");
                Console.WriteLine("9. Display container information");
                Console.WriteLine("10. Display ship information");
                Console.WriteLine("11. Exit");
                
                Console.Write("Select an option, choose number (1-10): ");
                string choice = Console.ReadLine();
                Console.WriteLine("==================================");
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter container type (L - Liquid, G - Gas, C - Refrigerated): ");
                        string containerType = Console.ReadLine();
                        Console.Write("Enter container max capacity (kg): ");
                        double maxCapacity = double.Parse(Console.ReadLine());
                        Console.Write("Enter container own weight (kg): ");
                        double ownWeight = double.Parse(Console.ReadLine());
                        Console.Write("Enter container height (cm): ");
                        double height = double.Parse(Console.ReadLine());
                        Console.Write("Enter container depth (cm): ");
                        double depth = double.Parse(Console.ReadLine());

                        Container container = null;
                        if (containerType.ToUpper() == "L")
                        {
                            Console.WriteLine("Is the load hazardous? (YES/NO): ");
                            bool isLoadHazardous = Console.ReadLine().ToUpper() == "YES";
                            container = new LiquidContainer(maxCapacity, ownWeight, height, depth, isLoadHazardous);
                        }
                        else if (containerType.ToUpper() == "G")
                        {
                            Console.WriteLine("Enter pressure inside container (Pa): ");
                            double pressure = double.Parse(Console.ReadLine());
                            container = new GasContainer(maxCapacity, ownWeight, height, depth, pressure);
                        }
                        else if (containerType.ToUpper() == "C")
                        {
                            Console.WriteLine("Enter temperature inside container (degrees Celsius): ");
                            double temperature = double.Parse(Console.ReadLine());
                            container = new RefrigeratedContainer(maxCapacity, ownWeight, height, depth, temperature);
                        }
                        if (container != null)
                        {
                           containers.Add(container);
                           Console.WriteLine("===================================");
                           Console.WriteLine($"Container {container.SerialNumber} has been created");
                           Console.WriteLine("===================================");
                        }
                        break;
                    
                    case "2":
                        Console.Write("Enter ship name: ");
                        string shipName = Console.ReadLine();
                        Console.Write("Enter max speed (knots): ");
                        double maxSpeedKnots = double.Parse(Console.ReadLine());
                        Console.Write("Enter the maximum number of containers that the ship can accommodate: ");
                        int maxContainersCount = int.Parse(Console.ReadLine());
                        Console.Write("Enter the maximum weight that the ship can carry (tons): ");
                        double maxWeightTons = double.Parse(Console.ReadLine());
                        
                        ships.Add(new Ship(shipName, maxSpeedKnots, maxContainersCount, maxWeightTons));
                        Console.WriteLine($"Ship {shipName} has been created successfully");
                        break;
                    
                    case "3":
                        Console.Write("Enter container serial number: ");
                        string serialNumber = Console.ReadLine();
                        Container foundContainer = containers.Find(c => c.SerialNumber == serialNumber);
                        if (foundContainer != null)
                        {
                            if (foundContainer is RefrigeratedContainer refrigeratedContainer)
                            {
                                Console.Write("Enter product type to load: ");
                                string productType = Console.ReadLine();
                                Console.Write("Enter product required temperature: ");
                                double requiredTemperature = double.Parse(Console.ReadLine());
                                Console.Write("Enter cargo weight (kg): ");
                                double productMass = double.Parse(Console.ReadLine());
                                try
                                {
                                    refrigeratedContainer.LoadProductType(productType, requiredTemperature, productMass);
                                    refrigeratedContainer.Load(productMass);
                                    Console.WriteLine($"{productType} has been loaded successfully onto container {foundContainer.SerialNumber}");
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine($"Error: {exception.Message}");
                                }
                            }
                            else
                            {
                                Console.Write("Enter cargo weight (kg): ");
                                double weight = double.Parse(Console.ReadLine());
                                try
                                {
                                    foundContainer.Load(weight);
                                    Console.WriteLine($"{weight} has been loaded successfully onto container {foundContainer.SerialNumber}");
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine($"Error: {exception.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Container not found");
                        }
                        break;
                    
                    case "4":
                        Console.Write("Enter container serial number: ");
                        serialNumber = Console.ReadLine();
                        foundContainer = containers.Find(c => c.SerialNumber == serialNumber);
                        if (foundContainer != null)
                        {
                            foundContainer.Unload();
                        }
                        else
                        {
                            Console.WriteLine("Container not found");
                        }
                        break;
                    
                    case "5":
                        Console.Write("Enter ship name: ");
                        string name = Console.ReadLine();
                        Ship foundShip  = ships.Find(s => s.Name == name);
                        if (foundShip != null)
                        {
                            Console.Write("Enter container serial numbers separated by commas (KON-L-1, KON-G-2): ");
                            serialNumber = Console.ReadLine();
                            string[] serialNumbers = serialNumber.Split(",");
                            foreach (string number in serialNumbers)
                            {
                                string trimmedSerialNumber = number.Trim();
                                foundContainer = containers.Find(c => c.SerialNumber == trimmedSerialNumber);
                                if (foundContainer != null)
                                {
                                    foundShip.LoadContainer(foundContainer);
                                }
                                else
                                {
                                    Console.WriteLine($"Container {serialNumber} not found");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ship {name} not found");
                        }
                        break;
                    
                    case "6":
                        Console.Write("Enter ship name: ");
                        name = Console.ReadLine();
                        foundShip = ships.Find(s => s.Name == name);
                        if (foundShip != null)
                        {
                            Console.Write("Enter container serial number: ");
                            serialNumber = Console.ReadLine();
                            foundContainer = containers.Find(c => c.SerialNumber == serialNumber);
                            if (foundContainer != null)
                            {
                                foundShip.UnloadContainer(foundContainer);
                                Console.WriteLine($"Container {foundContainer.SerialNumber} has been unloaded");
                            }
                            else
                            {
                                Console.WriteLine($"Container {serialNumber} not found on the ship");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Ship {name} not found");
                        }
                        break;
                    
                    case "7":
                        Console.Write("Enter ship name: ");
                        name = Console.ReadLine();
                        foundShip = ships.Find(s => s.Name == name);
                        if (foundShip != null)
                        {
                            Console.Write("Enter serial number of the container to be replaced: ");
                            serialNumber = Console.ReadLine();
                            Container foundContainerToReplace = containers.Find(c => c.SerialNumber == serialNumber);
                            if (foundContainerToReplace != null)
                            {
                                Console.Write("Enter serial number of the new container: ");
                                string newContainerSerialNumber = Console.ReadLine();
                                Container foundNewContainer = containers.Find(c => c.SerialNumber == newContainerSerialNumber);
                                if (foundNewContainer != null)
                                {
                                    foundShip.ReplaceContainer(serialNumber, foundNewContainer);
                                }
                                else
                                {
                                    Console.WriteLine($"New container {newContainerSerialNumber} not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Container {serialNumber} not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ship not found");
                        }
                        break;
                    
                    case "8":
                        Console.Write("Enter the source ship name: ");
                        string sourceShipName = Console.ReadLine();
                        Ship sourceShip = ships.Find(s => s.Name == sourceShipName);
                        if (sourceShip != null)
                        {
                            Console.Write("Enter the target ship name: ");
                            string targetShipName = Console.ReadLine();
                            Ship targetShip = ships.Find(s => s.Name == targetShipName);
                            if (targetShip != null)
                            {
                                Console.Write("Enter serial number of the container to transfer: ");
                                serialNumber = Console.ReadLine();
                                Container containerToTransfer = containers.Find(c => c.SerialNumber == serialNumber);
                                if (containerToTransfer != null)
                                {
                                    try
                                    {
                                        sourceShip.TransferContainer(targetShip, containerToTransfer);
                                        Console.WriteLine($"Container {serialNumber} successfully transfered to {targetShip.Name} from {sourceShip.Name}");

                                    }
                                    catch(Exception exception)
                                    {
                                        Console.WriteLine($"Error: {exception.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Container not found on the source ship");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Target ship {targetShipName} not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Source ship {sourceShipName} not found");
                        }
                        break;
                        

                    case "9":
                        Console.Write("Enter container serial number: ");
                        serialNumber = Console.ReadLine();
                        foundContainer = containers.Find(c => c.SerialNumber == serialNumber);
                        if (foundContainer != null)
                        {
                            foundContainer.DisplayContainerInfo();
                        }
                        else
                        {
                            Console.WriteLine($"Container {serialNumber} not found");
                        }
                        break;
                    
                    case "10":
                        Console.Write("Enter ship name: ");
                        name = Console.ReadLine();
                        foundShip = ships.Find(s => s.Name == name);
                        if (foundShip != null)
                        {
                            foundShip.DisplayShipInfo();
                        }
                        else
                        {
                            Console.WriteLine($"Ship {name} not found");
                        }
                        break;
                    
                    case "11":
                        Console.Write("Exiting the system!!!");
                        return;
                    default:
                        Console.WriteLine("Invalid input. Please try again");
                        break;
                }
            }
        }

    }
}
