namespace FreelanceBotBase.Infrastructure.Settings
{
    /// <summary>
    /// Provides configuration options for Telegram bot.
    /// </summary>
    public class BotConfiguration
    {
        /// <summary>
        /// Configuration route.
        /// </summary>
        public static readonly string Configuration = "BotConfiguration";

        public string BotToken { get; set; } = string.Empty;
    }
}
