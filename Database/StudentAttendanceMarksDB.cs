using Microsoft.EntityFrameworkCore;

namespace StudentAttendanceMarks.Database;

public class StudentAttendanceMarksDB : DbContext
{
    private DbSet<Student> _students => Set<Student>();
    private DbSet<Marks> _marks => Set<Marks>();
    private DbSet<Lesson> _lessons => Set<Lesson>();


    public StudentAttendanceMarksDB() => Database.EnsureCreated();


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=StudentAttendanceMarksDB.db");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Marks>()
            .HasIndex(m => new { m.StudentId, m.LessonId })
            .IsUnique()
            .HasName("IX_Student_Lesson");
    }

    public IEnumerable<Student> GetAllStudents() => _students;
    public IEnumerable<Lesson> GetAllLessons() => _lessons;
    public IEnumerable<Marks> GetAllMarks() => _marks;

    public Lesson? GetLessonByName(string name) => _lessons.FirstOrDefault(l => l.Name == name);

    public Student? GetStudentById(Guid id) => _students.FirstOrDefault(s => s.Id == id);


}