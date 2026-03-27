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
            3 => $"The client-add command expects client-id and user-name arguments.",
            4 => $"The equipment {args[0]} is already returned.",
            5 => $"The equipment {args[0]} cannot be returned before the date of rental.",
            6 => $"There is no unsettled payment of {args[0]} for {args[1]} client.",
            7 => $"The payment for {args[0]} can be accepted only after a return.",
            8 => $"The client with {args[0]} id is already registered in system.",
            9 => $"The client with {args[0]} id has outstanding payments for {args[0]} days and cannot rent any more equipment.",
            10 => "Invalid return date. Please provide a future date.",
            11 => "The client-add command client-id cannot be empty value",
            12 => "The client-add command user-name cannot be empty value",
            13 => $"The client-detail command expects client-id argument.",
            14 => "The client-detail command client-id cannot be empty value",
            15 => $"The client with {args[0]} id does not exist.",
            _ => ""
        };
    }
}