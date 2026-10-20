namespace EduHub.Domain;

/// <summary>
/// Đại diện cho sinh viên trong hệ thống EduHub.
/// </summary>
public class Student
{
    public Guid StudentId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }

    /// <summary>Constructor riêng tư – dùng Create() để khởi tạo.</summary>
    private Student() { }

    /// <summary>
    /// Factory method tạo một Student hợp lệ.
    /// </summary>
    public static Student Create(string fullName, string email, DateOnly dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Họ và tên không được để trống.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        if (dateOfBirth >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày sinh phải là ngày trong quá khứ.", nameof(dateOfBirth));

        return new Student
        {
            StudentId   = Guid.NewGuid(),
            FullName    = fullName.Trim(),
            Email       = email.Trim().ToLower(),
            DateOfBirth = dateOfBirth
        };
    }

    public override string ToString() =>
        $"[Student] {FullName} | {Email} | Sinh: {DateOfBirth:dd/MM/yyyy}";
}
