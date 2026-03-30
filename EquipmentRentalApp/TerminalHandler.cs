using System.Globalization;

namespace EquipmentRentalApp;

public static class TerminalHandler
{
    public static void Run()
    {
        ConsoleBot.RestoreMode = true;
        ExpectUserCommand();
    }

    private static void ExpectUserCommand()
    {
        string command = ConsoleBot.RestoreMode ? ConsoleBot.ReadStorageNext() : Console.ReadLine() ?? "";
        string[] parts = command.Split(' ');
        string[] rest = parts[1..];
        HandleUserCommand(parts[0], rest);
    }

    private static void HandleUserCommand(string command, string[] args)
    {
        try
        {
            switch (command)
            {
                case "welcome":
                    DisplayWelcomePrompt();
                    break;
                case "help":
                    DisplayHelpPrompt();
                    break;
                case "exit":
                    HandleExitPrompt();
                    return; //RETURN remains here - terminate program
                case "client-add":
                    HandleClientAddPrompt();
                    break;
                case "client-detail":
                    HandleClientDetailPrompt(args);
                    break;
                case "client-list":
                    HandleClientListPrompt();
                    break;
                case "client-rentals":
                    HandleClientRentalsPrompt(args);
                    break;
                case "client-payments":
                    HandleClientPaymentsPrompt(args);
                    break;
                case "rent":
                    HandleEquipmentRentalPrompt(args);
                    break;
                case "return":
                    HandleEquipmentReturnPrompt(args);
                    break;
                case "equipment-add":
                    HandleEquipmentAddPrompt(args);
                    break;
                case "equipment-list":
                    HandleEquipmentListPrompt(args);
                    break;
                case "equipment-detail":
                    HandleEquipmentDetailPrompt(args);
                    break;
                case "equipment-available":
                    HandleEquipmentAvailabilityPrompt(args);
                    break;
                case "equipment-history":
                    HandleEquipmentHistoryPrompt(args);
                    break;
                case "report":
                    HandleReportRequestPrompt();
                    break;
                case "payment-accept":
                    HandlePaymentPrompt(args);
                    break;
                default:
                    HandleUnrecognizedPrompt();
                    break;
            }
        }
        catch (ConsoleException e)
        {
            Console.WriteLine($"WARN: {e.Message}");
        }

        ExpectUserCommand();
    }

    private static void DisplayHelpPrompt()
    {
        Console.WriteLine(
            """
                welcome
                    display initial welcome message
                help
                    list of commands
                exit
                    exit program
                client-add
                    register new client
                    the user is brought through a survey
                client-detail {client-id}
                    show client information
                client-list
                    show all clients
                client-rentals {client-id}
                    list equipment rented by client
                client-payments {client-id}
                    show if client has outstanding payments 
                rent {client-id} {equipment-id} {date}
                    rent equipment to client
                rent {client-id} {equipment-id} {date-from} {date-to}
                    rent equipment to a client by specifying exact dates
                return {equipment-id}
                    register an equipment return in system
                equipment-add
                    the user is brought through a survey
                    register new equipment
                equipment-list
                    list registered equipment
                equipment-list {category}
                    list registered equipment by given category
                equipment-detail {equipment-id}
                    show equipment information
                equipment-available {equipment-id}
                    check if equipment is available to be rented
                equipment-history {equipment-id}
                    list equipment rental history
                equipment-state {equipment-id}
                    set a state of equipment (operable, broken, repair)
                report
                    generate rental report in the given period of time
                payment-accept {client-id} {equipment-id}
                    register payment for delayed return
            """);
    }

    private static void DisplayWelcomePrompt()
    {
        Console.Clear();
        Console.WriteLine("Hello User!\nhelp - list of commands\nexit - exit program");
    }

    private static void HandleExitPrompt()
    {
        Console.WriteLine("Bye User!");
    }

    private static void HandleUnrecognizedPrompt()
    {
        Console.WriteLine(
            "Could not interpret command. Try again or type\nhelp - list of commands\nexit - exit program");
    }

