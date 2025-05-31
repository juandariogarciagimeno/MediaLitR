using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Models;
using MediaLitr.Abstractions.Pipelines;
using MediaLitr.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MediaLitr.Test
{
    public class Tests
    {
        public record TestQuery(string Input) : IQuery<string>;
        public record TestCommand(string Input) : ICommand<string>;
        public record VoidCommand(string Input) : ICommand;

        [Fact]
        public async Task QueryAsync_ShouldInvokeQueryHandler()
        {
            var query = new TestQuery("hello");
            var mockHandler = new Mock<IQueryHandler<TestQuery, string>>();
            mockHandler.Setup(h => h.HandleAsync(query, It.IsAny<CancellationToken>()))
                       .ReturnsAsync("result");

            var services = new ServiceCollection();
            services.AddSingleton(mockHandler.Object);
            var provider = services.BuildServiceProvider();

            var options = Options.Create(new PipelineConfig());
            var mediator = new MediaLitR(provider, options);

            var result = await mediator.QueryAsync<TestQuery, string>(query);

            Assert.Equal("result", result);
        }

        [Fact]
        public async Task SendAsync_WithResult_ShouldInvokeCommandHandler()
        {
            var command = new TestCommand("test");
            var mockHandler = new Mock<ICommandHandler<TestCommand, string>>();
            mockHandler.Setup(h => h.HandleAsync(command, It.IsAny<CancellationToken>()))
                       .ReturnsAsync("done");

            var services = new ServiceCollection();
            services.AddSingleton(mockHandler.Object);
            var provider = services.BuildServiceProvider();

            var options = Options.Create(new PipelineConfig());
            var mediator = new MediaLitR(provider, options);

            var result = await mediator.SendAsync<TestCommand, string>(command);

            Assert.Equal("done", result);
        }

        [Fact]
        public async Task SendAsync_ShouldInvokeVoidCommandHandler()
        {
            var command = new VoidCommand("noop");
            var mockHandler = new Mock<ICommandHandler<VoidCommand>>();
            mockHandler.Setup(h => h.HandleAsync(command, It.IsAny<CancellationToken>()))
                       .Returns(Unit.CompletedTask);

            var services = new ServiceCollection();
            services.AddSingleton(mockHandler.Object);
            var provider = services.BuildServiceProvider();

            var options = Options.Create(new PipelineConfig());
            var mediator = new MediaLitR(provider, options);

            await mediator.SendAsync(command);
            mockHandler.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SendAsync_ShouldUsePipelineBehavior()
        {
            var command = new TestCommand("pipeline");
            var mockHandler = new Mock<ICommandHandler<TestCommand, string>>();
            mockHandler.Setup(h => h.HandleAsync(command, It.IsAny<CancellationToken>()))
                       .ReturnsAsync("handled");

            var mockPipeline = new Mock<IPipelineBehavior<TestCommand, string>>();
            mockPipeline
                .Setup(p => p.HandleAsync(command, It.IsAny<CancellationToken>(), It.IsAny<RequestDelegate<string>>()))
                .Returns((TestCommand cmd, CancellationToken ct, RequestDelegate<string> next) =>
                {
                    return next(ct); // simply forwards the call
                });

            var services = new ServiceCollection();
            services.AddSingleton(mockHandler.Object);
            services.AddSingleton(mockPipeline.Object);
            var provider = services.BuildServiceProvider();

            var options = Options.Create(new PipelineConfig());
            var mediator = new MediaLitR(provider, options);

            var result = await mediator.SendAsync<TestCommand, string>(command);

            Assert.Equal("handled", result);
            mockPipeline.Verify(p => p.HandleAsync(command, It.IsAny<CancellationToken>(), It.IsAny<RequestDelegate<string>>()), Times.Once);
        }

        [Fact]
        public async Task QueryAsync_ShouldThrow_WhenNoHandlerRegistered()
        {
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var options = Options.Create(new PipelineConfig());
            var mediator = new MediaLitR(provider, options);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                mediator.QueryAsync<TestQuery, string>(new TestQuery("fail")));
        }

        [Fact]
        public async Task SendAsync_ShouldUseGenericPipeline()
        {
            var command = new TestCommand("hello");
            var mockHandler = new Mock<ICommandHandler<TestCommand, string>>();
            mockHandler.Setup(h => h.HandleAsync(command, It.IsAny<CancellationToken>()))
                       .ReturnsAsync("core");

            var services = new ServiceCollection();
            services.AddSingleton(mockHandler.Object);
            services.AddTransient(typeof(GenericPipeline<,>));

            var provider = services.BuildServiceProvider();
            var config = new PipelineConfig
            {
                GenericPipelines = [typeof(GenericPipeline<,>)]
            };
            var options = Options.Create(config);
            var mediator = new MediaLitR(provider, options);

            var result = await mediator.SendAsync<TestCommand, string>(command);

            Assert.Equal("core", result);
        }

        // Generic pipeline used for testing
        public class GenericPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
        {
            public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, RequestDelegate<TResponse> next)
            {
                return next(cancellationToken); // Just forwards the call
            }
        }
    }
}
