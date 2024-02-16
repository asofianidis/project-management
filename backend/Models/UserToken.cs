using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserToken
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string Token { get; set; }
    public DateTime Expires { get; set; }

}