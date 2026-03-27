namespace EquipmentRentalApp;

public abstract class Client
{
    public string Id { get; }
    public string FullName { get; }
    
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public string EmailAddress { get; protected set; }
    
    public ClientType Type { get; protected set; }

    public Client(string id, string fullName)
    {
        this.Id = id;
        this.FullName = fullName;
    }
}

public enum ClientType
{
    Student,
    Employee,
    Guest,
    All,
    Internal,
    External
}

public class Student : Client
{
    public FieldOfStudy Subject { get; private set; }
    public int StartYear { get; private set; }
    public bool IsActiveStudent { get; private set; }
    public bool IsFullTime { get; private set; }

    public enum FieldOfStudy
    {
        ComputerScience,
        Automation,
        Electronics,
        MechanicalEngineering,
        BiomedicalEngineering,
        Mathematics,
        Physics,
        Management,
    }
    
    public Student(string id, string fullName) : base(id, fullName)
    {
        this.Type = ClientType.Student;
    }
}

public class Employee : Client
{
    public Role Position { get; set; }
    public enum Role
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
    
    public Employee(string id, string fullName) : base(id, fullName)
    {
        this.Type = ClientType.Employee;
    }
}

public class Guest : Client
{
    public Guest(string id, string fullName) : base(id, fullName)
    {
        this.Type = ClientType.Guest;
    }
}