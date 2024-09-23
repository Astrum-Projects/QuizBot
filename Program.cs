using Telegram.Bot;

internal class Program
{
    public static void Main(string[] args)
    {
        string Token  = "7307431261:AAER4L9GWMsHxYrjldqnryHcAXBIU_BNTRA";
        var updateHandle = new UpdateHandeler();
        var botClient = new TelegramBotClient(Token);
        botClient.StartReceiving(updateHandler: updateHandle.HandleUpdate,
                    pollingErrorHandler: updateHandle.HandleError,
                    receiverOptions: new Telegram.Bot.Polling.ReceiverOptions() { ThrowPendingUpdates = true },
                    cancellationToken: default
        );
        Console.ReadLine();
    }
}

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         // Create repository instances
//         IUserRepository userRepository = new UserRepository();
//         ITestRepository testRepository = new TestRepository();
//         ISubjectRepository subjectRepository = new SubjectRepository();

//         // Testing UserRepository
//         Console.WriteLine("Testing UserRepository:");
//         var user1 = new User { Username = "Alice", ChatId = 12345, UserContact = "998763250" };
//         userRepository.AddNewUser(user1);
//         var retrievedUser = userRepository.GetUserByChatId(12345);
//         Console.WriteLine(retrievedUser != null ? $"User found: {retrievedUser.Username}" : "User not found");
        
//         var allUsers = userRepository.GetAllUsers();
//         Console.WriteLine($"Total Users: {allUsers.Count()}");

//         // Testing SubjectRepository
//         Console.WriteLine("\nTesting SubjectRepository:");
//         var subject1 = new Subject { Name = "Mathematics", PointsPerCorrectAnswer = 5 };
//         subjectRepository.AddSubject(subject1);
//         var allSubjects = subjectRepository.GetAllSubjects();
//         Console.WriteLine($"Total Subjects: {allSubjects.Count()}");

//         // Testing TestRepository
//         Console.WriteLine("\nTesting TestRepository:");
//         var test1 = new Test
//         {
//             SubjectId = 1,
//             Question = "What is 2 + 2?",
//             Options = new[] { "3", "4", "5" },
//             CorrectOptionIndex = 1
//         };
//         testRepository.AddTest(test1);
//         var testsBySubject = testRepository.GetTestsBySubjectId(1);
//         Console.WriteLine($"Total Tests for SubjectId 1: {testsBySubject.Count()}");
        
//         // Deleting a test
//         testRepository.DeleteTest(test1.TestId);
//         var testsAfterDelete = testRepository.GetTestsBySubjectId(1);
//         Console.WriteLine($"Total Tests for SubjectId 1 after deletion: {testsAfterDelete.Count()}");
//     }
// }
