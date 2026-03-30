namespace EquipmentRentalApp;

public class ConsoleException : Exception
{
    private readonly int _code;
    private readonly string[] _args;

    public ConsoleException(int code, string[] args) : base(_printMessage(code, args))
    {
        this._code = code;
        this._args = args;
    }

    private static string _printMessage(int code, string[] args)
    {
        return code switch
        {
            1 => $"The equipment {args[0]} is already rented for {args[1]} client.",
            2 => $"The equipment {args[0]} is not currently rented.",
            3 => "Filling process aborted.",
            4 => $"The equipment {args[0]} is already returned.",
            5 => $"The equipment {args[0]} cannot be returned before the date of rental.",
            6 => $"There is no unsettled payment of {args[0]} for {args[1]} client.",
            7 => $"The payment for {args[0]} can be accepted only after a return.",
            8 => $"The client with {args[0]} id is already registered in system.",
            9 => $"The client with {args[0]} id has outstanding payments for {args[0]} days and cannot rent any more equipment.",
            10 => "Invalid expected day of return. Please provide a future date.",
            11 => $"The equipment {args[0]} is not rented for {args[1]} client.",
            12 => "The client-add command user-name cannot be empty value",
            13 => "The command expects client-id argument.",
            14 => "The command client-id cannot be empty value",
            15 => $"The client with {args[0]} id does not exist.",
            16 => "The list of clients is empty.",
            17 => "Incorrect value, try again...",
            18 => "The list of equipment is empty.",
            19 => "The command expects equipment-id argument.",
            20 => "The command equipment-id cannot be empty value",
            21 => $"The equipment with {args[0]} id does not exist.",
            22 => "The rent command expects client-id equipment-id return-date arguments.",
            23 => $"The rent command client-id equipment-id return-date arguments cannot be empty value.",
            24 => $"The equipment with {args[0]} id does not exist.",
            25 => $"The client with {args[0]} id does not exist.",
            26 => $"Incorrect date format. Legal format is {args[0]}",
            27 => "The payment-accept command expects client-id equipment-id arguments.",
            28 => $"The are no unpaid assets for {args[0]} client.",
            29 => $"The are no rented equipment by {args[0]} client.",
            30 => $"The payment-accept command client-id equipment-id arguments cannot be empty value.",
            _ => ""
        };
    }
}