using Facebook;
using FreelanceBotBase.Bot.Commands.Base;
using FreelanceBotBase.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FreelanceBotBase.Bot.Commands.Default
{
    /// <summary>
    /// Command that activates every new post in Telegram channel.
    /// </summary>
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

        /// <summary>
        /// Executes default command. Parses Telegram post to Facebook post.
        /// </summary>
        /// <param name="message">Telegram message.</param>
        /// <param name="cancellationToken">Token to stop process.</param>
        /// <returns>Logs.</returns>
        public override async Task<string> ExecuteAsync(Message message, CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, object>
            {
                { "message", message.Text ?? message.Caption! }
            };

            await ProcessImages(message, parameters, cancellationToken);

            dynamic? postResult;
            if (message.Video != null || message.Animation != null)
            {
                string contentType = message.Video != null ? "video/mp4" : "image/gif";
                string fileName = message.Video != null ? "video.mp4" : "animation.gif";
                postResult = await ProcessMedia(message, (string)parameters["message"], contentType, fileName, cancellationToken);
            }
            else
            {
                postResult = await _fbClient.PostTaskAsync($"{_pageId}/feed", parameters, cancellationToken);
            }

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

        private async Task<object?> ProcessMedia(Message message, string caption, string contentType, string fileName, CancellationToken cancellationToken)
        {
            var mediaFile = await BotClient.GetFileAsync(message.Animation != null ? message.Animation!.FileId : message.Video!.FileId, cancellationToken);
            var mediaBytes = await _httpClient.GetByteArrayAsync($"https://api.telegram.org/file/bot{_botToken}/" + mediaFile.FilePath,
                cancellationToken);
            var mediaParameters = new Dictionary<string, object>
            {
                { "source", new FacebookMediaObject { ContentType = contentType, FileName = fileName }.SetValue(mediaBytes) },
                { "published", true },
                { "no_story", false },
                { "description", caption }
            };

            dynamic result = await _fbClient.PostTaskAsync($"{_pageId}/videos", mediaParameters, cancellationToken);

            return result;
        }
    }
}

