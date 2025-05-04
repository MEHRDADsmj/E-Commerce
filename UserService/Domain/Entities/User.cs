using System.ComponentModel.DataAnnotations;

namespace UserService.Domain.Entities;

public class User
{
    [Key] public Guid Id { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string HashedPassword { get; set; }
    [Required] public string FullName { get; set; }
    DateTime CreatedAt { get; set; }
}