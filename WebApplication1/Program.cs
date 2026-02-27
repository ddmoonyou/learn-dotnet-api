using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Custom middleware
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();

app.Run();
