using FreelanceBotBase.Bot.Commands.Greeting;
using FreelanceBotBase.Bot.Commands.Interface;
using Telegram.Bot;

namespace FreelanceBotBase.Bot.Commands.Const
{
    public static class CommandsDictionary
    {
        public readonly static Dictionary<string, Func<ITelegramBotClient, ICommand>> All = new()
        {
            { "/hi", botClient => new GreetingCommand(botClient) },
        };
    }
}
