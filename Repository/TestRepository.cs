using Npgsql;

public class TestRepository : ITestRepository
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=QuizBot";

  public void AddTest(Test test)
{
    using var connection = new NpgsqlConnection(_connectionString);
    connection.Open();
    
    var query = "INSERT INTO Test (SubjectId, Question, Options, CorrectOptionIndex) VALUES (@subjectId, @question, @options, @correctOptionIndex)";
    
    using var command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@subjectId", test.SubjectId);
    command.Parameters.AddWithValue("@question", test.Question);
    
    // Pass options as an array
    command.Parameters.AddWithValue("@options", test.Options.ToArray()); // Assuming test.Options is a List<string> or string[]

    command.Parameters.AddWithValue("@correctOptionIndex", test.CorrectOptionIndex);
    command.ExecuteNonQuery();
}


  public IEnumerable<Test> GetTestsBySubjectId(int subjectId)
{
    var tests = new List<Test>();
    using var connection = new NpgsqlConnection(_connectionString);
    connection.Open();
    var query = "SELECT * FROM Test WHERE SubjectId = @subjectId";
    using var command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@subjectId", subjectId);
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        tests.Add(new Test
        {
            TestId = (int)reader["TestId"],
            SubjectId = (int)reader["SubjectId"],
            Question = reader["Question"].ToString(),
            Options = reader["Options"] is string[] options ? options : new string[0], // Ensure correct handling
            CorrectOptionIndex = (int)reader["CorrectOptionIndex"]
        });
    }
    return tests;
}


    public void DeleteTest(int testId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "DELETE FROM Test WHERE TestId = @testId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@testId", testId);
        command.ExecuteNonQuery();
    }

   public Test GetTestById(int testId)
{
    Test test = null;

    using var connection = new NpgsqlConnection(_connectionString);
    connection.Open();

    var query = "SELECT * FROM Test WHERE TestId = @testId";
    using var command = new NpgsqlCommand(query, connection);
    command.Parameters.AddWithValue("@testId", testId);

    using var reader = command.ExecuteReader();
    if (reader.Read())
    {
        test = new Test
        {
            TestId = (int)reader["TestId"],
            SubjectId = (int)reader["SubjectId"],
            Question = reader["Question"].ToString(),
            // Fetch the text[] from PostgreSQL and cast it to string[]
            Options = (string[])reader["Options"], // Properly cast to string[]
            CorrectOptionIndex = (int)reader["CorrectOptionIndex"]
        };
    }

    return test;
}


}
