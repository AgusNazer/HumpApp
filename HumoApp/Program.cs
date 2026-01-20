using DotNetEnv;
using HumoApp.Services;
Env.Load();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

//  Leer variables del .env
var mongoConnection = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE");

if (string.IsNullOrWhiteSpace(mongoConnection))
{
    throw new Exception("MongoDB connection string not configured");
}

if (string.IsNullOrWhiteSpace(databaseName))
{
    throw new Exception("MongoDB database name not configured");
}

// Registrar servicio MongoDB
builder.Services.AddSingleton<PythonAnalysisService>();
builder.Services.AddSingleton<IMongoAnalysisService>(sp => 
    new MongoAnalysisService(mongoConnection, databaseName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())  //  CAMBIAR A if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                  
    app.UseSwaggerUI();               
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();