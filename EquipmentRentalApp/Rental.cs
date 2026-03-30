namespace EquipmentRentalApp;

public class Rental
{
    public Client Client { get; }
    public Equipment Equipment { get; }
    private DateTime _fromDate;
    private DateTime _toDate;
    private DateTime? _returnDate = null;
    private bool _payment = false;
    public bool IsRented => this._returnDate == null;
    public int UnpaidDays
    {
        get
        {
            if (this._payment) return 0;
            DateTime returnDate = this._returnDate ?? DateTime.Now;
            var delayPeriod = (returnDate - this._toDate).Days;
            return delayPeriod > 0 ? delayPeriod : 0;
        }
    }

    public Rental(Equipment equipment, Client client, DateTime fromDate, DateTime toDate)
    {
        this.Client = client;
        this.Equipment = equipment;
        this._fromDate = fromDate;
        this._toDate = toDate;
    }

    public void RegisterReturn()
    {
        RegisterReturn(DateTime.Now);
    }

    public void RegisterReturn(DateTime returnDate)
    {
        if (!this.IsRented) throw new ConsoleException(4, new[] { this.Equipment.Id });
        if (returnDate < this._fromDate) throw new ConsoleException(5, new[] { this.Equipment.Id });
        this._returnDate = returnDate;
    }

    public void AcceptPayment()
    {
        if (this.IsRented) throw new ConsoleException(7, new[] { this.Equipment.Id });
        this._payment = true;
    }

    public bool IsRentedFor(Client client)
    {
        return this.IsRented && this.Client == client;
    }    
    
    public override string ToString()
    {
        return
            $"[Rental] Client ID: {this.Client.Id}; Client: {this.Client.Personalia}; Equipment ID: {this.Equipment.Id}; Equipment: {this.Equipment.Description}; Rental date: {this._fromDate.ToString("yyyy-MM-dd")}; Expected return date: {this._toDate.ToString("yyyy-MM-dd")}; Actual return date: {(this.IsRented ?"---": this._returnDate?.ToString("yyyy-MM-dd"))}; Unpaid days: {this.UnpaidDays}";
    }
}