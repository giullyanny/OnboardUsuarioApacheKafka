using System;

namespace ProducerMessageUser.DTO;

public struct UserResponseDTO
{
    public string Name { get; set; }
    public DateTime Create { get; set; }
    public string Phone { get; set; }

    public UserResponseDTO(string name, DateTime create, string phone)
    {
        this.Name = name;
        this.Create = create;
        this.Phone = phone;
    }
}
