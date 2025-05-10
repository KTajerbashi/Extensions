using System.Collections.Concurrent;

namespace Framework.Basesource.Apps.SendEmail.Data;
public sealed class DatabaseContext : IDisposable
{
    // Thread-safe collections with auto-incrementing IDs
    private readonly ConcurrentDictionary<int, UserModel> _userData = new();
    private readonly ConcurrentDictionary<int, EmailModel> _emailData = new();
    private readonly ConcurrentQueue<EmailModel> _emailQueue = new();
    private int _lastUserId = 0;
    private int _lastEmailId = 0;

    // Singleton implementation
    private static readonly Lazy<DatabaseContext> _instance = new(() => new DatabaseContext());
    public static DatabaseContext Instance => _instance.Value;

    private DatabaseContext()
    {
        // Initialize with admin user
        AddUser(new UserModel("Admin", "Admin", "admin@mail.com"));
    }

    // User operations
    public IReadOnlyCollection<UserModel> Users => _userData.Values.ToList().AsReadOnly();
    public IReadOnlyCollection<EmailModel> Emails => _emailData.Values.ToList().AsReadOnly();
    public bool HasPendingEmails => !_emailQueue.IsEmpty;

    public UserModel? GetUser(int id) => _userData.TryGetValue(id, out var user) ? user : null;
    public EmailModel? GetEmail(int id) => _emailData.TryGetValue(id, out var email) ? email : null;

    public UserModel AddUser(UserModel user)
    {
        var id = Interlocked.Increment(ref _lastUserId);
        var newUser = user with { Id = id };
        _userData.TryAdd(id, newUser);
        return newUser;
    }

    public EmailModel AddEmail(EmailModel email)
    {
        var id = Interlocked.Increment(ref _lastEmailId);
        var newEmail = email with { Id = id };
        _emailData.TryAdd(id, newEmail);
        _emailQueue.Enqueue(newEmail);
        return newEmail;
    }

    public bool UpdateUser(UserModel user)
    {
        if (!_userData.ContainsKey(user.Id)) return false;
        _userData[user.Id] = user;
        return true;
    }

    public bool UpdateEmail(EmailModel email)
    {
        if (!_emailData.ContainsKey(email.Id)) return false;
        _emailData[email.Id] = email;
        return true;
    }

    public bool RemoveUser(int id) => _userData.TryRemove(id, out _);
    public bool RemoveEmail(int id) => _emailData.TryRemove(id, out _);

    public EmailModel? DequeueEmail() => _emailQueue.TryDequeue(out var email) ? email : null;

    public void ClearAllData()
    {
        _userData.Clear();
        _emailData.Clear();
        _emailQueue.Clear();
        _lastUserId = 0;
        //_emailQueue = 0;

        // Re-add admin user
        AddUser(new UserModel("Admin", "Admin", "admin@mail.com"));
    }

    public void Dispose()
    {
        ClearAllData();
        GC.SuppressFinalize(this);
    }
}

// Enhanced model classes with validation
public record UserModel(string Name, string Family, string Email)
{
    public int Id { get; set; }
    private string _email = Email;

    public string Email
    {
        get => _email;
        set => _email = IsValidEmail(value) ? value :
            throw new ArgumentException("Invalid email format");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

public record EmailModel(string Email, string Subject, string Body)
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public EmailStatus Status { get; set; } = EmailStatus.Pending;

    public string ShortBody => Body.Length > 50 ? Body[..47] + "..." : Body;
}

public enum EmailStatus
{
    Pending,
    Sent,
    Failed
}