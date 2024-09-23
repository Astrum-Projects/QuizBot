using Npgsql;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=QuizBot";

    public void AddNewUser(User user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "INSERT INTO Users (Username, ChatId, UserContact, IsAdmin) VALUES (@username, @chatId, @userContact, @isAdmin)";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@chatId", user.ChatId);
        command.Parameters.AddWithValue("@userContact", user.UserContact);
        command.Parameters.AddWithValue("@isAdmin", user.IsAdmin);
        command.ExecuteNonQuery();
        System.Console.WriteLine("came");
    }

    public  User GetUserByChatId(long chatId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT * FROM Users WHERE ChatId = @chatId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@chatId", chatId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Username = reader["Username"].ToString(),
                ChatId = (long)reader["ChatId"],
                UserContact = reader["UserContact"].ToString(),
                IsAdmin = (bool)reader["IsAdmin"]
            };
        }
        return null;
    }

    public IEnumerable<User> GetAllUsers()
    {
        var users = new List<User>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT * FROM Users";
        using var command = new NpgsqlCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User
            {
                Username = reader["Username"].ToString(),
                ChatId = (long)reader["ChatId"],
                UserContact = reader["UserContact"].ToString(),
                IsAdmin = (bool)reader["IsAdmin"]
            });
        }
        return users;
    }
}
