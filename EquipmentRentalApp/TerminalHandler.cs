using System.Globalization;

namespace EquipmentRentalApp;

public static class TerminalHandler
{
    public static void DisplayInitialPrompt()
    {
        Console.Clear();
        Console.WriteLine("Hello User!\nhelp - list of commands\nexit - exit program");
        ExpectUserCommand();
    }

    private static void ExpectUserCommand()
    {
        string command = Console.ReadLine() ?? "";
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
                case "help":
                    DisplayHelpPrompt();
                    break;
                case "exit":
                    HandleExitPrompt();
                    return; //RETURN!!!
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
                    HandleReportRequestPrompt(args);
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
                equipment-available
                    list only available equipment
                equipment-history {equipment-id}
                    list equipment rental history
                equipment-state {equipment-id}
                    set a state of equipment (operable, broken, repair)
                report {date-from} {date-to}
                    generate rental report in the given period of time
                payment-accept {client-id} {value}
                    register payment for delayed return
            """);
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
        Console.WriteLine("list equipment rented by client");
    }

    private static void HandleClientPaymentsPrompt(params string[] args)
    {
        Console.WriteLine("show if client has outstanding payments");
    }

    private static void HandleEquipmentRentalPrompt(params string[] args)
    {
        if (args.Length < 3) throw new ConsoleException(22, []);

        string clientId = args[0].Trim();
        string equipmentId = args[1].Trim();
        string userDate = args[2].Trim();
        if (equipmentId.Length == 0 || clientId.Length == 0 || userDate.Length == 0) throw new ConsoleException(23, []);

        Equipment? equipment = RentalHandler.GetEquipmentById(equipmentId);
        Client? client = ClientHandler.GetClientById(clientId);

        if (equipment == null) throw new ConsoleException(24, new[] { equipmentId });
        if (client == null) throw new ConsoleException(25, new[] { clientId });

        string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };

        if (!DateTime.TryParseExact(userDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime returnDate))
            throw new ConsoleException(26, new[] { string.Join(", ", formats) });

        RentalHandler.RentEquipment(equipment, client, returnDate);
    }

    private static void HandleEquipmentReturnPrompt(params string[] args)
    {
        Console.WriteLine("register equipment return in system");
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
        Console.WriteLine("check if equipment is available to be rented");
    }

    private static void HandleEquipmentHistoryPrompt(params string[] args)
    {
        Console.WriteLine("list equipment rental history");
    }

    private static void HandleReportRequestPrompt(params string[] args)
    {
        Console.WriteLine("generate rental report in the given period of time");
    }

    private static void HandlePaymentPrompt(params string[] args)
    {
        Console.WriteLine("register payment for delayed return");
    }

    public static string GetValueFromUser(string headerMessage, bool isOptional, Func<string, bool>? validate = null)
    {
        string opt = isOptional ? " (optional)" : "";
        string instruction = $"{headerMessage}{opt} or type exit";
        Console.WriteLine(instruction);
        do
        {
            string value = Console.ReadLine() ?? "";
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
            bool isIndexCorrect = int.TryParse(Console.ReadLine(), out index);
            index--; //suit to indexes
            if (!isIndexCorrect || index < 0 || index > options.Length) // > instead of >= - options.Length = Exit
                Console.WriteLine("Incorrect value, try again...");
            else break;
        } while (true);

        if (index == options.Length) throw new ConsoleException(3, []);
        return index;
    }
}