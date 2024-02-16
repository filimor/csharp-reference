using AdoNet.Models;

namespace AdoNet.Repositories;

internal interface IUserRepository
{
    public List<User> Get();
    public User? Get(int id);
    public void Insert(User user);
    public void Update(User user);
    public void Delete(int id);
}
