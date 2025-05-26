using CodeMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CodeMediator;

public sealed class CodeMediator : ICodeMediator
{
    private readonly IServiceProvider serviceProvider;

    public CodeMediator(IServiceProvider services) => serviceProvider = services;

    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : ICodeNotification
    {
        var handlerType = typeof(ICodeNotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            await (Task)handlerType
                .GetMethod("Handle")!
                .Invoke(handler, new object[] { notification, cancellationToken })!;
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
            .Invoke(handler, new object[] { request, cancellationToken })!;
    }
}
