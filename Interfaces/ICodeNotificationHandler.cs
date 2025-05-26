namespace CodeMediator.Interfaces;

public interface ICodeNotificationHandler<in TNotification> where TNotification : ICodeNotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}