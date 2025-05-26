namespace CodeMediator.Interfaces;

public interface ICodeMediator
{
    Task<TResponse> SendAsync<TResponse>(ICodeRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : ICodeNotification;
}
