using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.ChangeDataLog.Models.Bases;

namespace WebApi.ChangeDataLog.Models.Security;

[Table("Users", Schema = "Security")]
public class UserEntity : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
