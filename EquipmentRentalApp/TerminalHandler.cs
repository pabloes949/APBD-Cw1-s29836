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
                case "register":
                    HandleEquipmentRegisterPrompt(args);
                    break;
                case "client-add":
                    HandleClientAddPrompt(args);
                    break;
                case "client-detail":
                    HandleClientDetailPrompt(args);
                    break;
                case "client-list":
                    HandleClientListPrompt(args);
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
            Console.WriteLine($"WARN: {e.Message}\nTry again...");
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
                register {category} {equipment-name} {quantity}
                    register new equipment
                    quantity is optional, default: 1
                client-add {client-id} {name(s) surname(s)}
                    register new client
                client-detail {client-id}
                    show client information
                client-list
                    show all clients
                client-rentals {client-id}
                    list equipment rented by client
                client-payments {client-id}
                    show if client has outstanding payments 
                rent {client-id} {equipment-id}
                    rent equipment to client
                return {equipment-id}
                    register an equipment return in system
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

    private static void HandleEquipmentRegisterPrompt(params string[] args)
    {
        Console.WriteLine("handle new equipment");
    }

    private static void HandleClientAddPrompt(params string[] args)
    {
        if (args.Length < 2) throw new ConsoleException(3, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(11, []);
        string[] namePart = args[1..];
        string fullName = string.Join(" ", namePart).Trim();
        if (fullName.Length == 0) throw new ConsoleException(12, []);
        ClientHandler.RegisterNewClient(new Client(id, fullName));
        Console.WriteLine($"Client {id} registered successfully.");
    }

    private static void HandleClientDetailPrompt(params string[] args)
    {
        if (args.Length < 1) throw new ConsoleException(13, []);
        string id = args[0].Trim();
        if (id.Length == 0) throw new ConsoleException(14, []);
        Client? client = ClientHandler.GetClientById(id);
        if (client == null) throw new ConsoleException(15, new[] { id });
        Console.WriteLine($"ID: {client.Id}; Name: {client.FullName}");
    }

    private static void HandleClientListPrompt(params string[] args)
    {
        ClientHandler.getClientList(client => Console.WriteLine($"ID: {client.Id}; Name: {client.FullName}"));
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
        Console.WriteLine("rent equipment to client");
    }

    private static void HandleEquipmentReturnPrompt(params string[] args)
    {
        Console.WriteLine("register equipment return in system");
    }

    private static void HandleEquipmentListPrompt(params string[] args)
    {
        Console.WriteLine("list registered equipment (by given category)");
    }

    private static void HandleEquipmentDetailPrompt(params string[] args)
    {
        Console.WriteLine("show equipment information");
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
}