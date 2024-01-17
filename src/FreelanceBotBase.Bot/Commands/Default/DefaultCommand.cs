using Facebook;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Default
{
    public class DefaultCommand : CommandBase
    {
        private readonly FacebookClient _fbClient;
        private readonly HttpClient _httpClient;
        private readonly string _botToken;
        private readonly string _groupId;
        private readonly string _pageId;

        public DefaultCommand(
            ITelegramBotClient botClient,
            FacebookClient fbClient,
            IHttpClientFactory httpClientFactory,
            IOptions<FacebookConfiguration> fbConfig,
            IOptions<BotConfiguration> tgConfig)
            : base(botClient)
        {
            _fbClient = fbClient;
            _httpClient = httpClientFactory.CreateClient();
            _botToken = tgConfig.Value.BotToken;
            _groupId = fbConfig.Value.GroupId;
            _pageId = fbConfig.Value.PageId;
        }

        public override async Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>
            {
                { "message", message.Text ?? message.Caption! }
            };

            await ProcessImages(message, parameters, cancellationToken);

            dynamic? postResult = (message.Video != null)
                ? await ProcessVideos(message, (string)parameters["message"], cancellationToken)
                : await _fbClient.PostTaskAsync($"{_pageId}/feed", parameters, cancellationToken);

            if (postResult != null && postResult!.id != null)
                return postResult!.id;
            return string.Empty;
        }

        private async Task ProcessImages(Message message, Dictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            if (message.Photo != null)
            {
                var photoIds = new List<object>();
                var maxPhotos = message.Photo
                    .GroupBy(p => p.FileUniqueId[..10])
                    .Select(g => g.OrderByDescending(p => p.FileSize).First())
                    .ToList();

                foreach (var photo in maxPhotos)
                {
                    var photoFile = await BotClient.GetFileAsync(photo.FileId, cancellationToken);
                    var imageBytes = await _httpClient.GetByteArrayAsync($"https://api.telegram.org/file/bot{_botToken}/" + photoFile.FilePath,
                        cancellationToken);
                    var photoParameters = new Dictionary<string, object>
                    {
                        { "source", new FacebookMediaObject { ContentType = "image/jpeg", FileName = "image.jpg" }.SetValue(imageBytes) },
                        { "published", false }
                    };

                    dynamic result = await _fbClient.PostTaskAsync($"{_pageId}/photos", photoParameters, cancellationToken);

                    if (result != null && result!.id != null)
                    {
                        photoIds.Add(new { media_fbid = result!.id });
                    }
                }

                parameters.Add("attached_media", photoIds);
            }
        } 
        
        private async Task<object?> ProcessVideos(Message message, string caption, CancellationToken cancellationToken)
        {
            var videoFile = await BotClient.GetFileAsync(message.Video!.FileId, cancellationToken);
            var videoBytes = await _httpClient.GetByteArrayAsync($"https://api.telegram.org/file/bot{_botToken}/" + videoFile.FilePath,
                cancellationToken);
            var videoParameters = new Dictionary<string, object>
            {
                { "source", new FacebookMediaObject { ContentType = "video/mp4", FileName = "video.mp4" }.SetValue(videoBytes) },
                { "published", true },
                { "no_story", false },
                { "description", caption }
            };

            dynamic result = await _fbClient.PostTaskAsync($"{_pageId}/videos", videoParameters, cancellationToken);

            return result;
        }
    }
}

