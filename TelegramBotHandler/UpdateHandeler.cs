using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class UpdateHandeler
{
    public async Task HandleUpdate(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        var task = update.Type switch
        {
            UpdateType.Message => HandleMessageAsync(telegramBotClient, update, cancellationToken),
            UpdateType.CallbackQuery => HandleCallbackQueryAsync(telegramBotClient, update, cancellationToken),
            _ => HandleUnknown(telegramBotClient, update, cancellationToken)
        };
    }

    private async Task HandleMessageAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        var messageHandeler = new MessageHandeler();
        var sentMessage = message.Type switch
        {
            MessageType.Text => messageHandeler.HandleTextMessageAsync(telegramBotClient, message, cancellationToken),
            MessageType.Contact => messageHandeler.HandleContactMessageAsync(telegramBotClient, message, cancellationToken),
            _ => messageHandeler.HandleUnknownMessageAsync(telegramBotClient, message, cancellationToken)
        };
    }

    private async Task HandleUnknown(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    public async Task HandleCallbackQueryAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        // Console.WriteLine("Received callback query: " + update.CallbackQuery.Data);

        var callbackQuery = update.CallbackQuery;
        if (callbackQuery.Data.StartsWith("subject_"))
    {
        // Handle subject selection
        await CommandsHandeler.HandleSubjectSelection(telegramBotClient, callbackQuery, cancellationToken);
    }
    else if (callbackQuery.Data.StartsWith("test_"))
    {
        // Handle answer selection
        await CommandsHandeler.HandleAnswerSelection(telegramBotClient, callbackQuery, cancellationToken);
    }
    }
    public async Task HandleError(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken)
    {

    }
}
