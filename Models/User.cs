
namespace FakeUserDataGenerator.Models;

public class User
{
    public int Index { get; set; }
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
}
