using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace Herald.MediatR.Retry.Tests
{
    [RetryPolicy]
    public class Command : IRequest<bool>
    {
    }

    public class Handler : IRequestHandler<Command, bool>
    {
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new ArgumentException();
            return await Task.FromResult(true);
        }
    }

    public class RetryPolicyBehaviorTests
    {
        [Fact]
        public async Task ShouldThrowsApplicationExceptionWhenReachMaxRetryCount()
        {
            //Arrange
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryPolicyBehavior<,>));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            var mediator = services.BuildServiceProvider().GetService<IMediator>();
            var cmd = new Command();

            //Act
            Func<Task> act = () => mediator.Send(cmd);

            //Assert
            Assert.ThrowsAsync<ApplicationException>(act);
        }
    }
}
