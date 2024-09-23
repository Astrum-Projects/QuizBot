public interface IUserRepository
{
    void AddNewUser(User user);
    User GetUserByChatId(long chatId);
    IEnumerable<User> GetAllUsers();
}
