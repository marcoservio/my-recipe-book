using Azure.Messaging.ServiceBus;

using MyRecipeBook.Application.UseCase.User.Delete.Delete;

namespace MyRecipeBook.API.BackgroundServices;

public class DeleteUserService(IServiceProvider serviceProvider, ServiceBusProcessor processor) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ServiceBusProcessor _processor = processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;

        _processor.ProcessErrorAsync += ExceptionReceivedHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
    {
        var message = eventArgs.Message.Body.ToString();

        var userIdentifier = Guid.Parse(message);

        var scope = _serviceProvider.CreateScope();

        var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();

        await deleteUserUseCase.Execute(userIdentifier);
    }

    private static Task ExceptionReceivedHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

    ~DeleteUserService() => Dispose();
}
