var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .AddSwaggerGen()
    .AddEndpointsApiExplorer()
    .AddExceptionHandler<ExceptionHandler>()
    .AddHttpContextAccessor()
    .AddValidatorsFromAssembly(System.Reflection.Assembly.Load("ArchitectureTemplate.Application"))
    .AddRequestHandlers()
    .AddScoped<ICurrentUser, CurrentUser>()
    .AddHostedService<DomainEventOutboxProcessor>()
    .AddDomainEvents()
    .AddDbContext<TemplateDbContext>(options =>
    {
        var connection = new SqliteConnection("Data Source=TemplateDB;Mode=Memory;Cache=Shared");
        connection.Open();
        options.UseSqlite(connection);
        options.EnableSensitiveDataLogging();
    });

JsonConvert.DefaultSettings = () => new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

var app = builder.Build();

app.UseExceptionHandler(options => { });
app.UseMiddleware<CurrentUserMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

using var scope = app.Services.CreateScope();
using var dbContext = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();
await dbContext.Database.EnsureCreatedAsync();

app.Run();