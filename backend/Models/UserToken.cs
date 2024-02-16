using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserToken
{
    public int UserId { get; set; }
    public User User { get; set; }

    public string Token { get; set; }
    public DateTime Expires { get; set; }

}