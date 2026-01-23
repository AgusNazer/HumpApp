using DotNetEnv;
using HumoApp.Services;

// Solo cargar .env en desarrollo local
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    Env.Load();
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// CORS para producción
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Leer variables del .env o de Render
var mongoConnection = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING") 
    ?? throw new Exception("MongoDB connection string not configured");
var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE") 
    ?? throw new Exception("MongoDB database name not configured");

// Registrar servicios
builder.Services.AddSingleton<PythonAnalysisService>();
builder.Services.AddSingleton<IMongoAnalysisService>(sp => 
    new MongoAnalysisService(mongoConnection, databaseName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                  
    app.UseSwaggerUI();               
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Puerto dinámico: local en dev, Render en prod
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");