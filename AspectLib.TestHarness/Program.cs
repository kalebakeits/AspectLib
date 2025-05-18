using AspectLib.ServiceResolver;
using AspectLib.TestHarness.AspectLibConfig;
using AspectLib.TestHarness.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices().AddHttpContextAccessor().AddControllers();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

AspectServiceResolver.Resolver = new CustomServiceResolver(app.Services);

app.Run();
