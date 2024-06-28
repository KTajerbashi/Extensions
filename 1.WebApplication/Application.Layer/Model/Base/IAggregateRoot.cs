using System.Reflection;
using System.Windows.Input;

namespace Application.Layer.Model.Base;
public enum ApplicationServiceStatus
{
    Ok = 1,
    NotFound = 2,
    ValidationError = 3,
    InvalidDomainState = 4,
    Exception = 5,
}
public interface IApplicationServiceResult
{
    IEnumerable<string> Messages { get; }
    ApplicationServiceStatus Status { get; set; }
}

public abstract class ApplicationServiceResult : IApplicationServiceResult
{
    protected readonly List<string> _messages = new();

    public IEnumerable<string> Messages => _messages;

    public ApplicationServiceStatus Status { get; set; }

    public void AddMessage(string error) => _messages.Add(error);
    public void AddMessages(IEnumerable<string> errors) => _messages.AddRange(errors);
    public void ClearMessages() => _messages.Clear();

}

/// <summary>
/// توسط این تعیین میکنیم این رکورد توسط کی اضافه یا ویرایش شده است 
/// اطللاعات مربوط به رخداد ها
/// </summary>
public interface IAuditableEntity
{
}

public interface IAggregateRoot
{
    /// <summary>
    /// رخداد های موجودیت را حذف میکند 
    /// </summary>
    void ClearEvents();
    /// <summary>
    /// تمام رویداد های که روی موجودیت اتفاق می افتد را نگه میدارد
    /// </summary>
    /// <returns></returns>
    IEnumerable<IDomainEvent> GetEvents();
}

public abstract class Entity<TId> : IAuditableEntity
          where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{

    public TId Id { get; protected set; }

    protected Entity() { }


    #region Equality Check
    public bool Equals(Entity<TId>? other) => this == other;
    public override bool Equals(object? obj) =>
         obj is Entity<TId> otherObject && Id.Equals(otherObject.Id);

    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
        => !(right == left);

    #endregion
}
public abstract class Entity : Entity<long>
{

}

/// <summary>
/// پیاده سازی الگوی AggregateRoot
/// توضیحات کامل در مورد این الگو را در آدرس زیر می‌توانید مشاهده نمایید
/// https://martinfowler.com/bliki/DDD_Aggregate.html
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct,
          IComparable,
          IComparable<TId>,
          IConvertible,
          IEquatable<TId>,
          IFormattable
{
    /// <summary>
    /// لیست Eventهای مربوطه را نگهداری می‌کند        
    /// </summary>
    private readonly List<IDomainEvent> _events;
    protected AggregateRoot() => _events = new();

    /// <summary>
    /// سازنده Aggregate برای ایجاد Aggregate از روی Eventها
    /// </summary>
    /// <param name="events">در صورتی که Event از قبل وجود داشته باشد توسط این پارامتر به Aggregate ارسال می‌گردد</param>
    public AggregateRoot(IEnumerable<IDomainEvent> events)
    {
        if (events == null || !events.Any()) return;
        foreach (var @event in events)
        {
            Mutate(@event);
        }
    }
    /// <summary>
    /// ممکن است بجای StateBase  کار کردن
    /// EventBase کار کنیم
    /// </summary>
    /// <param name="event"></param>
    protected void Apply(IDomainEvent @event)
    {
        Mutate(@event);
        AddEvent(@event);
    }

    private void Mutate(IDomainEvent @event)
    {
        var onMethod = GetType().GetMethod("On", BindingFlags.Instance | BindingFlags.NonPublic, [@event.GetType()]);
        onMethod.Invoke(this, new[] { @event });
    }

    /// <summary>
    /// یک Event جدید به مجموعه Eventهای موجود در این Aggregate اضافه می‌کند.
    /// مسئولیت ساخت و ارسال Event به عهده خود Aggregateها می‌باشد.
    /// </summary>
    /// <param name="event"></param>
    protected void AddEvent(IDomainEvent @event) => _events.Add(@event);

    /// <summary>
    /// لیستی از Eventهای رخداده برای Aggregate را به صورت فقط خواندی و غیر قابل تغییر را بازگشت می‌دهد
    /// </summary>
    /// <returns>لیست Eventها</returns>
    public IEnumerable<IDomainEvent> GetEvents() => _events.AsEnumerable();

    /// <summary>
    /// Eventهای موجود در این Aggregate را پاک می‌کند
    /// </summary>
    public void ClearEvents() => _events.Clear();
}



public abstract class AggregateRoot : AggregateRoot<long>
{

}
