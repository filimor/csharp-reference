using System.Data;
using System.Data.SqlClient;
using AdoNet.Models;

namespace AdoNet.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlConnection _connection =
        new(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdoNetDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

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
                users.Add(new User
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
        var command1 =
            new SqlCommand(
                "SELECT da.Id, da.Name, da.ZipCode, da.State, da.City, da.District, da.Street, da.Number, da.Compliment FROM Users u LEFT JOIN DeliveryAddresses da ON u.Id = da.UserId WHERE u.Id = @Id;",
                _connection);
        command1.Parameters.AddWithValue("@Id", id);

        var command2 = new SqlCommand(
            "SELECT d.Id, d.Name FROM Users u LEFT JOIN UsersDepartments ud ON u.Id = ud.UserId LEFT JOIN Departments d ON ud.DepartmentId = d.Id WHERE u.Id = @Id;",
            _connection);
        command2.Parameters.AddWithValue("@Id", id);

        var command3 = new SqlCommand(
            "SELECT u.Id, u.Name, u.Email, u.Gender, u.RG, u.CPF, u.MotherName, u.RegistrationStatus, u.RegistrationDate, c.Id ContactId, c.PhoneNumber, c.MobilePhone FROM Users u LEFT JOIN Contacts c ON u.Id = c.UserId WHERE u.Id = @Id;",
            _connection);
        command3.Parameters.AddWithValue("@Id", id);


        using (_connection)
        {
            var deliveryAddresses = new List<DeliveryAddress>();
            var departments = new List<Department>();

            _connection.Open();

            using (command1)
            {
                var dataReader = command1.ExecuteReader();

                while (dataReader.Read())
                {
                    var address = new DeliveryAddress
                    {
                        Id = dataReader.GetInt32("Id"),
                        Name = dataReader.GetString("Name"),
                        Street = dataReader.GetString("Street"),
                        Number = dataReader.GetString("Number"),
                        Compliment = dataReader.GetString("Compliment"),
                        District = dataReader.GetString("District"),
                        City = dataReader.GetString("City"),
                        State = dataReader.GetString("State"),
                        ZipCode = dataReader.GetString("ZipCode")
                    };

                    deliveryAddresses.Add(address);
                }

                dataReader.Close();
            }

            using (command2)
            {
                var dataReader = command2.ExecuteReader();

                while (dataReader.Read())
                {
                    var department = new Department
                    {
                        Id = dataReader.GetInt32("Id"), Name = dataReader.GetString("Name")
                    };

                    departments.Add(department);
                }

                dataReader.Close();
            }

            using (command3)
            {
                var dataReader = command3.ExecuteReader();

                if (!dataReader.Read())
                {
                    return null;
                }

                return new User
                {
                    Id = dataReader.GetInt32("Id"),
                    Name = dataReader.GetString("Name"),
                    Email = dataReader.GetString("Email"),
                    Gender = dataReader.GetString("Gender"),
                    RG = dataReader.GetString("RG"),
                    CPF = dataReader.GetString("CPF"),
                    MotherName = dataReader.GetString("MotherName"),
                    RegistrationStatus = dataReader.GetString("RegistrationStatus"),
                    RegistrationDate = dataReader.GetDateTimeOffset(8),
                    Contact = new Contact
                    {
                        Id = dataReader.GetInt32("ContactId"),
                        PhoneNumber = dataReader.GetString("PhoneNumber"),
                        MobilePhone = dataReader.GetString("MobilePhone")
                    },
                    DeliveryAddresses = deliveryAddresses,
                    Departments = departments
                };
            }
        }
    }

    public void Insert(User user)
    {
        var command = new SqlCommand(
            "INSERT INTO Users(Name, Email, Gender, RG, CPF, MotherName, RegistrationStatus, RegistrationDate) VALUES(@Name, @Email, @Gender, @RG, @CPF, @MotherName, @RegistrationStatus, @RegistrationDate); SELECT CAST(scope_identity() AS INT);",
            _connection);
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
                "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, MotherName = @MotherName , RegistrationStatus = @RegistrationStatus , RegistrationDate = @RegistrationDate WHERE Id = @Id;",
                _connection);
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
