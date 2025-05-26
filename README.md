
CodeMediator
============

CodeMediator é uma alternativa leve, gratuita e moderna ao MediatR para .NET. 
Ideal para aplicações que seguem o padrão CQRS, Clean Architecture ou simplesmente desejam desacoplar 
a comunicação entre componentes, sem depender de bibliotecas pesadas ou comerciais.

✨ Principais recursos
---------------------
• Suporte a Commands, Queries e Notifications
• Baixa dependência, alta performance
• Pronto para testes e fácil de mockar
• Sem reflection, proxies ou magia escondida
• Compatível com .NET 6, .NET 7 e superior

🚀 Instalação
-------------
dotnet add package CodeMediator

🛠️ Exemplo de uso
------------------
1. Defina seu Query e Handler:

public record GetUserByIdQuery(Guid Id) : IQuery<UserDto>;

public class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new UserDto(request.Id, "João"));
    }
}

2. Use o IMediator no controller ou serviço:

public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }
}

🧩 Integração com DI
--------------------
Registre tudo de forma automática:

services.AddCodeMediator();

ou 

services.AddCodeMediator(assemblyContainingHandlers: typeof(MyClass).Assembly);

📦 Roadmap
----------
• [x] Suporte a ICommand e IQuery
• [x] Suporte a INotification
• [ ] Pipeline behaviors (pré e pós-execução)
• [ ] Filtros e políticas de exceção
• [ ] Documentação completa
• [ ] Benchmarks comparativos

📄 Licença
----------
Distribuído sob a licença MIT. Veja o arquivo LICENSE para mais detalhes.

🤝 Contribuindo
---------------
Contribuições são bem-vindas! Sinta-se à vontade para abrir uma issue, forkar o projeto ou enviar um pull request.

❤️ Agradecimentos
-----------------
Inspirado por projetos como MediatR, Brighter e MinimalApiPlayground.
