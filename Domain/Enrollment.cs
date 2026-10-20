namespace EduHub.Domain;

/// <summary>
/// Đại diện cho việc đăng ký học (Enrollment) của một sinh viên vào một khoá học.
/// Khoá ngoại: StudentId → Student, CourseId → Course.
/// </summary>
public class Enrollment
{
    public Guid EnrollmentId { get; private set; }

    // Khoá ngoại tham chiếu Student
    public Guid StudentId { get; private set; }

    // Khoá ngoại tham chiếu Course
    public Guid CourseId { get; private set; }

    public DateOnly EnrolledDate { get; private set; }

    /// <summary>Constructor riêng tư – dùng Create() để khởi tạo.</summary>
    private Enrollment() { }

    /// <summary>
    /// Factory method tạo một Enrollment hợp lệ với ngày đăng ký là hôm nay.
    /// </summary>
    public static Enrollment Create(Guid studentId, Guid courseId)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("StudentId không hợp lệ.", nameof(studentId));

        if (courseId == Guid.Empty)
            throw new ArgumentException("CourseId không hợp lệ.", nameof(courseId));

        return new Enrollment
        {
            EnrollmentId = Guid.NewGuid(),
            StudentId    = studentId,
            CourseId     = courseId,
            EnrolledDate = DateOnly.FromDateTime(DateTime.Today)
        };
    }

    public override string ToString() =>
        $"[Enrollment] Student {StudentId} ↔ Course {CourseId} | Ngày đăng ký: {EnrolledDate:dd/MM/yyyy}";
}
