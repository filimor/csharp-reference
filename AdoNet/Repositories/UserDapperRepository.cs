﻿using System.Data.SqlClient;
using AdoNetDapper.Models;
using Dapper;

namespace AdoNetDapper.Repositories;

public class UserDapperRepository : IUserRepository
{
    private static readonly SqlConnection _connection =
        new(
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AdoNetDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;");

    public List<User> Get()
    {
        return _connection.Query<User>("SelectUsers").ToList();
    }

    public User? Get(int id)
    {
        return _connection.Query<User, Contact, User>(
            "SELECT u.Id, u.Name, u.Email, u.Gender, u.RG, u.CPF, u.MotherName, u.RegistrationStatus, u.RegistrationDate, c.Id ContactId, c.PhoneNumber, c.MobilePhone FROM Users u LEFT JOIN Contacts c ON u.Id = c.UserId WHERE u.Id = @Id;",
            (user, contact) =>
            {
                user.Contact = contact;
                return user;
            },
            new { Id = id }).FirstOrDefault();
    }

    public void Insert(User user)
    {
        user.Id = _connection.Query<int>(
                "INSERT INTO Users(Name, Email, Gender, RG, CPF, MotherName, RegistrationStatus, RegistrationDate) VALUES(@Name, @Email, @Gender, @RG, @CPF, @MotherName, @RegistrationStatus, @RegistrationDate); SELECT CAST(scope_identity() AS INT);",
                user)
            .Single();
    }

    public void Update(User user)
    {
        _connection.Execute(
            "UPDATE Users SET Name = @Name, Email = @Email, Gender = @Gender, RG = @RG, CPF = @CPF, MotherName = @MotherName , RegistrationStatus = @RegistrationStatus , RegistrationDate = @RegistrationDate WHERE Id = @Id;",
            user);
    }

    public void Delete(int id)
    {
        _connection.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = id });
    }
}