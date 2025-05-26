using CodeMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeMediator.Abstractions;

public static class CodeMediatorExtensions
{
    public static IServiceCollection AddCodeMeditor(this IServiceCollection services) => services.AddSingleton<ICodeMediator, CodeMediator>();

    public static IServiceCollection AddCodeMeditor(this IServiceCollection services, Assembly assembly)
    {
        AddCodeMeditor(services);

        // Registrando todos os IQueryHandler<>
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(ICodeRequestHandler<,>) ||
                             i.GetGenericTypeDefinition() == typeof(ICodeNotificationHandler<>)))
                .Select(i => new { Service = i, Implementation = t }));

        foreach (var handler in handlerTypes)
            services.AddScoped(handler.Service, handler.Implementation);

        return services;
    }
}