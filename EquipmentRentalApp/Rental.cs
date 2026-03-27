namespace EquipmentRentalApp;

public class Rental
{
    private Client _client;
    private Equipment _equipment;
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

    public Rental(Equipment equipment, Client client, DateTime toDate)
    {
        this._client = client;
        this._equipment = equipment;
        this._fromDate = DateTime.Now;
        this._toDate = toDate;
    }

    public void RegisterReturn()
    {
        RegisterReturn(DateTime.Now);
    }

    public void RegisterReturn(DateTime returnDate)
    {
        if (!this.IsRented) throw new ConsoleException(4, new[] { this._equipment.Id });
        if (returnDate < this._fromDate) throw new ConsoleException(5, new[] { this._equipment.Id });
        this._returnDate = returnDate;
    }

    public void AcceptPayment()
    {
        if (this.IsRented) throw new ConsoleException(7, new[] { this._equipment.Id });
        if (this.UnpaidDays == 0) throw new ConsoleException(6, new[] { this._equipment.Id, this._client.Id });
        this._payment = true;
    }
}