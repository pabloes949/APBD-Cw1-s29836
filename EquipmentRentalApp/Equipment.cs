namespace EquipmentRentalApp;

public abstract class Equipment
{
    public string Id { get; } = RentalHandler.GenerateEquipmentId();
    protected string Producer { get; }
    protected string Model { get; }
    protected string SerialNumber { get; }
    protected EquipmentCondition Condition { get; }

    protected Equipment()
    {
    }

    public static Dictionary<string, Func<Equipment>> CreateEquipment = new()
    {
        { "ComputerHardware", () => new ComputerHardware() },
        { "Multimedia", () => new MultimediaDevice() },
        { "LabTools", () => new LabTool() }
    };

    public static string[] GetEquipmentTypes()
    {
        return CreateEquipment.Keys.ToArray();
    }

    public enum EquipmentCondition
    {
        Operable,
        ServiceNeed,
        RepairNeed
    }
}

public class ComputerHardware : Equipment
{
    private bool HasCharger { get; set; }
    private bool IsPortable { get; set; }
    private ComputerHardwareType Type  { get; set; }

    private enum ComputerHardwareType
    {
        Laptop,
        Desktop,
        Monitor,
        Keyboard,
        Mouse,
        DockingStation,
        ExternalDrive,
        Tablet,
        Webcam,
        Headset
    }

    public ComputerHardware()
    {
    }
    
    public override string ToString()
    {
        return
            $"[ComputerHardware] Id: {this.Id}; Type: {this.Type.ToString()}; Producer: {this.Producer}; Model: {this.Model}; Serial: {this.SerialNumber}; Status: {this.Condition.ToString()}, Portable: {this.IsPortable}; Has charger: {this.HasCharger}";
    }
}

public class MultimediaDevice : Equipment
{
    private bool RequiresTraining { get; set; }
    private bool HasAccessories { get; set; }
    private MultimediaDeviceType Type { get; set; }

    private enum MultimediaDeviceType
    {
        Projector,
        Camera,
        Microphone,
        Speaker,
        VRHeadset,
        Headphones,
        LightingKit
    }

    public MultimediaDevice()
    {
    }
    
    public override string ToString()
    {
        return
            $"[MultimediaDevice] Id: {this.Id}; Type: {this.Type.ToString()}; Producer: {this.Producer}; Model: {this.Model}; Serial: {this.SerialNumber}; Status: {this.Condition.ToString()}, Requires Training: {this.RequiresTraining}; Has accessories: {this.HasAccessories}";
    }
}

public class LabTool : Equipment
{
    private bool RequiresSupervisor { get; set; }
    private LabToolType Type { get; set; }
    private enum LabToolType
    {
        Oscilloscope,
        Microscope,
        Arduino,
        Raspberry
    }

    public LabTool()
    {
    }

    public override string ToString()
    {
        return
            $"[LabTool] Id: {this.Id}; Type: {this.Type.ToString()}; Producer: {this.Producer}; Model: {this.Model}; Serial: {this.SerialNumber}; Status: {this.Condition.ToString()}, Requires Supervisor: {this.RequiresSupervisor}";
    }
}