using MediatR;

using Microsoft.Extensions.Logging;

using Polly;

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Herald.MediatR.Retry
{
    public class RetryPolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RetryPolicyBehavior<TRequest, TResponse>> _logger;

        public RetryPolicyBehavior(ILogger<RetryPolicyBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var retryAttr = typeof(TRequest).GetCustomAttribute<RetryPolicyAttribute>();
            if (retryAttr == null)
            {
                return await next();
            }

            var policyResult = await Policy.Handle<Exception>()
                .WaitAndRetryAsync(
                    retryAttr.RetryCount,
                    i => TimeSpan.FromMilliseconds(i * retryAttr.SleepDuration),
                    (ex, ts, _) => _logger.LogWarning(ex, "Failed to execute handler for request {Request}, retrying after {RetryTimeSpan}s: {ExceptionMessage}", typeof(TRequest).Name, ts.TotalSeconds, ex.Message))
                .ExecuteAndCaptureAsync(async () => await next());

            if (policyResult.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(policyResult.FinalException, $"Failed to execute handler for request {typeof(TRequest).Name}");
                throw new ApplicationException("FAILED", policyResult.FinalException);
            }

            return policyResult.Result;
        }
    }
}
