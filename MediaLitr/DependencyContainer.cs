using MediaLitr.Abstractions;
using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Pipelines;
using MediaLitr.Config;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MediaLitr
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddMediaLitrForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                throw new ArgumentException("At least one assembly must be provided.", nameof(assemblies));
            }

            var handlerInterfaces = new[]
            {
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)
            };

            var types = assemblies.SelectMany(x => x.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && handlerInterfaces
                        .Any(h => h.IsGenericTypeDefinition && i.GetGenericTypeDefinition() == h));

                foreach (var iface in interfaces)
                {
                    services.AddTransient(iface, type);
                }
            }

            services.AddSingleton<IMediator, MediaLitr>(); 
            services.AddSingleton(sp => sp);

            return services;
        }

        public static IServiceCollection AddBehaviorsForAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                throw new ArgumentException("At least one assembly must be provided.", nameof(assemblies));
            }
            var behaviorInterface = typeof(IPipelineBehavior<,>);

            var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Where(type => type.GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == behaviorInterface));

            List<Type> genericTypes = new();

            foreach (var type in types)
            {
                if (type.IsGenericType)
                {
                    genericTypes.Add(type);
                }

                else
                {
                    var iface = type.GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == behaviorInterface);

                    if (iface != null)
                        services.AddTransient(iface, type);
                }
            }
            
            services.Configure<PipelineConfig>(config =>
            {
                config.GenericPipelines = genericTypes;
            });


            return services;
        }
    }
}
