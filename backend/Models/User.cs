namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Pass { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime Created { get; set; }
}