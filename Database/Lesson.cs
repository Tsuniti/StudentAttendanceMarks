namespace StudentAttendanceMarks.Database;



public class Lesson
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DayAndTime { get; set; }
    ICollection<Marks> Marks = new List<Marks>();

}
