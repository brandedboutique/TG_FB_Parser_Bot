using FreelanceBotBase.Bot.Commands.Default;
using FreelanceBotBase.Bot.Commands.Interface;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Handlers.Update
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly ICommand _command;

        public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, DefaultCommand command)
        {
            _botClient = botClient;
            _logger = logger;
            _command = command;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { ChannelPost: { } message } => BotOnMessageReceived(message, cancellationToken),
                { EditedChannelPost: { } message } => BotOnMessageReceived(message, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }
        #region Bot Processors
        private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } && message.Caption is not { })
                return;

            string postId = await _command.ExecuteAsync(message, cancellationToken);
            _logger.LogInformation("The post was published with id: {PostId}", postId);
        }

        private Task UnknownUpdateHandlerAsync(Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        #endregion
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

            // Cooldown in case of network troubles.
            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
