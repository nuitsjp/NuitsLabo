var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

var app = builder.Build();
app.UseRouting(); 
app.UseEndpoints(endpoints =>
{
    endpoints.MapMagicOnionService();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});
//app.MapGet("/", () => "Hello World!");

app.Run();
