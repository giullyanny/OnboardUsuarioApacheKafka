using ConsumerMessageUser.Entities;
using ConsumerMessageUser.Service;

namespace ConsumerMessageUser.Model;

public class UserModel
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserModel(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<int> Incluir(User user)
    {
        // Salva o usuário no banco de dados
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Users.Add(user);
            return await dbContext.SaveChangesAsync();
        }
    }
}