using EduHub.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ═══════════════════════════════════════════════════════════
PrintHeader("EDUHUB – HỆ THỐNG QUẢN LÝ HỌC TẬP");
// ═══════════════════════════════════════════════════════════

// ── 1. Tạo giáo viên ────────────────────────────────────────
PrintSection("1. GIÁO VIÊN");
var teacher1 = Teacher.Create("Nguyễn Tuấn Kiệt",  "kiet.nguyen@eduhub.vn",  "Kỹ nghệ phần mềm");
var teacher2 = Teacher.Create("Trần Thị Mai",       "mai.tran@eduhub.vn",     "Lập trình hướng đối tượng");
Console.WriteLine(teacher1);
Console.WriteLine(teacher2);

// ── 2. Tạo khoá học ─────────────────────────────────────────
PrintSection("2. KHOÁ HỌC");
var course1 = Course.Create(
    "IN4211 – Phát triển phần mềm HĐT",
    "Kỹ nghệ phần mềm và lập trình hướng đối tượng với C#",
    teacher1.TeacherId);
var course2 = Course.Create(
    "IN4212 – Cơ sở dữ liệu",
    "Thiết kế CSDL quan hệ và SQL",
    teacher2.TeacherId);
Console.WriteLine(course1);
Console.WriteLine(course2);

// ── 3. Tạo sinh viên ────────────────────────────────────────
PrintSection("3. SINH VIÊN");
var student1 = Student.Create("Lê Văn An",       "an.le@student.edu.vn",     new DateOnly(2003, 5,  14));
var student2 = Student.Create("Phạm Thị Bình",   "binh.pham@student.edu.vn", new DateOnly(2002, 11, 22));
var student3 = Student.Create("Hoàng Minh Cường", "cuong.hoang@student.edu.vn", new DateOnly(2003, 3,  8));
Console.WriteLine(student1);
Console.WriteLine(student2);
Console.WriteLine(student3);

// ── 4. Đăng ký khoá học ─────────────────────────────────────
PrintSection("4. ĐĂNG KÝ KHOÁ HỌC");
var enrollment1 = Enrollment.Create(student1.StudentId, course1.CourseId);
var enrollment2 = Enrollment.Create(student2.StudentId, course1.CourseId);
var enrollment3 = Enrollment.Create(student3.StudentId, course1.CourseId);
Console.WriteLine($"✔ {student1.FullName,-22} → {course1.CourseName}");
Console.WriteLine($"✔ {student2.FullName,-22} → {course1.CourseName}");
Console.WriteLine($"✔ {student3.FullName,-22} → {course1.CourseName}");

// ── 5. Điểm danh – 3 buổi học ───────────────────────────────
PrintSection("5. ĐIỂM DANH – 3 BUỔI");
var sessions = new DateOnly[]
{
    new DateOnly(2025, 9,  1),
    new DateOnly(2025, 9,  8),
    new DateOnly(2025, 9, 15)
};

// student1: có mặt cả 3 buổi  (100%)
// student2: vắng buổi 2        (~67%)
// student3: vắng buổi 2 và 3   (~33%)
var attendances = new List<Attendance>
{
    Attendance.Create(enrollment1.EnrollmentId, sessions[0], true),
    Attendance.Create(enrollment1.EnrollmentId, sessions[1], true),
    Attendance.Create(enrollment1.EnrollmentId, sessions[2], true),

    Attendance.Create(enrollment2.EnrollmentId, sessions[0], true),
    Attendance.Create(enrollment2.EnrollmentId, sessions[1], false),
    Attendance.Create(enrollment2.EnrollmentId, sessions[2], true),

    Attendance.Create(enrollment3.EnrollmentId, sessions[0], true),
    Attendance.Create(enrollment3.EnrollmentId, sessions[1], false),
    Attendance.Create(enrollment3.EnrollmentId, sessions[2], false),
};

foreach (var a in attendances) Console.WriteLine(a);

// ── 6. Báo cáo tham dự ──────────────────────────────────────
PrintSection("6. BÁO CÁO THAM DỰ");
Console.WriteLine($"Khoá học : {course1.CourseName}");
Console.WriteLine($"Giáo viên: {teacher1.FullName}");
Console.WriteLine($"Tổng số buổi: {sessions.Length}\n");

string header  = $"{"Họ và tên",-22} {"Có mặt",7} {"Tổng",6} {"Tỉ lệ",8}   Trạng thái";
string divider = new string('─', 65);
Console.WriteLine(header);
Console.WriteLine(divider);

var rows = new[]
{
    (enrollment1, student1),
    (enrollment2, student2),
    (enrollment3, student3)
};

foreach (var (enroll, student) in rows)
{
    var sa      = attendances.Where(a => a.EnrollmentId == enroll.EnrollmentId).ToList();
    int present = sa.Count(a => a.IsPresent);
    int total   = sa.Count;
    double rate = total > 0 ? (double)present / total * 100 : 0;
    string status = rate >= 80 ? "✅ Đạt" : "⚠️  Cần chú ý";

    Console.WriteLine($"{student.FullName,-22} {present,7} {total,6} {rate,7:F1}%   {status}");
}

Console.WriteLine(divider);

// ── 7. Kiểm tra Validation ──────────────────────────────────
PrintSection("7. KIỂM TRA VALIDATION (BẮT LỖI)");
RunValidationTest("Student – tên rỗng",
    () => Student.Create("", "ok@test.com", new DateOnly(2000, 1, 1)));

RunValidationTest("Student – email không hợp lệ",
    () => Student.Create("Test User", "not-an-email", new DateOnly(2000, 1, 1)));

RunValidationTest("Student – ngày sinh trong tương lai",
    () => Student.Create("Test User", "ok@test.com", new DateOnly(2030, 1, 1)));

RunValidationTest("Course – tên quá ngắn",
    () => Course.Create("AB", "Mô tả", Guid.NewGuid()));

RunValidationTest("Attendance – ngày buổi học trong tương lai",
    () => Attendance.Create(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today.AddDays(5)), true));

Console.WriteLine("\n✔ Tất cả luồng nghiệp vụ chạy thành công!\n");

// ──────────────────────── HELPERS ───────────────────────────
static void PrintHeader(string title)
{
    string border = new string('═', 60);
    Console.WriteLine(border);
    Console.WriteLine($"  {title}");
    Console.WriteLine(border);
    Console.WriteLine();
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"── {title} " + new string('─', Math.Max(0, 50 - title.Length)));
    Console.ResetColor();
}

static void RunValidationTest(string label, Action action)
{
    try
    {
        action();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [FAIL] {label}: không bắt được lỗi!");
        Console.ResetColor();
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [OK] {label}: \"{ex.Message}\"");
        Console.ResetColor();
    }
}
