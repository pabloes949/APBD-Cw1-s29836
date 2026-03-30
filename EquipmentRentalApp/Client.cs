namespace EquipmentRentalApp;

public abstract class Client
{
    public string Id { get; protected set; }

    public string Personalia
    {
        get { return $"{this.Name} {this.Surname}"; }
    }

    protected string? Name { get; }
    protected string? Surname { get; }
    protected string? Email { get; }


    protected Client()
    {
        this.Name = TerminalHandler.GetValueFromUser("Give a name", false);
        this.Surname = TerminalHandler.GetValueFromUser("Give a surname", false);
        this.Email = TerminalHandler.GetValueFromUser("Give an email address", true, (val) =>
        {
            if (val.Contains('@')) return true;
            Console.WriteLine("Invalid email address, try again...");
            return false;
        });
    }

    public static Dictionary<string, Func<Client>> CreateClient = new()
    {
        { "Student", () => new Student() },
        { "Employee", () => new Employee() },
        { "Guest", () => new Guest() }
    };

    public static string[] GetClientTypes()
    {
        return CreateClient.Keys.ToArray();
    }
}

public abstract class Internal : Client
{
    protected Internal()
    {
        this.Id = TerminalHandler.GetValueFromUser("Give index number", false, (val) =>
        {
            if (ClientHandler.IsIdentifierRegistered(val))
            {
                Console.WriteLine("The given index number is already registered, try again...");
                return false;
            }

            return true;
        });
    }
}

public class Student : Internal
{
    private StudyField FieldOfStudy { get; set; }
    private int StartYear { get; set; }
    private bool IsActiveStudent { get; set; }
    private StudyMode ModeOfStudy { get; set; }

    private enum StudyMode
    {
        FullTime,
        PartTime
    }

    private enum StudyField
    {
        ComputerScience,
        CognitiveScience,
        InformationManagement,
        GraphicDesign,
        MultimediaArts,
        InteriorArchitecture,
        JapaneseCulture
    }

    public Student()
    {
        this.FieldOfStudy = (StudyField)TerminalHandler.GetOptionFromUser(
            "Choose field of study",
            "abort adding new student",
            Enum.GetNames(typeof(StudyField))
        ); //choose enum option by index

        this.ModeOfStudy = (StudyMode)TerminalHandler.GetOptionFromUser(
            "Choose mode of study",
            "abort adding new student",
            Enum.GetNames(typeof(StudyMode))
        );

        this.StartYear = int.Parse(TerminalHandler.GetValueFromUser("Type start year of studies", false, (val) =>
        {
            if (!int.TryParse(val, out int year))
            {
                Console.WriteLine("Invalid year value, try again...");
                return false;
            }

            if (year < 1990 || year > DateTime.Now.Year)
            {
                Console.WriteLine("The year is out of the allowed range, try again...");
                return false;
            }

            return true;
        }));

        this.IsActiveStudent = TerminalHandler.GetOptionFromUser(
            "Is student active",
            "abort adding new student",
            new[] { "yes", "no" }) == 0;
    }

    public override string ToString()
    {
        return
            $"[Student] Index number: {this.Id}; Name: {this.Name}; Surname: {this.Surname}; Subject: {FieldOfStudy.ToString()}; Mode: {ModeOfStudy.ToString()}; Active: {this.IsActiveStudent}";
    }
}

public class Employee : Internal
{
    private Position Role { get; set; }

    private enum Position
    {
        Professor,
        AssistantProfessor,
        Lecturer,
        TeachingAssistant,
        LabAssistant,
        Researcher,
        ITSupport,
        Administrator,
        DepartmentHead
    }

    public Employee()
    {
        this.Role = (Position)TerminalHandler.GetOptionFromUser(
            "Choose employee position",
            "abort adding new employe",
            Enum.GetNames(typeof(Position))
        );
    }


    public override string ToString()
    {
        return
            $"[Employee] Index number: {this.Id}; Name: {this.Name}; Surname: {this.Surname}; Position: {this.Role.ToString()}";
    }
}

public class Guest : Client
{
    private string Company { get; set; }

    public Guest()
    {
        this.Id = ClientHandler.GenerateClientId();
        this.Company = TerminalHandler.GetValueFromUser("Give company name", true);
    }

    public override string ToString()
    {
        return
            $"[Guest] Name: {this.Name}; Surname: {this.Surname}; Company: {this.Company}";
    }
}