namespace EquipmentRentalApp;

public static class ClientHandler
{
    public static Dictionary<Client, List<Rental>> ClientRentalHistory { get; } = new();
    private static Dictionary<string, Client> ClientIdentifiers { get; } = new();

    public static void RegisterNewClient(Client client)
    {
        if (ClientIdentifiers.ContainsKey(client.Id)) throw new ConsoleException(8, new[] { client.Id });
        ClientRentalHistory.Add(client, new List<Rental>());
        ClientIdentifiers.Add(client.Id, client);
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

    public static Client? GetClientById(string id)
    {
        if (ClientIdentifiers.ContainsKey(id)) return ClientIdentifiers[id];
        else return null;
    }

    public static void getClientList(Action<Client> callback)
    {
        foreach (Client client in ClientIdentifiers.Values) callback(client);
    }
}