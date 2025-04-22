using Azure.Messaging.ServiceBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyRecipeBook.Application.UseCases.User.Delete.Delete;

namespace MyRecipeBook.CrossCutting.BackgroudServices;

public class DeleteUserService(IServiceProvider service, ServiceBusProcessor processor) : BackgroundService
{
    private readonly IServiceProvider _service = service;
    private readonly ServiceBusProcessor _processor = processor;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ExceptionRecieveHandler;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs eventArgs)
    {
        var mensagem = eventArgs.Message.Body.ToString();
        var userIdentifier = Guid.Parse(mensagem);

        var scope = _service.CreateScope();
        var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();
        await deleteUserUseCase.Execute(userIdentifier);
    }

    private static Task ExceptionRecieveHandler(ProcessErrorEventArgs _) => Task.CompletedTask;

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);
        await _processor.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}
