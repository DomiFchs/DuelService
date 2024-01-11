using RegistrationModel.Entities;

namespace RegistrationDomain.Repositories.Interfaces; 

public interface IUserRepository : IRepository<User> {
    Task<List<User>> GetRegisteredUsersAsync(CancellationToken ct);
}