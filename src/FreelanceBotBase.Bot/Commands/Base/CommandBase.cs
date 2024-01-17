using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Base
{
    /// <summary>
    /// Base class for Commands.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        protected readonly ITelegramBotClient BotClient;
        /// <summary>
        /// Constructs base command with Telegram Bot Client.
        /// </summary>
        /// <param name="botClient"></param>
        public CommandBase(ITelegramBotClient botClient) => BotClient = botClient;

        /// <summary>
        /// Executes base command.
        /// </summary>
        /// <param name="message">Telegram message.</param>
        /// <param name="cancellationToken">Token to stop process.</param>
        /// <returns>Logs.</returns>
        public abstract Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