    private static void HandleEquipmentAddPrompt(params string[] args)
    {
        string[] equipmentTypes = Equipment.GetEquipmentTypes();
        int chosenType =
            TerminalHandler.GetOptionFromUser("Choose equipment type", "give up adding new equipment", equipmentTypes);
        Equipment e = Equipment.CreateEquipment[equipmentTypes[chosenType]]();
        RentalHandler.RegisterNewEquipment(e);
        Console.WriteLine($"Equipment {e.Id} registered successfully.");
    }

    private static void HandleClientAddPrompt()
    {
        string[] clientTypes = Client.GetClientTypes();
        int chosenType = TerminalHandler.GetOptionFromUser("Choose user type", "give up adding new user", clientTypes);
        Client c = Client.CreateClient[clientTypes[chosenType]]();
        ClientHandler.RegisterNewClient(c);
        Console.WriteLine($"Client {c.Id} registered successfully.");
    }

    private static void HandleClientDetailPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(13, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(14, []);
        Client? client = ClientHandler.GetClientById(id);
        if (client == null) throw new ConsoleException(15, new[] { id });
        Console.WriteLine(client.ToString());
    }

    private static void HandleClientListPrompt()
    {
        if (ClientHandler.GetClientCount() == 0) throw new ConsoleException(16, []);
        ClientHandler.getClientList(client => Console.WriteLine(client.ToString()));
    }

    private static void HandleClientRentalsPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(13, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(14, []);
        Client? client = ClientHandler.GetClientById(id);
        if (client == null) throw new ConsoleException(15, new[] { id });
        int countClientRentals = ClientHandler.GetClientRentals(client, (rental) => Console.WriteLine(rental));
        if (countClientRentals == 0) throw new ConsoleException(29, new[] { id });
    }

    private static void HandleClientPaymentsPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(13, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(14, []);
        Client? client = ClientHandler.GetClientById(id);
        if (client == null) throw new ConsoleException(15, new[] { id });
        int countUnpaidAssets = ClientHandler.LoopUnpaidAssets(client, (rental) => Console.WriteLine(rental));
        if(countUnpaidAssets == 0) throw new ConsoleException(28, new[] { id });
    }

    private static void HandleEquipmentRentalPrompt(params string[] args)
    {
        if (args.Length < 3) throw new ConsoleException(22, []);

        string clientId = args[0].Trim();
        string equipmentId = args[1].Trim();
        string userFromDate = args.Length > 3 ? args[2].Trim() : DateTime.Now.ToString("yyyy-MM-dd");
        string userToDate = args.Length > 3 ? args[3].Trim() : args[2].Trim();

        if (equipmentId.Length == 0 || clientId.Length == 0 || userFromDate.Length == 0)
            throw new ConsoleException(23, []);

        Client? client = ClientHandler.GetClientById(clientId);
        Equipment? equipment = RentalHandler.GetEquipmentById(equipmentId);

        if (client == null) throw new ConsoleException(25, new[] { clientId });
        if (equipment == null) throw new ConsoleException(24, new[] { equipmentId });

        DateTime parsedFromDate = ParseDate(userFromDate);
        DateTime parsedToDate = ParseDate(userToDate);
        RentalHandler.RentEquipment(equipment, client, parsedFromDate, parsedToDate);
        Console.WriteLine($"Client {client.Id} rented {equipment.Id} equipment successfully.");
    }

