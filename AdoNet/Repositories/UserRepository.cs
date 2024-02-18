using System.Data;
using System.Data.SqlClient;
using AdoNet.Models;

namespace AdoNet.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlConnection _connection = new(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdoNetDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

    public List<User> Get()
    {
        var users = new List<User>();

        var command = new SqlCommand("SELECT * FROM Users", _connection);

        using (_connection)
        using (command)
        {
            _connection.Open();
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

        return users;
    }

    public User? Get(int id)
    {
        var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id;", _connection);
        command.Parameters.AddWithValue("@Id", id);

        using (_connection)
        using (command)
        {
            _connection.Open();
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

    public void Insert(User user)
    {
        var command = new SqlCommand("INSERT INTO Users(Name, Email, Gender, RG, CPF, MotherName, RegistrationStatus, RegistrationDate) VALUES(@Name, @Email, @Gender, @RG, @CPF, @MotherName, @RegistrationStatus, @RegistrationDate); SELECT CAST(scope_identity() AS INT);", _connection);
        SetCommandParameters(user, command);

        using (_connection)
        using (command)
        {
            _connection.Open();
            user.Id = (int)command.ExecuteScalar();
        }
    }

    public void Update(User user)
    {
        var command =
            new SqlCommand(
                "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, MotherName = @MotherName , RegistrationStatus = @RegistrationStatus , RegistrationDate = @RegistrationDate WHERE Id = @Id;", _connection);
        SetCommandParameters(user, command);
        command.Parameters.AddWithValue("@Id", user.Id);

        using (_connection)
        using (command)
        {
            _connection.Open();
            command.ExecuteNonQuery();
        }

    }

    public void Delete(int id)
    {
        var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", _connection);
        command.Parameters.AddWithValue("@Id", id);

        using (_connection)
        using (command)
        {
            _connection.Open();
            command.ExecuteNonQuery();
        }
    }

    private static void SetCommandParameters(User user, SqlCommand command)
    {
        command.Parameters.AddWithValue("@Name", user.Name);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@Gender", user.Gender);
        command.Parameters.AddWithValue("@RG", user.RG);
        command.Parameters.AddWithValue("@CPF", user.CPF);
        command.Parameters.AddWithValue("@MotherName", user.MotherName);
        command.Parameters.AddWithValue("@RegistrationStatus", user.RegistrationStatus);
        command.Parameters.AddWithValue("@RegistrationDate", user.RegistrationDate);
    }
}
