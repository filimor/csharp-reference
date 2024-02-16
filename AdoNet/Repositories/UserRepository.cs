using System.Data;
using System.Data.SqlClient;
using AdoNet.Models;

namespace AdoNet.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlConnection _connection = new ("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AdoNetDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

    public List<User> Get()
    {
        var users = new List<User>();

        var command = new SqlCommand("SELECT * FROM Users", _connection);

        using (_connection)
        {
            _connection.Open();

            using (command)
            {
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    users.Add(new User()
                    {
                        Id = dataReader.GetInt32("Id"),
                        Name = dataReader.GetString("Name"),
                        Email = dataReader.GetString("Email"),
                        Gender = dataReader.GetString("Gender"),
                        RG = dataReader.GetString("RG"),
                        CPF = dataReader.GetString("CPF"),
                        MotherName = dataReader.GetString("MotherName"),
                        RegistrationStatus = dataReader.GetString("RegistrationStatus"),
                        RegistrationDate = dataReader.GetDateTimeOffset(8)
                    });
                }
            }
        }


        return users;
    }

    public User? Get(int id)
    {
        var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", _connection);
        command.Parameters.AddWithValue("@Id", id);

        using (_connection)
        {
            _connection.Open();

            using (command)
            {
                var dataReader = command.ExecuteReader();

                if (!dataReader.Read())
                {
                    return null;
                }

                return new User()
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name"),
                    Email = dataReader.GetString("Email"),
                    Gender = dataReader.GetString("Gender"),
                    RG = dataReader.GetString("RG"),
                    CPF = dataReader.GetString("CPF"),
                    MotherName = dataReader.GetString("MotherName"),
                    RegistrationStatus = dataReader.GetString("RegistrationStatus"),
                    RegistrationDate = dataReader.GetDateTimeOffset(8)
                };
            }
        }


    }

    public void Insert(User user)
    {

    }

    public void Update(User user)
    {

    }

    public void Delete(int id)
    {

    }
}
