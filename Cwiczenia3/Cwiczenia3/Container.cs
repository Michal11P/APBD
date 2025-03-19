namespace Cwiczenia3;

public abstract class Container
{
    double LoadWeight { get; set; }
    double Height { get; set; }
    double OwnWeight  { get; set; }
    double Depth { get; set; }
    double MaxCapacity { get; set; }
    String SerialNumber { get; set; }
    
    public abstract void Load(double LoadWeight);
    public abstract void Unload();
}