    private static DateTime ParseDate(string date)
    {
        string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };
        if (!DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime returnDate))
            throw new ConsoleException(26, new[] { string.Join(", ", formats) });
        return returnDate;
    }

    private static void HandleEquipmentReturnPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(19, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(20, []);
        Equipment? equipment = RentalHandler.GetEquipmentById(id);
        if (equipment == null) throw new ConsoleException(21, new[] { id });
        RentalHandler.ReturnEquipment(equipment);
        Console.WriteLine($"Equipment {equipment.Id} returned successfully.");
    }

    private static void HandleEquipmentListPrompt(params string[] args)
    {
        if (RentalHandler.GetEquipmentCount() == 0) throw new ConsoleException(18, []);
        RentalHandler.GetEquipmentList(eq => Console.WriteLine(eq.ToString()));
    }

    private static void HandleEquipmentDetailPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(19, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(20, []);
        Equipment? equipment = RentalHandler.GetEquipmentById(id);
        if (equipment == null) throw new ConsoleException(21, new[] { id });
        Console.WriteLine(equipment.ToString());
    }

    private static void HandleEquipmentAvailabilityPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(19, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(20, []);
        Equipment? equipment = RentalHandler.GetEquipmentById(id);
        if (equipment == null) throw new ConsoleException(21, new[] { id });
        bool isAvailable = !RentalHandler.IsEquipmentRented(equipment);
        Console.WriteLine(isAvailable ? "Available" : "Not available" );
    }

    private static void HandleEquipmentHistoryPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(19, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(20, []);
        Equipment? equipment = RentalHandler.GetEquipmentById(id);
        if (equipment == null) throw new ConsoleException(21, new[] { id });
        int count = RentalHandler.ListEquipmentHistory(equipment, (eq) => Console.WriteLine(eq));
        if (count == 0) throw new ConsoleException(31, new[] { id });
    }

    private static void HandleReportRequestPrompt()
    {
        Console.WriteLine($"Client number: {ClientHandler.GetClientCount()}");
        Console.WriteLine($"Equipment number: {RentalHandler.GetEquipmentCount()}");
    }

    private static void HandlePaymentPrompt(params string[] args)
    {
        if (args.Length < 2) throw new ConsoleException(27, []);

        string clientId = args[0].Trim();
        string equipmentId = args[1].Trim();

        if (equipmentId.Length == 0 || clientId.Length == 0)
            throw new ConsoleException(30, []);

        Client? client = ClientHandler.GetClientById(clientId);
        Equipment? equipment = RentalHandler.GetEquipmentById(equipmentId);

        if (client == null) throw new ConsoleException(25, new[] { clientId });
        if (equipment == null) throw new ConsoleException(24, new[] { equipmentId });
        
        RentalHandler.PayEquipment(equipment, client);
        Console.WriteLine("The payment accepted!");
    }

    public static string GetValueFromUser(string headerMessage, bool isOptional, Func<string, bool>? validate = null)
    {
        string opt = isOptional ? " (optional)" : "";
        string instruction = $"{headerMessage}{opt} or type exit";
        Console.WriteLine(instruction);
        do
        {
            string value = ConsoleBot.RestoreMode ? ConsoleBot.ReadStorageNext() : Console.ReadLine() ?? "";
            if (value.Trim().Length == 0)
            {
                if (isOptional) return "";
                Console.WriteLine("The value cannot be empty, try again...");
            }
            else if (value.Trim() == "exit") throw new ConsoleException(3, []);
            else if (!validate?.Invoke(value) ?? false) continue;
            else return value;
        } while (true);
    }

    public static int GetOptionFromUser(string headerMessage, string exitMessage, string[] options)
    {
        int index;
        string instruction = $"{headerMessage} - type number:";
        for (int x = 0; x < options.Length; x++) instruction += $"\n{x + 1} : {options[x]}";
        instruction += $"\n{options.Length + 1} - [exit: {exitMessage}]";
        Console.WriteLine(instruction);
        do
        {
            bool isIndexCorrect =
                int.TryParse(ConsoleBot.RestoreMode ? ConsoleBot.ReadStorageNext() : Console.ReadLine(), out index);
            index--; //suit to indexes
            if (!isIndexCorrect || index < 0 || index > options.Length) // > instead of >= - options.Length = Exit
                Console.WriteLine("Incorrect value, try again...");
            else break;
        } while (true);

        if (index == options.Length) throw new ConsoleException(3, []);
        return index;
    }
}