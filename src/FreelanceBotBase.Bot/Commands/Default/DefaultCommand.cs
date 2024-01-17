using Facebook;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Domain.Models;
using FreelanceBotBase.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Default
{
    public class DefaultCommand : CommandBase
    {
        private readonly FacebookClient _fbClient;
        private readonly string _groupId;

        public DefaultCommand(ITelegramBotClient botClient, FacebookClient fbClient, IOptions<FacebookConfiguration> config) : base(botClient)
        {
            _fbClient = fbClient;
            _groupId = config.Value.GroupId;
        }

        public override async Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var parameters = new FacebookPostParameters
            {
                Message = message.Text
            };

            if (message.Photo != null)
            {
                var photoUrl = await BotClient.GetFileAsync(message.Photo.FirstOrDefault().FileId, cancellationToken: cancellationToken);
                parameters.Link = photoUrl.FilePath;
            }

            var result = await _fbClient.PostTaskAsync($"{_groupId}/feed", parameters, cancellationToken);

            var dict = (IDictionary<string, object>)result;
            if (dict.TryGetValue("id", out object? value))
                return value.ToString()!;

            return string.Empty;
        }
    }
}
