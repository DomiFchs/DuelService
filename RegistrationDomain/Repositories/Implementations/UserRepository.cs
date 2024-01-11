using Microsoft.EntityFrameworkCore;
using RegistrationDomain.Repositories.Interfaces;
using RegistrationModel.Configurations;
using RegistrationModel.Entities;
using RegistrationModel.Enums;

namespace RegistrationDomain.Repositories.Implementations; 

public class UserRepository : ARepository<User>, IUserRepository {
    public UserRepository(RegistrationDbContext context) : base(context) { }
    public async Task<List<User>> GetRegisteredUsersAsync(CancellationToken ct) {
        return await Table.Where(u => u.State == EPlayerState.Registered).ToListAsync(ct);
    }
}