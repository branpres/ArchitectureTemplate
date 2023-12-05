var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRequestHandlers();
builder.Services.AddDomainEvents();
builder.Services.AddValidatorsFromAssembly(System.Reflection.Assembly.Load("ArchitectureTemplate.Application"));

JsonConvert.DefaultSettings = () => new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddDbContext<TemplateDbContext>(options =>
{
    var connection = new SqliteConnection("Data Source=TemplateDB;Mode=Memory;Cache=Shared");
    connection.Open();
    options.UseSqlite(connection);
    options.EnableSensitiveDataLogging();    
});

builder.Services.Configure<ILogger>(x =>
{
    
});

builder.Services.AddHostedService<DomainEventOutboxProcessor>();

var app = builder.Build();

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