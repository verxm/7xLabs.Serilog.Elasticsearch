using Api.Tests.Accessors;
using Serilog;
using Serilog.Elk.POC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureLogger(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IServiceCollection ConfigureLogger(IServiceCollection services)
{
    var tenantHeaderAccessor = new TenantHeaderAccessorTest();

    var logger = new LoggerConfiguration()
        .AddDefaultConfiguration()
        .WithTenantIdEnricher(tenantHeaderAccessor)
        .CreateLogger();

    return services
        .AddHttpContextAccessor() // Required for TenantHeaderAccessorTest
        .AddLogging(lb => lb.AddSerilog(logger));
}