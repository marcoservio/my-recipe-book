namespace MyRecipeBook.Domain.Services.ServiceBus;

public interface IDeleteUserQueue
{
    Task SendMessage(Entities.User user);
}
