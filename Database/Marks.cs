using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAttendanceMarks.Database;

public class Marks
{
    public Guid Id { get; set; }
    public int? Grade { get; set; }

    public AttendanceMark AttendanceMark { get; set; } = AttendanceMark.ABSENT;

    [ForeignKey("Student")]
    public Guid StudentId { get; set; }

    public Student Student { get; set; }

    [ForeignKey("Lesson")]
    public Guid LessonId { get; set; }
    public Lesson Lesson { get; set; }


    public class MarksConfiguration : IEntityTypeConfiguration<Marks>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Marks> builder)
        {
            builder.HasIndex(m => new { m.StudentId, m.LessonId }).IsUnique().HasDatabaseName("IX_Student_Lesson");
        }
    }
}
