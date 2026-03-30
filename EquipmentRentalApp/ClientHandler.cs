namespace EquipmentRentalApp;

public static class ClientHandler
{
    public static Dictionary<Client, List<Rental>> ClientRentalHistory { get; } = new();
    private static Dictionary<string, Client> ClientIdentifiers { get; } = new();

    public static void RegisterNewClient(Client client)
    {
        ClientRentalHistory.Add(client, new List<Rental>());
        ClientIdentifiers.Add(client.Id, client);
    }

    private static Dictionary<Rental, int> ListUnpaidAssets(Client client)
    {
        Dictionary<Rental, int> total = new Dictionary<Rental, int>();

        if (ClientRentalHistory.ContainsKey(client))
            foreach (Rental r in ClientRentalHistory[client])
                if (r.UnpaidDays > 0)
                    total.Add(r, r.UnpaidDays);
        return total;
    }

    public static Rental? HasClientUnpaidAsset(Client client, Equipment equipment)
    {
        if (ClientRentalHistory.ContainsKey(client))
            foreach (Rental r in ClientRentalHistory[client])
                if (r.Equipment == equipment && r.UnpaidDays > 0) return r;

        return null;
    }
    
    public static int LoopUnpaidAssets(Client client, Action<string> callback)
    {
        int count = 0;
        if (ClientRentalHistory.ContainsKey(client))
        {
            foreach (Rental r in ClientRentalHistory[client])
            {
                if (r.UnpaidDays > 0)
                {
                    count++;
                    callback(r.ToString());
                }
            }

            return count;
        }

        return -1;
    }

    public static int CountUnpaidDays(Client client)
    {
        Dictionary<Rental, int> unpaidAssets = ListUnpaidAssets(client);
        int totalUnpaidDays = 0;
        foreach (var (k, v) in unpaidAssets) totalUnpaidDays += v;
        return totalUnpaidDays;
    }

    public static Client? GetClientById(string id)
    {
        if (ClientIdentifiers.ContainsKey(id)) return ClientIdentifiers[id];
        return null;
    }

    public static void getClientList(Action<Client> callback)
    {
        foreach (Client client in ClientIdentifiers.Values) callback(client);
    }

    public static int GetClientCount()
    {
        return ClientIdentifiers.Count;
    }

    public static int GetClientRentals(Client client, Action<string> callback)
    {
        int count = -1;
        if (ClientRentalHistory.ContainsKey(client))
        {
            count = ClientRentalHistory[client].Count;
            foreach (Rental r in ClientRentalHistory[client])
                callback(r.ToString());
        }

        return count;
    }

    public static bool IsIdentifierRegistered(string id)
    {
        return ClientIdentifiers.ContainsKey(id);
    }

    public static string GenerateClientId()
    {
        string id;
        do id = Guid.NewGuid().ToString("N").Substring(0, 5); //5 digits
        while (ClientIdentifiers.ContainsKey(id));
        return id;
    }
}