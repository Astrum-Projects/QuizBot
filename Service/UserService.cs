public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void RegisterUser(User user)
    {
        var existingUser = _userRepository.GetUserByChatId(user.ChatId);
        if (existingUser == null)
        {
            _userRepository.AddNewUser(user);
        }
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User GetUserByChatId(long chatId)
    {
        return _userRepository.GetUserByChatId(chatId);
    }
}
