using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    /// <summary>
    /// Interface for Commands.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="message">Telegram message.</param>
        /// <param name="cancellationToken">Token to stop process.</param>
        /// <returns>Logs.</returns>
        Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
