using System;

namespace ProducerMessageUser.DTO;

public struct UserRequestDTO
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}
