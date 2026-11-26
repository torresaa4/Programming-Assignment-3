using System.Data;
using Microsoft.Data.SqlClient;
using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Data;

public class UserDatabaseAccess
{
    private readonly string database_connection;

    public UserDatabaseAccess(string Connection_String)
    {
        database_connection = Connection_String;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(database_connection);
    }



    public bool RegisterUser(RegisterModel user, string passwordHash)
{
    using (SqlConnection connection = GetConnection())
    {
        connection.Open();

        string query = @"
            INSERT INTO Users (Name, Email, PhoneNumber, Address, PasswordHash)
            VALUES (@Name, @Email, @PhoneNumber, @Address, @PasswordHash)";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = user.Name;
            command.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = user.Email;
            command.Parameters.Add("@PhoneNumber", SqlDbType.VarChar, 20).Value = user.PhoneNumber;
            command.Parameters.Add("@Address", SqlDbType.VarChar, 255).Value = user.Address;
            command.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 255).Value = passwordHash;

            int rows = command.ExecuteNonQuery();

            return rows > 0;
            
        }
    }
}

// find through email, use ? to return null or object
public User? FindUser(string email)
{
    using (SqlConnection connection = GetConnection())
    {
        connection.Open();

        string query = @"
            SELECT Id, Name, Email, PhoneNumber, Address, PasswordHash
            FROM Users
            WHERE Email = @Email";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = email;

            using (SqlDataReader read = command.ExecuteReader())
            {
                if(read.Read())
                {
                    return new User
                    {
                        Id = read.GetInt32(0),
                        Name = read.GetString(1),
                        Email = read.GetString(2),
                        PhoneNumber = read.GetString(3),
                        Address = read.GetString(4),
                        PasswordHash = read.GetString(5)
                    };
                }
            }
        }
    }
    return null;
}
public bool EmailExists(string email)
{
    using (SqlConnection connection = GetConnection())
    {
        connection.Open();

        string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = email;

            int count = (int)command.ExecuteScalar();

            return count > 0;
        }
    }
}
}

