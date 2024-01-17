using Facebook;
using FreelanceBotBase.Bot.Commands.Default;
using FreelanceBotBase.Bot.Handlers.Update;
using FreelanceBotBase.Bot.Services.Polling;
using FreelanceBotBase.Bot.Services.Receiver;
using FreelanceBotBase.Infrastructure.Configuration;
using FreelanceBotBase.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<BotConfiguration>(
            context.Configuration.GetSection(BotConfiguration.Configuration));

        services.Configure<FacebookConfiguration>(
            context.Configuration.GetSection(FacebookConfiguration.Configuration));

        services.Configure<ReceiverOptions>(
            context.Configuration.GetSection(nameof(ReceiverOptions)));

        services.AddHttpClient("telegram_bot_client")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                TelegramBotClientOptions options = new(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

        services.AddSingleton(sp =>
        {
            FacebookConfiguration? fbConfig = sp.GetConfiguration<FacebookConfiguration>();
            return new FacebookClient(fbConfig.AccessToken);
        });

        services.AddScoped<DefaultCommand>();
        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();
    })
    .Build();

await host.RunAsync();