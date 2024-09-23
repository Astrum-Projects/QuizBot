public interface ISubjectRepository
{
    void AddSubject(Subject subject);
    IEnumerable<Subject> GetAllSubjects();
    void DeleteSubject(int subjectId);
}
