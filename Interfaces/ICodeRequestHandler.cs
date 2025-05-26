namespace CodeMediator.Interfaces;

public  interface ICodeRequestHandler<in TRequest, TResponse> where TRequest : ICodeRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
