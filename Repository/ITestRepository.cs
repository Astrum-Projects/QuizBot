public interface ITestRepository
{
    void AddTest(Test test);
    IEnumerable<Test> GetTestsBySubjectId(int subjectId);
    void DeleteTest(int testId);
    Test GetTestById(int testId);
}
