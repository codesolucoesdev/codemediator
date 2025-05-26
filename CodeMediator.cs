using CodeMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMediator;

public sealed class CodeMediator(IServiceProvider serviceProvider) : ICodeMediator
{
    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : ICodeNotification
    {
        var handlerType = typeof(ICodeNotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = serviceProvider.GetServices(handlerType);        

        foreach (var handler in handlers)
        {
            await (Task)handlerType
                .GetMethod("Handle")!
                .Invoke(handler, [notification, cancellationToken])!;
        }
    }

    public async Task<TResponse> SendAsync<TResponse>(ICodeRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICodeRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetService(handlerType);
        return handler is null
            ? throw new InvalidOperationException($"Handler {handlerType} not found")
            : await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [request, cancellationToken])!;
    }
}
