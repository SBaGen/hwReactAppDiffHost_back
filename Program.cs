var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddCors(options => options.AddPolicy("AllowPolicy",
    builder =>
    {
        builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }));

var app = builder.Build();

app.UseCors("AllowPolicy");

// Логирование
app.Use(async (context, next) =>
{
    await next();
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();