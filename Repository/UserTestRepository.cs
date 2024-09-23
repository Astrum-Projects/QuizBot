using Npgsql;

public class UserTestRepository : IUserTestRepository
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=QuizBot";

    public void SaveUserTest(UserTest userTest)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "INSERT INTO UserTest (UserID, SubjectId, TestID, UserAnswer, Score, TestDate) VALUES (@userId, @subjectId, @testId, @userAnswer, @score, @testDate)";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@userId", userTest.UserID);
        command.Parameters.AddWithValue("@subjectId", userTest.SubjectId); // Add SubjectId
        command.Parameters.AddWithValue("@testId", userTest.TestID);
        command.Parameters.AddWithValue("@userAnswer", userTest.UserAnswer);
        command.Parameters.AddWithValue("@score", userTest.Score);
        command.Parameters.AddWithValue("@testDate", userTest.TestDate);
        command.ExecuteNonQuery();
    }

    public IEnumerable<UserTest> GetUserTests(int userId, int subjectId)
    {
        var userTests = new List<UserTest>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT * FROM UserTest WHERE UserID = @userId AND SubjectId = @subjectId"; // Filter by SubjectId
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@subjectId", subjectId); // Update parameter name
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            userTests.Add(new UserTest
            {
                UserTestID = (int)reader["UserTestID"],
                UserID = (int)reader["UserID"],
                SubjectId = (int)reader["SubjectId"], // Read SubjectId
                TestID = (int)reader["TestID"],
                UserAnswer = reader["UserAnswer"].ToString(),
                Score = (int)reader["Score"],
                TestDate = (DateTime)reader["TestDate"]
            });
        }
        return userTests;
    }
}
