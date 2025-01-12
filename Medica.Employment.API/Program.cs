using Medica.Employment.Domain;
using Medica.Employment.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the repository as scoped service
builder.Services.AddScoped<IEmployeeRepository>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var filePath = configuration["CsvFiles:EmployeeData"]; // Path from appsettings.json
    return new EmployeeRepositoryCsv(filePath);
});

// Add Swagger services for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS API to allow requests from the Blazor app.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("http://localhost:26831") // Blazor app origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); ;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazor"); // Apply the CORS policy

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
