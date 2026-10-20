namespace EduHub.Domain;

/// <summary>
/// Đại diện cho giáo viên trong hệ thống EduHub.
/// </summary>
public class Teacher
{
    public Guid TeacherId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Specialization { get; private set; } = string.Empty;

    /// <summary>Constructor riêng tư – dùng Create() để khởi tạo.</summary>
    private Teacher() { }

    /// <summary>
    /// Factory method tạo một Teacher hợp lệ.
    /// </summary>
    public static Teacher Create(string fullName, string email, string specialization)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Họ và tên không được để trống.", nameof(fullName));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Chuyên ngành không được để trống.", nameof(specialization));

        return new Teacher
        {
            TeacherId      = Guid.NewGuid(),
            FullName       = fullName.Trim(),
            Email          = email.Trim().ToLower(),
            Specialization = specialization.Trim()
        };
    }

    public override string ToString() =>
        $"[Teacher] {FullName} | {Email} | {Specialization}";
}
