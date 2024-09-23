using Npgsql;

public class SubjectRepository : ISubjectRepository
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=QuizBot";

    public void AddSubject(Subject subject)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "INSERT INTO Subject (Name, PointsPerCorrectAnswer) VALUES (@name, @points)";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@name", subject.Name);
        command.Parameters.AddWithValue("@points", subject.PointsPerCorrectAnswer);
        command.ExecuteNonQuery();
    }

    public IEnumerable<Subject> GetAllSubjects()
    {
        var subjects = new List<Subject>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT * FROM Subject";
        using var command = new NpgsqlCommand(query, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            subjects.Add(new Subject
            {
                SubjectId = (int)reader["SubjectId"],
                Name = reader["Name"].ToString(),
                PointsPerCorrectAnswer = (int)reader["PointsPerCorrectAnswer"]
            });
        }
        return subjects;
    }

    public void DeleteSubject(int subjectId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var query = "DELETE FROM Subject WHERE SubjectId = @subjectId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@subjectId", subjectId);
        command.ExecuteNonQuery();
    }
}
