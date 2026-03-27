namespace EquipmentRentalApp;

public static class RentalHandler
{
    private static Dictionary<Client, List<Rental>> ClientRentalHistory { get; } = new();
    private static Dictionary<Equipment, List<Rental>> EquipmentRentalHistory { get; } = new();
    private static Dictionary<string, Equipment> EquipmentIdentifiers { get; } = new();
    private static Dictionary<string, Client> ClientIdentifiers { get; } = new();

    public static void RegisterNewClient(Client client)
    {
        if (ClientIdentifiers.ContainsKey(client.Id)) throw new ConsoleException(8, new[] { client.Id });
        ClientRentalHistory.Add(client, new List<Rental>());
        ClientIdentifiers.Add(client.Id, client);
    }

    public static void RegisterNewEquipment(Equipment eq)
    {
        EquipmentRentalHistory.Add(eq, new List<Rental>());
        EquipmentIdentifiers.Add(eq.Id, eq);
    }

    public static void RegisterNewRental(Equipment equipment, Client client, Rental rental)
    {
        ClientRentalHistory[client].Add(rental);
        EquipmentRentalHistory[equipment].Add(rental);
    }

    public static void RentEquipment(Equipment equipment, Client client, DateTime toDate)
    {
        if (IsEquipmentRented(equipment)) throw new ConsoleException(1, new[] { equipment.Id, client.Id });
        int unpaidDays = CountUnpaidDays(client);
        if (unpaidDays > 0) throw new ConsoleException(9, new[] { client.Id, unpaidDays.ToString() });
        if (toDate < DateTime.Now) throw new ConsoleException(10, []);

        Rental rental = new Rental(equipment, client, toDate);
        RentalHandler.RegisterNewRental(equipment, client, rental);
        ClientRentalHistory[client].Add(rental);
        EquipmentRentalHistory[equipment].Add(rental);
    }

    public static void ReturnEquipment(Equipment equipment, Client client)
    {
        if (!IsEquipmentRented(equipment)) throw new ConsoleException(2, new[] { equipment.Id });
        var history = EquipmentRentalHistory[equipment];
        var last = history[history.Count - 1];
        last.RegisterReturn();
    }

    public static Dictionary<Rental, int> ListUnpaidAssets(Client client)
    {
        Dictionary<Rental, int> total = new Dictionary<Rental, int>();

        if (ClientRentalHistory.ContainsKey(client))
            foreach (Rental r in ClientRentalHistory[client])
                if (r.UnpaidDays > 0)
                    total.Add(r, r.UnpaidDays);
        return total;
    }

    public static int CountUnpaidDays(Client client)
    {
        Dictionary<Rental, int> unpaidAssets = ListUnpaidAssets(client);
        int totalUnpaidDays = 0;
        foreach (var (k, v) in unpaidAssets) totalUnpaidDays += v;
        return totalUnpaidDays;
    }

    public static string GenerateEquipmentId()
    {
        string id;
        do id = Guid.NewGuid().ToString("N").Substring(0, 5); //5 digits
        while (EquipmentIdentifiers.ContainsKey(id));
        return id;
    }

    private static bool IsEquipmentRented(Equipment equipment)
    {
        List<Rental> history = EquipmentRentalHistory[equipment];
        Rental? last = history.Count > 0 ? history[history.Count - 1] : null;
        if (last == null) return false;
        return last.IsRented;
    }
}