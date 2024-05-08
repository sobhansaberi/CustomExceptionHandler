using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(PatternCollection.Instance());
builder.Services.AddPersistence();
builder.Services.AddSingleton<MerchantCollection>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseExceptionHandlerMiddleware();
app.UseCustomAuthenticationMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
