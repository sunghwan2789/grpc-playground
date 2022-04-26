using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGrpcService<CoreServiceV1>();

app.Run();
