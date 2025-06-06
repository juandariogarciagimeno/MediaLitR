﻿# MediaLitR

**MediaLitR** is a lightweight, opinionated implementation of the Mediator pattern in .NET, inspired by [MediatR](https://github.com/jbogard/MediatR).  
It provides a clean CQRS foundation with pipeline support and minimal setup.

---

[![NuGet - MediaLitr](https://img.shields.io/nuget/v/MediaLitr?style=flat-square&logo=nuget)](https://www.nuget.org/packages/MediaLitr)
[![NuGet - MediaLitr.Abstractions](https://img.shields.io/nuget/v/MediaLitr.Abstractions?style=flat-square&logo=nuget)](https://www.nuget.org/packages/MediaLitr.Abstractions)
![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blueviolet?style=flat-square&logo=dotnet)
![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue?style=flat-square&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)

---

## ✨ Features

- ✅ Minimalist mediator implementation
- ✅ Segregated Command and Query handlers (CQRS)
- ✅ Built-in support for pipeline behaviors (middlewares)
- ✅ Easy service registration 
---

## 📦 Installation

```bash
dotnet add package MediaLitr.Abstractions
dotnet add package MediaLitr
```

---

## 🚀 Usage

### 🧱 Define a Command and Handler

```csharp
public class ProcessOrderCommand : ICommand<ProcessOrderResponse>
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class ProcessOrderResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ProcessOrderHandler : ICommandHandler<ProcessOrderCommand, ProcessOrderResponse>
{
    public Task<ProcessOrderResponse> HandleAsync(ProcessOrderCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ProcessOrderResponse
        {
            IsSuccess = true,
            Message = $"Order {command.OrderId} has been processed successfully."
        });
    }
}
```

### 🛠 Register and Send a Command

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMediaLitrForAssemblies(typeof(Program).Assembly)
    .AddBehaviorsForAssemblies(typeof(Program).Assembly);

var app = builder.Build();

await app.StartAsync();

var mediator = app.Services.GetRequiredService<IMediator>();

var result = await mediator.SendAsync<ProcessOrderCommand, ProcessOrderResponse>(new ProcessOrderCommand()
{
    Amount = 100,
    OrderId = Guid.NewGuid().ToString(),
    CustomerId = Guid.NewGuid().ToString()
});

Console.WriteLine($"Order processed: {result.IsSuccess}, Message: {result.Message}");

await app.WaitForShutdownAsync();
```

---

## 🧩 Optional: Pipeline Behaviors

You can add cross-cutting concerns like logging, validation, etc., using custom pipeline behaviors.

```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
        => this.logger = logger;

    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, RequestDelegate<TResponse> next)
    {
        logger.LogInformation("Handling {RequestType}: {@Request}", typeof(TRequest).Name, request);
        var response = await next();
        logger.LogInformation("Handled {RequestType}", typeof(TRequest).Name);
        return response;
    }
}
```

---

## 📚 License

This project is licensed under the [MIT License](LICENSE.txt).

---

<p align="center">
  <em>Fast, lightweight, and extensible mediator — your CQRS foundation in .NET.</em><br/>
  ⭐ Star it if you like it!
</p>