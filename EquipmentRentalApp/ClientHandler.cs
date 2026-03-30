using System.Text.Json;

namespace EquipmentRentalApp;

public static class ClientHandler
{
    public static Dictionary<Client, List<Rental>> ClientRentalHistory { get; } = new();
    private static Dictionary<string, Client> ClientIdentifiers { get; set; } = new();
    private static string ClientUrl { get; } = Path.Combine(StorageHandler.DirPath, "client.json");

    public static void RegisterNewClient(Client client)
    {
        ClientRentalHistory.Add(client, new List<Rental>());
        ClientIdentifiers.Add(client.Id, client);
        StorageHandler.Save(ClientUrl, StorageHandler.OnError.Warn, () =>
            JsonSerializer.Serialize(ClientIdentifiers, StorageHandler.SavingJsonOptions)
        );
    }

    public static void Load()
    {
        StorageHandler.Load(ClientUrl, StorageHandler.OnError.Warn, (json) =>
            ClientIdentifiers = JsonSerializer.Deserialize<Dictionary<string, Client>>(json) ?? ClientIdentifiers);
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