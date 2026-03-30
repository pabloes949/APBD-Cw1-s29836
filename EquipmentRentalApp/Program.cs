namespace EquipmentRentalApp;

class Program
{
    static void Main()
    {
        RentalHandler.Load();
        ClientHandler.Load();
        TerminalHandler.DisplayInitialPrompt();
    }
}