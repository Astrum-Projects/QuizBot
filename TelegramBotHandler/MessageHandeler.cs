using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

public class MessageHandeler
{
    public async Task HandleContactMessageAsync(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        IUserRepository userRepository = new UserRepository();
        var contact = message.Contact;
        userRepository.AddNewUser(new User
        {
            Username = contact.FirstName,
            ChatId = (long)contact.UserId,
            UserContact = contact.PhoneNumber,
        });
        var resivedMessage = await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Command that you can use\n /start - To start Bot\n /tests - To see available tests\n /results - To see every test you have done");
    }

    public async Task HandleTextMessageAsync(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Text == "/start")
        {
            await CommandsHandeler.HandleStartCommand(telegramBotClient, message, cancellationToken);
        }
        else if (message.Text == "/tests")
        {
            await CommandsHandeler.HandleTestCommand(telegramBotClient, message, cancellationToken);
        }
        else
        {
            var resivedMessage = await telegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Command that you can use\n /start - To start Bot\n /tests - To see available tests\n /results - To see every test you have done");
        }
    }

    public async Task HandleUnknownMessageAsync(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        await telegramBotClient.SendTextMessageAsync(chatId: message.Chat.Id, "Sorry but we are just ignoring your message.");
    }
}