using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

public class CommandsHandeler
{
    private static int totalQuestions = 0;
    private static int totalAnsweredQuestions = 0;
    public static async Task HandleStartCommand(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        UserRepository userRepository = new UserRepository();
        if ((userRepository.GetUserByChatId(message.Chat.Id))== null)
        {

        var replyMarkup = new ReplyKeyboardMarkup(new[]
        {
                new KeyboardButton("Share Contact") { RequestContact = true }
            })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        var resivedMessage = await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Please share your contact information by clicking the button below.",
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken
        );
        }
         else
        {
            await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Command that you can use\n /start - To start Bot\n /tests - To see available tests\n /results - To see every test you have done");
        }
    }

    public static async Task HandleTestCommand(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {

        ISubjectRepository subjectRepository = new SubjectRepository();
        IEnumerable<Subject> subjects = subjectRepository.GetAllSubjects();


        var inlineKeyboard = new InlineKeyboardMarkup(subjects.Select(subject =>
            InlineKeyboardButton.WithCallbackData(subject.Name, $"subject_{subject.SubjectId}")
        ).ToArray());

        await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "All available subjects:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );
    }

    public static async Task HandleAnswerSelection(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        System.Console.WriteLine(callbackQuery.Data);
        // Parse callback data
        totalAnsweredQuestions++;
        var data = callbackQuery.Data.Split('_');
        var testId = int.Parse(data[1]); // Extract testId from callback data
        var userAnswerIndex = int.Parse(data[2]); // Extract option index

        ITestRepository testRepository = new TestRepository();
        Test selectedTest = testRepository.GetTestById(testId);

        // Assuming user ID is available from callbackQuery
        var userId = callbackQuery.From.Id;

        // Check if the user's answer is correct
        bool isCorrect = userAnswerIndex == selectedTest.CorrectOptionIndex;
        int score = isCorrect ? 1 : 0;

        // Save UserTest result
        IUserTestRepository userTestRepository = new UserTestRepository();
        var userTest = new UserTest
        {
            UserID = (int)userId,
            TestID = testId,
            SubjectId = selectedTest.SubjectId,
            UserAnswer = selectedTest.Options[userAnswerIndex], // The text of the answer chosen
            Score = score,
            TestDate = DateTime.Now
        };
        userTestRepository.SaveUserTest(userTest);
        // Delete the question message after the user answers
        await telegramBotClient.DeleteMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            cancellationToken: cancellationToken
        );
        // Feedback to the user about the answer
        var feedback = isCorrect ? "Correct!" : $"Wrong! The correct answer is {selectedTest.Options[selectedTest.CorrectOptionIndex]}.";

        if (totalQuestions == 0)
        {
            // Fetch total questions for this subject (log only once)
            totalQuestions = testRepository.GetTestsBySubjectId(selectedTest.SubjectId).Count();
            System.Console.WriteLine($"Total Questions for Subject {selectedTest.SubjectId}: {totalQuestions}");
        }
        // Fetch all user tests for this user and this subject
        var userTests = userTestRepository.GetUserTests((int)userId, (int)selectedTest.SubjectId);


        System.Console.WriteLine(totalAnsweredQuestions);
        // Check if the test is completed
        if (totalAnsweredQuestions >= totalQuestions)
        {

            var totalScore = userTests.Sum(ut => ut.Score);
            System.Console.WriteLine($"Total Score: {totalScore}");

            // Display the final score
            await telegramBotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Test completed! Your total score is: {totalScore}/{totalQuestions}",
                cancellationToken: cancellationToken
            );
            totalAnsweredQuestions = 0;
            totalQuestions = 0;
        }
    }






    // public static async Task HandleAnswerSelection(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    // {
    //     Console.WriteLine("Callback received");

    //     // Log callback data
    //     var data = callbackQuery.Data.Split('_');
    //     Console.WriteLine($"Callback data: {callbackQuery.Data}");

    //     // Parse testId and userAnswerIndex from callback data
    //     var testId = int.Parse(data[1]); // Extract testId from callback data
    //     var userAnswerIndex = int.Parse(data[2]); // Extract option index
    //     Console.WriteLine($"Parsed testId: {testId}, userAnswerIndex: {userAnswerIndex}");

    //     // Fetch the test by testId
    //     ITestRepository testRepository = new TestRepository();
    //     Test selectedTest = testRepository.GetTestById(testId);

    //     // Log if the test is null
    //     if (selectedTest == null)
    //     {
    //         Console.WriteLine("Test not found");
    //         await telegramBotClient.SendTextMessageAsync(
    //             chatId: callbackQuery.Message.Chat.Id,
    //             text: "Test not found.",
    //             cancellationToken: cancellationToken
    //         );
    //         return;
    //     }
    //     else
    //     {
    //         Console.WriteLine($"Test found: {selectedTest.Question}");
    //     }

    //     // Continue with the rest of the logic...
    //     // Check if the user's answer is correct
    //     bool isCorrect = userAnswerIndex == selectedTest.CorrectOptionIndex;
    //     int score = isCorrect ? 1 : 0;

    //     Console.WriteLine($"User selected option index: {userAnswerIndex}, correct option index: {selectedTest.CorrectOptionIndex}");
    //     Console.WriteLine(isCorrect ? "Answer is correct" : "Answer is wrong");

    //     // Save UserTest result (you may want to log this as well)
    //     IUserTestRepository userTestRepository = new UserTestRepository();
    //     var userTest = new UserTest
    //     {
    //         UserID = (int)callbackQuery.From.Id,  // Use callbackQuery.From.Id to get the Telegram user ID
    //         TestID = testId,
    //         UserAnswer = selectedTest.Options[userAnswerIndex], // The text of the answer chosen
    //         Score = score,
    //         TestDate = DateTime.Now
    //     };
    //     userTestRepository.SaveUserTest(userTest);

    //     // Send feedback to the user
    //     var feedback = isCorrect ? "Correct!" : $"Wrong! The correct answer is {selectedTest.Options[selectedTest.CorrectOptionIndex]}.";
    //     Console.WriteLine(feedback);
    //     try
    // {
    //     await telegramBotClient.SendTextMessageAsync(
    //         chatId: callbackQuery.Message.Chat.Id,
    //         text: feedback,
    //         cancellationToken: cancellationToken
    //     );
    //     Console.WriteLine($"Feedback sent to user: {feedback}");
    // }
    // catch (Exception ex)
    // {
    //     Console.WriteLine($"Error sending message: {ex.Message}");
    // }

    // }


    public static async Task HandleSubjectSelection(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var subjectId = int.Parse(callbackQuery.Data.Split('_')[1]); // Extract subjectId from callback data

        ITestRepository testRepository = new TestRepository();
        IEnumerable<Test> tests = testRepository.GetTestsBySubjectId(subjectId);

        foreach (var test in tests)
        {
            var optionsButtons = test.Options.Select((option, index) => InlineKeyboardButton.WithCallbackData(option, $"test_{test.TestId}_{index}")).ToArray();
            var optionsKeyboard = new InlineKeyboardMarkup(optionsButtons);

            await telegramBotClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: test.Question,
                replyMarkup: optionsKeyboard,
                cancellationToken: cancellationToken
            );
        }
    }

}