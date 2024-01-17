using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Interface
{
    public interface ICommand
    {
        Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken);
    }
}
