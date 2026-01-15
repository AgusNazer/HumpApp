using DotNetEnv;
Env.Load();



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
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
//Console.WriteLine($"Mongo: Conexion establecida con la DB");



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
