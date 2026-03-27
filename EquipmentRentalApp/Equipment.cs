namespace EquipmentRentalApp;

public abstract class Equipment
{
    public string Id { get; } = RentalHandler.GenerateEquipmentId();
}

public class ComputerHardware : Equipment
{
}

public class Multimedia : Equipment
{
}

public class LabTools : Equipment
{
}