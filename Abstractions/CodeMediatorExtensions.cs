using CodeMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeMediator.Abstractions;

public static class CodeMediatorExtensions
{
    public enum CodeMediatorMode { Scoped, Transient, Singleton }

    public static IServiceCollection AddCodeMediator(this IServiceCollection services,
        CodeMediatorMode mode = CodeMediatorMode.Scoped)
    {
        switch (mode)
        {
            case CodeMediatorMode.Scoped:
            default:
                services.AddScoped<ICodeMediator, CodeMediator>();
                break;
            case CodeMediatorMode.Transient:
                services.AddTransient<ICodeMediator, CodeMediator>();
                break;
            case CodeMediatorMode.Singleton:
                services.AddSingleton<ICodeMediator, CodeMediator>();
                break;
        }

        return services;
    }

    public static IServiceCollection AddCodeMediator(this IServiceCollection services, Assembly assembly,
        CodeMediatorMode mode = CodeMediatorMode.Scoped)
    {
        AddCodeMediator(services, mode);

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