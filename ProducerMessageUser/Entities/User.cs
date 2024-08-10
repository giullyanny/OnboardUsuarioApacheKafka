namespace ProducerMessageUser.Entities;

public class User
{
    public string Name { get; set; }
    public DateTime Create { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }

    public User(string name, DateTime create, string phone, string password)
    {
        this.Name = name;
        this.Create = create;
        this.Phone = phone;
        this.Password = password;
    }
}
