using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Herald.MediatR.Retry
{
    public static class Configurations
    {
        public static IServiceCollection AddRetryPolictyBehavior(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            services.TryAdd(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(RetryPolicyBehavior<,>), serviceLifetime));
            return services;
        }
    }
}
