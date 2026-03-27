namespace EquipmentRentalApp;

public class Client
{
    public string Id { get; }
    public string FullName { get; }

    public Client(string id, string fullName)
    {
        this.Id = id;
        this.FullName = fullName;
    }
}