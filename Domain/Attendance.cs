namespace EduHub.Domain;

/// <summary>
/// Đại diện cho bản ghi điểm danh của một sinh viên trong một buổi học.
/// Khoá ngoại: EnrollmentId → Enrollment.
/// </summary>
public class Attendance
{
    public Guid AttendanceId { get; private set; }

    // Khoá ngoại tham chiếu Enrollment
    public Guid EnrollmentId { get; private set; }

    public DateOnly SessionDate { get; private set; }
    public bool IsPresent { get; private set; }

    /// <summary>Constructor riêng tư – dùng Create() để khởi tạo.</summary>
    private Attendance() { }

    /// <summary>
    /// Factory method tạo bản ghi điểm danh hợp lệ.
    /// </summary>
    public static Attendance Create(Guid enrollmentId, DateOnly sessionDate, bool isPresent)
    {
        if (enrollmentId == Guid.Empty)
            throw new ArgumentException("EnrollmentId không hợp lệ.", nameof(enrollmentId));

        if (sessionDate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày buổi học không được là ngày trong tương lai.", nameof(sessionDate));

        return new Attendance
        {
            AttendanceId = Guid.NewGuid(),
            EnrollmentId = enrollmentId,
            SessionDate  = sessionDate,
            IsPresent    = isPresent
        };
    }

    public override string ToString() =>
        $"[Attendance] Enrollment {EnrollmentId} | {SessionDate:dd/MM/yyyy} | {(IsPresent ? "Có mặt ✅" : "Vắng ❌")}";
}
