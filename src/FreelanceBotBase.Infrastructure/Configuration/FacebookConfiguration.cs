﻿namespace FreelanceBotBase.Infrastructure.Configuration
{
    /// <summary>
    /// Provides configuration options for Facebook API.
    /// </summary>
    public class FacebookConfiguration
    {
        /// <summary>
        /// Configuration route.
        /// </summary>
        public static readonly string Configuration = nameof(FacebookConfiguration);

        public string AccessToken { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
    }
}
