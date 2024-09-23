interface IUserTestRepository
{
    public void SaveUserTest(UserTest userTest);
    public IEnumerable<UserTest> GetUserTests(int userId, int subjectId);
    
}