namespace StudentAttendanceMarks.Database;

public class Student
{
    public Guid Id { get; set; }
    public byte[] Photo { get; set; }
    public string FullName { get; set; }
    public DateTime LastOnline { get; set; }
    ICollection<Marks> Marks = new List<Marks>();

}
