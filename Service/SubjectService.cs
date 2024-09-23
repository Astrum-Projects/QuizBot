public class SubjectService
{
    private readonly ISubjectRepository _subjectRepository;

    public SubjectService(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public void CreateSubject(Subject subject)
    {
        _subjectRepository.AddSubject(subject);
    }

    public IEnumerable<Subject> GetAllSubjects()
    {
        return _subjectRepository.GetAllSubjects();
    }

    public void DeleteSubject(int subjectId)
    {
        _subjectRepository.DeleteSubject(subjectId);
    }
}
