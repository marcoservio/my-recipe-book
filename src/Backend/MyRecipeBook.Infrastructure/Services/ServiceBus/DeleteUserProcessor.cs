using Azure.Messaging.ServiceBus;

namespace MyRecipeBook.Infrastructure.Services.ServiceBus;

public class DeleteUserProcessor(ServiceBusProcessor processor)
{
    private readonly ServiceBusProcessor _processor = processor;

    public ServiceBusProcessor GetProcessor() => _processor;
}
