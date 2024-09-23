public class TestService
{
    private readonly ITestRepository _testRepository;

    public TestService(ITestRepository testRepository)
    {
        _testRepository = testRepository;
    }

    public void CreateTest(Test test)
    {
        _testRepository.AddTest(test);
    }

    public IEnumerable<Test> GetTestsBySubjectId(int subjectId)
    {
        return _testRepository.GetTestsBySubjectId(subjectId);
    }

    public void DeleteTest(int testId)
    {
        _testRepository.DeleteTest(testId);
    }
}
