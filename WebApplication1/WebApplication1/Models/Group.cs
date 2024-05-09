namespace WebApplication1.Models;

public class Group
{
    public int GroupID { get; set; }
    public string Name { get; set; }
    public List<int> StudentsIDs { get; set; }
}

public class Student
{
    public int StudentID { get; set; }
    // public string FirstName { get; set; }
    // public string LastName { get; set; }
    // public string Phone { get; set; }
    // public DateTime Birthdate { get; set; }
    
}

