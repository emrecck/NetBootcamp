using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace NetBootcamp.Services.Users;

public class BackgroundServiceEmailSender(Channel<UserCreatedEvent> userCreatedEventChannel, ILogger<BackgroundServiceEmailSender> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await userCreatedEventChannel.Reader.WaitToReadAsync(stoppingToken))
        {
            // Hata olması durumunda uygulama shot down olacağı için try-catch bloğu ekledik
            // Böylece hata olsa bile diğer eventlerin işlenmesine devam edilecek
            // Background service ler singleton olduğu için hata durumunda uygulama shut down olur
            try
            {
                var userCreatedEvent = await userCreatedEventChannel.Reader.ReadAsync(stoppingToken);

                logger.LogInformation("Sending welcome email to {Email}", userCreatedEvent.Email);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Error occurred while sending welcome email");
            }
           
        }
    }
}