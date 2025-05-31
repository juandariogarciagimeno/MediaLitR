using MediaLitr;
using MediaLitr.Abstractions;
using MediaLitr.Console.Features.Dtos.Commands;
using MediaLitr.Console.Features.Dtos.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddMediaLitrForAssemblies(typeof(Program).Assembly)
    .AddBehaviorsForAssemblies(typeof(Program).Assembly);

var app = builder.Build();

await app.StartAsync();


var sender = app.Services.GetRequiredService<IMediator>();
var res = await sender.QueryAsync<GetAllNamesQuery, GetAllNamesResponse>(new GetAllNamesQuery());
Console.WriteLine(string.Join(", ", res.Names));

var res2 = await sender.SendAsync<ProcessOrderCommand, ProcessOrderResponse>(new ProcessOrderCommand()
{
    Amount = 100,
    OrderId = Guid.NewGuid().ToString(),
    CustomerId = Guid.NewGuid().ToString()
});
Console.WriteLine($"Order processed: {res2.IsSuccess}, Message: {res2.Message}");

await sender.SendAsync(new SendConfirmationMailCommand()
{
    Body = "This is a test email body.",
    Subject = "Test Email",
    Email = "test@test.com"
});

Console.WriteLine("Commands and queries executed successfully.");

await app.WaitForShutdownAsync();