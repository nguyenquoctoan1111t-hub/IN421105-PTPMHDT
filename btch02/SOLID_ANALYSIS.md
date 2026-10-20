# Bài tập Ch02 – Refactoring EduHub (ClubManager)

## Phân tích vi phạm SOLID trong ClubManager ban đầu

### 1. **SRP – Single Responsibility Principle** ❌
> "Một class chỉ nên có một lý do để thay đổi"

Trong thiết kế ban đầu (monolithic ClubManager), một class đảm nhiệm:
- Logic đăng ký thành viên
- Tính toán phí
- Gửi email thông báo
- Lưu trữ dữ liệu

→ **Vi phạm**: Class có nhiều hơn một lý do để thay đổi.  
→ **Fix**: Tách thành `RegisterMemberUseCase`, `CalculateFeeUseCase`, `DeactivateMemberUseCase`.

---

### 2. **OCP – Open/Closed Principle** ❌
> "Mở để mở rộng, đóng để chỉnh sửa"

Cách tính phí dùng `if/else` hoặc `switch` trực tiếp trong class:
```csharp
// Vi phạm OCP
if (type == "basic") fee = 100000 * months;
else if (type == "premium") fee = 180000 * months;
// Thêm VIP phải sửa code cũ → vi phạm
```
→ **Fix**: Dùng **Strategy Pattern** (`IFeeCalculationStrategy`) + **Factory Pattern** (`FeeStrategyFactory`) → thêm loại mới không cần sửa code cũ.

---

### 3. **ISP – Interface Segregation Principle** ❌
> "Client không nên bị ép implement những interface mà nó không dùng"

Repository ban đầu có một interface khổng lồ với cả read lẫn write.  
→ **Fix**: Tách `IMemberRepository` và `IClubRepository` riêng biệt theo từng aggregate.

---

### 4. **DIP – Dependency Inversion Principle** ❌
> "Module cấp cao không nên phụ thuộc trực tiếp vào module cấp thấp"

Use cases phụ thuộc trực tiếp vào class cụ thể:
```csharp
// Vi phạm DIP
private InMemoryStorage _storage = new InMemoryStorage(); // concrete!
```
→ **Fix**: Mọi use case inject qua `IMemberRepository`, `IFeeCalculationStrategy` (interface) → `InMemoryMemberRepository` là concrete implementation ở Infrastructure layer.

---

## Kiến trúc sau Refactoring

```
ClubManager.Domain/
├── Entities/         Member, Club, MembershipType
├── Events/           IDomainEvent, MemberRegisteredEvent, MemberDeactivatedEvent
└── Interfaces/       IMemberRepository, IClubRepository,
                      IFeeCalculationStrategy, IDomainEventHandler<T>

ClubManager.Application/
└── UseCases/         RegisterMemberUseCase, CalculateFeeUseCase, DeactivateMemberUseCase

ClubManager.Infrastructure/
├── Repositories/     InMemoryMemberRepository, InMemoryClubRepository
├── Strategies/       BasicFeeStrategy, PremiumFeeStrategy, VipFeeStrategy
├── Factories/        FeeStrategyFactory
├── Decorators/       LoggingFeeStrategyDecorator
└── EventHandlers/    WelcomeEmailHandler, MemberDeactivatedAuditHandler

ClubManager.Tests/    7+ xUnit tests (Arrange-Act-Assert)
```

## Design Patterns áp dụng

| Pattern | Class | Mục đích |
|---------|-------|----------|
| **Factory** | `FeeStrategyFactory` | Tạo strategy đúng loại theo MembershipType |
| **Strategy** | `IFeeCalculationStrategy` + 3 implementations | Đổi thuật toán tính phí tại runtime |
| **Decorator** | `LoggingFeeStrategyDecorator` | Thêm logging không sửa strategy gốc |
| **Observer** | `IDomainEventHandler<T>` + handlers | Phản ứng với domain events |
