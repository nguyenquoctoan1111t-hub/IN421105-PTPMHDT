namespace EduHub.Domain;

/// <summary>
/// Đại diện cho một khoá học trong hệ thống EduHub.
/// Khoá ngoại: TeacherId tham chiếu đến Teacher.
/// </summary>
public class Course
{
    public Guid CourseId { get; private set; }
    public string CourseName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    // Khoá ngoại tham chiếu Teacher
    public Guid TeacherId { get; private set; }

    /// <summary>Constructor riêng tư – dùng Create() để khởi tạo.</summary>
    private Course() { }

    /// <summary>
    /// Factory method tạo một Course hợp lệ.
    /// </summary>
    public static Course Create(string courseName, string description, Guid teacherId)
    {
        if (string.IsNullOrWhiteSpace(courseName))
            throw new ArgumentException("Tên khoá học không được để trống.", nameof(courseName));

        if (courseName.Trim().Length < 3)
            throw new ArgumentException("Tên khoá học phải có ít nhất 3 ký tự.", nameof(courseName));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Mô tả không được để trống.", nameof(description));

        if (teacherId == Guid.Empty)
            throw new ArgumentException("Khoá học phải có giáo viên phụ trách.", nameof(teacherId));

        return new Course
        {
            CourseId    = Guid.NewGuid(),
            CourseName  = courseName.Trim(),
            Description = description.Trim(),
            TeacherId   = teacherId
        };
    }

    public override string ToString() =>
        $"[Course] {CourseName} | {Description}";
}
