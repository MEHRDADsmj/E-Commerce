using System.ComponentModel.DataAnnotations;

namespace UserService.Domain.Entities;

public class User
{
    [Key] public Guid Id { get; private set; }
    [Required][EmailAddress] public string Email { get; private set; }
    [Required] public string HashedPassword { get; private set; }
    [Length(0, 50)] public string FullName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public bool IsEmpty() => Id == Guid.Empty;

    public User(string email, string hashedPassword, string fullName)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Email = email;
        HashedPassword = hashedPassword;
        FullName = fullName;
        ValidationContext validationContext = new ValidationContext(this);
        Validator.ValidateObject(this, validationContext, true);
    }

    public static User Empty()
    {
        return new User("null@null", "null", "null")
               {
                   CreatedAt = DateTime.UtcNow,
                   Id = Guid.Empty,
               };
    }
}