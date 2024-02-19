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
        using (_connection)
        {
            _connection.Open();

            var users = new List<User>();
            var command = new SqlCommand("SelectUsers", _connection) { CommandType = CommandType.StoredProcedure };
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

            return users;
        }
    }

    public User? Get(int id)
    {
        using (_connection)
        {
            _connection.Open();

            var command3 = new SqlCommand(
                "SELECT u.Id, u.Name, u.Email, u.Gender, u.RG, u.CPF, u.MotherName, u.RegistrationStatus, u.RegistrationDate, c.Id ContactId, c.PhoneNumber, c.MobilePhone FROM Users u LEFT JOIN Contacts c ON u.Id = c.UserId WHERE u.Id = @Id;",
                _connection);
            command3.Parameters.AddWithValue("@Id", id);
            var dataReader3 = command3.ExecuteReader();

            if (!dataReader3.Read())
            {
                return null;
            }

            var deliveryAddresses = new List<DeliveryAddress>();
            var command1 =
                new SqlCommand(
                    "SELECT da.Id, da.Name, da.ZipCode, da.State, da.City, da.District, da.Street, da.Number, da.Compliment FROM Users u LEFT JOIN DeliveryAddresses da ON u.Id = da.UserId WHERE u.Id = @Id;",
                    _connection);
            command1.Parameters.AddWithValue("@Id", id);
            var dataReader1 = command1.ExecuteReader();

            while (dataReader1.Read())
            {
                var address = new DeliveryAddress
                {
                    Id = dataReader1.GetInt32("Id"),
                    Name = dataReader1.GetString("Name"),
                    Street = dataReader1.GetString("Street"),
                    Number = dataReader1.GetString("Number"),
                    Compliment = dataReader1.GetString("Compliment"),
                    District = dataReader1.GetString("District"),
                    City = dataReader1.GetString("City"),
                    State = dataReader1.GetString("State"),
                    ZipCode = dataReader1.GetString("ZipCode")
                };

                deliveryAddresses.Add(address);
            }

            dataReader1.Close();

            var departments = new List<Department>();
            var command2 = new SqlCommand(
                "SELECT d.Id, d.Name FROM Users u LEFT JOIN UsersDepartments ud ON u.Id = ud.UserId LEFT JOIN Departments d ON ud.DepartmentId = d.Id WHERE u.Id = @Id;",
                _connection);
            command2.Parameters.AddWithValue("@Id", id);
            var dataReader2 = command2.ExecuteReader();

            while (dataReader2.Read())
            {
                var department =
                    new Department { Id = dataReader2.GetInt32("Id"), Name = dataReader2.GetString("Name") };

                departments.Add(department);
            }

            dataReader2.Close();

            return new User
            {
                Id = dataReader3.GetInt32("Id"),
                Name = dataReader3.GetString("Name"),
                Email = dataReader3.GetString("Email"),
                Gender = dataReader3.GetString("Gender"),
                RG = dataReader3.GetString("RG"),
                CPF = dataReader3.GetString("CPF"),
                MotherName = dataReader3.GetString("MotherName"),
                RegistrationStatus = dataReader3.GetString("RegistrationStatus"),
                RegistrationDate = dataReader3.GetDateTimeOffset(8),
                Contact = new Contact
                {
                    Id = dataReader3.GetInt32("ContactId"),
                    PhoneNumber = dataReader3.GetString("PhoneNumber"),
                    MobilePhone = dataReader3.GetString("MobilePhone")
                },
                DeliveryAddresses = deliveryAddresses,
                Departments = departments
            };
        }
    }

    public void Insert(User user)
    {
        using (_connection)
        {
            _connection.Open();

            var sqlTransaction = _connection.BeginTransaction();
            var departments = user.Departments?.Select(d => d.Id).ToList();

            try
            {
                var command1 = new SqlCommand(
                    "INSERT INTO Users(Name, Email, Gender, RG, CPF, MotherName, RegistrationStatus, RegistrationDate) VALUES(@Name, @Email, @Gender, @RG, @CPF, @MotherName, @RegistrationStatus, @RegistrationDate); SELECT CAST(scope_identity() AS INT);",
                    _connection, sqlTransaction);
                SetCommandParameters(user, command1);

                user.Id = (int)command1.ExecuteScalar();

                departments?.ForEach(departmentId =>
                {
                    var command2 =
                        new SqlCommand(
                            "INSERT INTO UsersDepartments(UserId, DepartmentId) VALUES(@UserId, @DepartmentId);",
                            _connection, sqlTransaction);
                    command2.Parameters.AddWithValue("@UserId", user.Id);
                    command2.Parameters.AddWithValue("@DepartmentId", departmentId);
                    command2.ExecuteNonQuery();
                });
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                throw;
            }

            sqlTransaction.Commit();
        }
    }

    public void Update(User user)
    {
        using (_connection)
        {
            _connection.Open();

            var command =
                new SqlCommand(
                    "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, MotherName = @MotherName , RegistrationStatus = @RegistrationStatus , RegistrationDate = @RegistrationDate WHERE Id = @Id;",
                    _connection);
            SetCommandParameters(user, command);
            command.Parameters.AddWithValue("@Id", user.Id);

            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (_connection)
        {
            _connection.Open();

            var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", _connection);
            command.Parameters.AddWithValue("@Id", id);

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
